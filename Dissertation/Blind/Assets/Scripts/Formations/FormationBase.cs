using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationBase : MonoBehaviour
{
    [Header("Setup Variables")]
    public GameObject UnitPrefab;
    public int UnitTotalCount;
    private GameObject[] SpawnPoints;
    private bool FlippedGate;

    [Header("Formation Variables")]
    public int ActiveUnitCount;
    public Vector3 SlotOffset;
    public bool InCombat;
    public List<Vector3> SlotPositions;
    private List<Vector3> SlotOffsets;
    private List<GameObject> FightersInFormation;
    private FormationManager Manager;
    public float ReformTime;
    public float ReformDelay;
    private Quaternion LeadRotation;
    private Vector3 LeadPosition;
    private FriendlyMarker ThisHUDMarker;

    [Header("Movement Variables")]
    //General Movement
    private float RotationSpeed;
    private float MoveSpeed;
    //Random Movement
    public Vector3 RandomTarget;
    private float MinDirChange;
    private float MaxDirChange;
    private float NextChangeTime;

    [Header("Collision Variables")]
    public float CollisionRange;
    public float PlayerEngageRange;
    public float PlayerAvoidRange;
    private Quaternion AvoidRotation;
    private bool CurrentlyAvoiding;

    [Header("Sight Variables")]
    public float SightRange;
    private GameObject PlayerOBJ;
    private bool CanSpotPlayer;

    [Header("Used in some implementations")]
    public GameObject LeadUnit;
    public Rigidbody LeadBody;

    public AverageFrames TestScript;

    //----------------------------------------------- Formation Generation Functions ----------------------

    private void Start()
    {
        CanSpotPlayer = true;
        //Get all possible spawn points
        SpawnPoints = GameObject.FindGameObjectsWithTag("ThaalianJumpgate");
        //Set Delay to reform after damage
        ReformDelay = 60f;
        //Get Formation manager from parent object
        Manager = this.transform.parent.GetComponent<FormationManager>();
        //Create empty list to store slots
        SlotPositions = new List<Vector3>();
        SlotOffsets = new List<Vector3>();
        //Create empty list to store fighters
        FightersInFormation = new List<GameObject>();
        //Get Player OBJ
        PlayerOBJ = GameObject.FindGameObjectWithTag("Player");
        //Set Movement Variables
        RotationSpeed = 40f;
        MoveSpeed = 10f;
        MinDirChange = 10f;
        MaxDirChange = 45f;
        RandomTarget = new Vector3(0, 0, 0);
        NextChangeTime = Time.time + Random.Range(5f, 10f);
        AvoidRotation = new Quaternion();

        //Call Formation Generation
        GenerateFormation();
        this.transform.position += this.transform.forward * 2;

        RandomTarget = this.transform.position + (this.transform.forward * 400);
        ThisHUDMarker = this.GetComponent<FriendlyMarker>();

        TestScript = GameObject.FindGameObjectWithTag("MissionControl").GetComponent<AverageFrames>();
    }

    private void GenerateFormation()
    {
        //Move Formation to randomly selected Spawn Gate
        Transform LeadTransform = ChooseNewGate();
        //Make copy of transform and rotation
        LeadPosition = LeadTransform.position;
        LeadRotation = LeadTransform.rotation;
        //Set this position and rotation
        this.transform.position = LeadPosition;
        this.transform.rotation = LeadRotation;

        //Bool for Reverse Gates (prevents formations spawning backwards on half gates
        FlippedGate = false;

        //Add LeadPosition
        SlotPositions.Add(LeadPosition);
        SlotOffsets.Add(new Vector3(0, 0, 0));

        //Rotate formation depeding on spawn gate
        if (LeadTransform.rotation.eulerAngles.y == 180)
        {
            LeadRotation.eulerAngles = new Vector3(0, 180f, 0);
            FlippedGate = true;
        }
        else
        {
            LeadRotation.eulerAngles = new Vector3(0, 0f, 0);
            FlippedGate = false;
        }

        //Spawn Lead fighter
        LeadUnit = Instantiate(UnitPrefab, LeadPosition, LeadRotation);
        LeadBody = LeadUnit.GetComponentInChildren<Rigidbody>();
        LeadUnit.GetComponentInChildren<FighterHealth>().ThisFormationBase = this;
        //Add Fighter to list
        FightersInFormation.Add(LeadUnit);

        //For each fighter to add minus 1 for lead unit
        for (int i = 0; i < UnitTotalCount - 1; i++)
        {
            Vector3 NewSlotPosition = new Vector3();
            //Calculate Slot position for offset
            if (i < 2)
            {
                //Check if gate is flipped
                if (FlippedGate)
                {
                    NewSlotPosition = LeadPosition - SlotOffset;
                }

                else
                {
                    NewSlotPosition = LeadPosition + SlotOffset;
                }
            }
            else
            {
                //Check if gate is flipped
                if (FlippedGate)
                {
                    NewSlotPosition = SlotPositions[SlotPositions.Count - 2] - SlotOffset;
                }

                else
                {
                    NewSlotPosition = SlotPositions[SlotPositions.Count - 2] + SlotOffset;
                }
            }

            SlotOffset.x *= -1;
            //Add slot to list
            SlotPositions.Add(NewSlotPosition);
            //Store offset in list (Useful for updating slot position while in motion
            Vector3 Newoffset = this.transform.position - NewSlotPosition;
            SlotOffsets.Add(Newoffset);
            //Spawn Fighter at position
            GameObject SpawnedFighter = Instantiate(UnitPrefab, NewSlotPosition, LeadRotation);
            //Give fighter reference to this formation
            SpawnedFighter.GetComponentInChildren<FighterHealth>().ThisFormationBase = this;
            //Add Fighter to list
            FightersInFormation.Add(SpawnedFighter);
        }
        //Set Unit Count
        ActiveUnitCount = UnitTotalCount;

    }

    // Update is called once per frame
    public void UpdateFormation()
    {
        //Check if formation is currently in combat
        if (InCombat)
        {
            //Check if enough time has passed to start reforming
            if(Time.time >= ReformTime)
            {
                //Set to no longer in combat so formation will activate next update
                InCombat = false;
                ThisHUDMarker.MyMarker.enabled = true;
                for (int i = 0; i < FightersInFormation.Count; i++)
                {
                    GameObject Fighter = FightersInFormation[i];
                    Fighter.GetComponentInChildren<FriendlyMarker>().MarkerActive = false;
                }
            }

            //Else update each ship with it's solo behaviour
            else
            {
                foreach(GameObject Fighter in FightersInFormation)
                {
                    Fighter.transform.GetChild(1).GetComponent<FighterMovement>().SoloMovement();
                }
            }
        }

        //Else Update Position as 
        else
        {
            FormationSpecific();
        }
    }


    public void RemoveFighter(GameObject ToRemove)
    {
        //Check if lead unit needs replacing
        if(ToRemove == LeadUnit)
        {
            //Lead unit is always index 0, so set new lead to index 1
            LeadUnit = FightersInFormation[1];
        }
        //Remove Fighter from list
        //All units in slots above removed ship will then move down a slot
        FightersInFormation.Remove(ToRemove);
        //Decrease number of active fighters
        ActiveUnitCount -= 1;
        //Remove last slot in formation as it is now unoccupied
        SlotPositions.RemoveAt(SlotPositions.Count - 1);
        SlotOffsets.RemoveAt(SlotOffsets.Count - 1);

        //Check if there is only 1 unit remaining
        if (ActiveUnitCount == 1)
        {
            //Move fighter to closest formation and destroy this formation
            MoveFighterToOtherForm();
        }
        //Else enable individual ship markers
        else
        {
            for (int i = 0; i < FightersInFormation.Count; i++)
            {
                GameObject Fighter = FightersInFormation[i];
                Fighter.GetComponentInChildren<FriendlyMarker>().MarkerActive = true;
            }
            ThisHUDMarker.MarkerActive = false;
        }
        Destroy(ToRemove);
    }

    private void MoveFighterToOtherForm()
    {
        Debug.Log("Consolodating Ship to nearest");
        //Locate Nearest formation
        float NearestDistance = float.MaxValue;
        GameObject NearestFormation = null;
        foreach (GameObject OtherFormation in Manager.SpawnedFormations)
        {
            if (OtherFormation != this)
            {
                if (Vector3.Distance(OtherFormation.transform.position, this.transform.position) < NearestDistance)
                {
                    //Set new nearest distance
                    NearestDistance = Vector3.Distance(OtherFormation.transform.position, this.transform.position);
                    //Set nearest game object
                    NearestFormation = OtherFormation;
                }
            }
        }

        //Check Nearest formation isn't somehow == null
        if (NearestDistance != float.MaxValue)
        {
            //Add fighter into formation
            FormationBase FoundFormation = NearestFormation.GetComponent<FormationBase>();
            FoundFormation.AddFighter(FightersInFormation[0]);
        }

        //Otherwise allow fighter to fly solo
        else
        {
            FightersInFormation[0].transform.SetParent(null);
            FightersInFormation[0].transform.GetChild(1).GetComponent<FighterMovement>().enabled = true;
        }

        //Destroy formation
        Manager.SpawnedFormations.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void AddFighter(GameObject ToAdd)
    {
        //Use similar code as formation generation for new slot position
        Vector3 NewSlotPosition = new Vector3();
        //Check if formation has more than 3 fighters
        if (ActiveUnitCount < 3)
        {
            //Check if gate is flipped
            if (FlippedGate)
            {
                NewSlotPosition = SlotPositions[0] - SlotOffset;
            }

            else
            {
                NewSlotPosition = SlotPositions[0] + SlotOffset;
            }
        }
        else
        {
            //Check if gate is flipped
            if (FlippedGate)
            {
                NewSlotPosition = SlotPositions[SlotPositions.Count - 2] - SlotOffset;
            }

            else
            {
                NewSlotPosition = SlotPositions[SlotPositions.Count - 2] + SlotOffset;
            }
        }

        SlotPositions.Add(NewSlotPosition);
        SlotOffset.x *= -1;
        //Store offset in list (Useful for updating slot position while in motion
        Vector3 Newoffset = this.transform.position - NewSlotPosition;
        SlotOffsets.Add(Newoffset);
        //Add fighter to list of formation units
        FightersInFormation.Add(ToAdd);
        //Increase number of fighters in formation
        ActiveUnitCount += 1;
    }

    private Transform ChooseNewGate()
    {
        bool AcceptableGate = false;
        GameObject SelectedGate = null;
        while (!AcceptableGate)
        {
            int RandomIndex = Random.Range(0, SpawnPoints.Length - 1);
            SelectedGate = SpawnPoints[RandomIndex];
            SpawnGateBase SelectedBase = SelectedGate.GetComponent<SpawnGateBase>();
            if (SelectedBase.GateTaken == false)
            {
                SelectedBase.GateTaken = true;
                AcceptableGate = true;
                StartCoroutine(SelectedBase.TakenDelay());
            }
        }
        MeshRenderer GateCollider = SelectedGate.transform.GetChild(1).GetComponent<MeshRenderer>();
        StartCoroutine(EffectDisable(GateCollider));
        return SelectedGate.transform;
    }

    private IEnumerator EffectDisable(MeshRenderer Chosengate)
    {
        Chosengate.enabled = true;

        yield return new WaitForSeconds(5);

        Chosengate.enabled = false;
    }

    private void RandomMovement()
    {
        //Check if time to choose random movement point
        if(Time.time >= NextChangeTime || Vector3.Distance(LeadUnit.transform.position, RandomTarget) <= 10)
        {
            //Update Change Time
            NextChangeTime = Time.time + Random.Range(MinDirChange, MaxDirChange);
            //Update Random Position
            RandomPosition();
        }
        Vector3 VectToTarget = RandomTarget - LeadUnit.transform.position;
        if (Vector3.Angle(LeadUnit.transform.forward, VectToTarget) != 0)
        {
            var q = Quaternion.LookRotation(VectToTarget);
            LeadUnit.transform.rotation = Quaternion.RotateTowards(LeadUnit.transform.rotation, q, RotationSpeed * Time.deltaTime);
        }
    }

    private void RandomPosition()
    {
        //Get Random point somewhere in bounds of level
        RandomTarget.x = Random.Range(-750, 750);
        RandomTarget.y = Random.Range(-750, 750);
        RandomTarget.z = Random.Range(-750, 750);
    }

    //----------------------------------------------- Formation Update / Movement Functions ---------------

    private void FormationSpecific()
    {
        GameObject ObstacleOBJ;
        Vector3 HitPoint;

        //Obstacle Detection
        if (CheckForObstacle(out ObstacleOBJ, out HitPoint))
        {
            IdentifyObstacle(ObstacleOBJ, HitPoint);
        }
        //Else check if currently making turn to avoid obstacle
        else if (CurrentlyAvoiding)
        {
            ContinueAvoiding();
        }

        //Check for line of sight on player
        else if (CheckForSight())
        {
            TurnToPlayer();
        }

        //Random Movement
        else
        {
            RandomMovement();
        }

        LeadBody.AddForce(this.transform.forward * (MoveSpeed - 1), ForceMode.Impulse);

        //Update Slot positions
        UpdateSlotPositions();

        //Update Unit positions
        MoveUnitsToSlots();
    }

    private bool CheckForObstacle(out GameObject HitObstacle, out Vector3 HitPoint)
    {
        RaycastHit HitOut = new RaycastHit();

        //Raycast from each fighter
        foreach(GameObject Ship in FightersInFormation)
        {
            //Get point in front of ship
            Vector3 ShipFront = Ship.transform.position + Ship.transform.forward * 5;
            //Check for obstacle within set range
            if (Physics.Raycast(ShipFront, Ship.transform.forward, out HitOut, CollisionRange))
            {
                //Check if not already avoiding or if obstacle is too close anyway and not another enemy fighter
                if((CurrentlyAvoiding == false || HitOut.distance <= 75) && !HitOut.collider.transform.root.CompareTag("EnemyFighter"))
                {
                    //Set HitObstacle Output to object hit
                    HitObstacle = HitOut.collider.gameObject;
                    HitPoint = HitOut.point;
                    //Return True when obstacle
                    return true;
                }
            }
        }
        //If no fighter returns a hit
        HitObstacle = null;
        HitPoint = Vector3.zero;
        return false;
    }

    private void IdentifyObstacle(GameObject ObstacleOBJ, Vector3 HitPoint)
    {
        //Get Obstacle Tag
        string ObstacleTag = ObstacleOBJ.tag;
        float DistanceToObstacle = Vector3.Distance(this.transform.position, ObstacleOBJ.transform.position);

        //Act accordingly based on tag
        switch (ObstacleTag)
        {
            //If hit object is player
            case "Player":
                Debug.Log("Player is obstacle, check if in range to attack!");
                //Check if Distance is too close
                if (DistanceToObstacle <= PlayerAvoidRange)
                {
                    CanSpotPlayer = false;
                    //Set Avoid Rotation
                    AvoidRotation = Quaternion.LookRotation((LeadUnit.transform.root.position - HitPoint) * 0.5f);
                    //Set currently avoiding
                    CurrentlyAvoiding = true;
                    //Start rotation
                    LeadUnit.transform.rotation = Quaternion.RotateTowards(LeadUnit.transform.rotation, AvoidRotation, RotationSpeed * Time.deltaTime);
                    StartCoroutine(SpotPlayerDelay());
                }
                //Else check if in range to engage
                else if (DistanceToObstacle <= PlayerEngageRange)
                {
                    foreach (GameObject Ship in FightersInFormation)
                    {
                        Ship.GetComponentInChildren<FighterFiring>().FireCannons(ObstacleOBJ.transform.position);
                    }
                }
                //Otherwise formation will continue to move towards Player
                break;

            //If obstacle is other
            default:
                //Set Avoid Rotation
                AvoidRotation = Quaternion.LookRotation((LeadUnit.transform.root.position - ObstacleOBJ.transform.position) * 0.5f);
                //Set currently avoiding
                CurrentlyAvoiding = true;
                //Start rotation
                LeadUnit.transform.rotation = Quaternion.RotateTowards(LeadUnit.transform.rotation, AvoidRotation, RotationSpeed * Time.deltaTime);

                break;
        }
    }

    private void ContinueAvoiding()
    {
        //Check if arrived at rotation
        if (LeadUnit.transform.rotation == AvoidRotation)
        {
            //If yes, set currently avoiding to false
            CurrentlyAvoiding = false;
            //Update Change Time
            NextChangeTime = Time.time + Random.Range(MinDirChange, MaxDirChange);

            //Update Random Position
            RandomPosition();
        }
        else
        {
            //Continue to rotate towards
            LeadUnit.transform.rotation = Quaternion.RotateTowards(LeadUnit.transform.rotation, AvoidRotation, RotationSpeed * Time.deltaTime);
        }
    }

    private bool CheckForSight()
    {
        RaycastHit HitOut = new RaycastHit();
        //Check each fighter for line of sight
        foreach (GameObject Ship in FightersInFormation)
        {
            Vector3 ShipFront = Ship.transform.position + Ship.transform.forward * 8;
            Vector3 VectToPlayer = PlayerOBJ.transform.position - ShipFront;
            //Raycast towards player
            if (Physics.Raycast(ShipFront, VectToPlayer, out HitOut, SightRange) && CanSpotPlayer)
            {
                //Check if hit is player
                if (HitOut.collider.gameObject.transform.root.CompareTag("Player"))
                {
                    //Return True if line of sight made
                    return true;
                }
            }

        }
        //Return false otherwise
        return false;
    }

    private void TurnToPlayer()
    {
        Quaternion PlayerRotation = Quaternion.LookRotation(PlayerOBJ.transform.position - LeadUnit.transform.position);
        //Turn Formation towards Player
        LeadUnit.transform.rotation = Quaternion.RotateTowards(LeadUnit.transform.rotation, PlayerRotation, RotationSpeed * Time.deltaTime);
    }

    private void UpdateSlotPositions()
    {
        //Check slot positions exist
        if(SlotPositions.Count != 0)
        {
            Vector3 SlotHolder = new Vector3();
            //Update Lead Position
            SlotPositions[0] = LeadUnit.transform.position;
            this.transform.position = LeadUnit.transform.position;
            this.transform.rotation = LeadUnit.transform.rotation;

            if (FlippedGate)
            {
                //Loop through other positions and set to Lead Position + Relevant Offset
                for (int i = 1; i < SlotPositions.Count; i++)
                {
                    SlotHolder = SlotPositions[0] + SlotOffsets[i];
                    //Rotate slot around center to match current rotation
                    SlotPositions[i] = RotatePointAroundPivot(SlotHolder, LeadUnit.transform.position, LeadUnit.transform.eulerAngles);
                }
            }
            else
            {
                //Loop through other positions and set to Lead Position + Relevant Offset
                for (int i = 1; i < SlotPositions.Count; i++)
                {
                    SlotHolder = SlotPositions[0] - SlotOffsets[i];
                    //Rotate slot around center to match current rotation
                    SlotPositions[i] = RotatePointAroundPivot(SlotHolder, LeadUnit.transform.position, LeadUnit.transform.eulerAngles);
                }
            }

        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    private void MoveUnitsToSlots()
    {
        for(int i = 1; i < SlotPositions.Count; i++)
        {
            //Get variables required for specific ship and slot
            GameObject Ship = FightersInFormation[i];
            Rigidbody ShipBody = Ship.GetComponentInChildren<Rigidbody>();
            if (ShipBody != null)
            {
                Vector3 TargetSlot = SlotPositions[i];
                float ShipMoveSpeed = MoveSpeed;
                Quaternion q = new Quaternion();

                //Turn to face slot position
                q = Quaternion.LookRotation(TargetSlot + (LeadUnit.transform.forward * 5) - Ship.transform.position, LeadUnit.transform.up);

                Ship.transform.rotation = Quaternion.RotateTowards(Ship.transform.rotation, q, RotationSpeed * Time.deltaTime);


                //Use Calculate force required to reach point with max move speed
                //Allows for units on outside of curve to catch up to formation and prevents units on inside from overshooting point
                float NeededSpeed = 0;
                Vector3 VectToPoint = TargetSlot - Ship.transform.position;

                if (Vector3.Distance(TargetSlot, Ship.transform.position) < MoveSpeed / 100)
                {
                    NeededSpeed = (MoveSpeed - 1) / Vector3.Distance(TargetSlot, Ship.transform.position);
                }
                else
                {
                    NeededSpeed = MoveSpeed / Vector3.Distance(TargetSlot, Ship.transform.position);
                }

                if(NeededSpeed < MoveSpeed)
                {
                    ShipBody.AddForce(VectToPoint * NeededSpeed, ForceMode.Impulse);
                }
            }
        }
    }

    private IEnumerator SpotPlayerDelay()
    {
        yield return new WaitForSeconds(10f);
        CanSpotPlayer = true;
    }
}
