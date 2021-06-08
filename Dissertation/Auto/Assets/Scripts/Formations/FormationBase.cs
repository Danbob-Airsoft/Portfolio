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
    private List<FighterStruct> FighterStructList;
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
    public bool LeadCurrentlyAvoiding;
    private float ShipsAvoiding;

    [Header("Sight Variables")]
    public float SightRange;
    private GameObject PlayerOBJ;
    private bool CanSpotPlayer;

    [Header("Used in some implementations")]
    public GameObject LeadUnit;
    public Rigidbody LeadBody;

    public class FighterStruct
    {
        public GameObject FighterOBJ;
        public bool Avoiding;
        public bool ReturningToSlot;
        public Quaternion AvoidRotation;
    }

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

        FighterStructList = new List<FighterStruct>();

        //Call Formation Generation
        GenerateFormation();
        this.transform.position += this.transform.forward * 2;

        RandomTarget = this.transform.position + (this.transform.forward * 400);
        ThisHUDMarker = this.GetComponent<FriendlyMarker>();
    }

    //----------------------------------------------- Formation Generation Functions ----------------------

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
        LeadUnit.GetComponentInChildren<FighterHealth>().FormationIndex = 0;
        //Add Fighter to list
        FightersInFormation.Add(LeadUnit);

        //Create Lead fighter struct
        FighterStruct LeadStruct = new FighterStruct();
        LeadStruct.FighterOBJ = LeadUnit;
        LeadStruct.Avoiding = false;

        FighterStructList.Add(LeadStruct);

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
            SpawnedFighter.GetComponentInChildren<FighterHealth>().FormationIndex = i + 1;
            //Add Fighter to list
            FightersInFormation.Add(SpawnedFighter);

            FighterStruct CurrentStruct = new FighterStruct();
            CurrentStruct.FighterOBJ = SpawnedFighter;
            CurrentStruct.Avoiding = false;
            FighterStructList.Add(CurrentStruct);
        }
        //Set Unit Count
        ActiveUnitCount = UnitTotalCount;

    }

    public void RemoveFighter(GameObject ToRemove, int InIndex)
    {
        Debug.Log("Removing at: " + InIndex);
        //Remove Fighter from list
        //All units in slots above removed ship will then move down a slot
        FightersInFormation.Remove(ToRemove);
        //Check if lead unit needs replacing
        if (ToRemove == LeadUnit)
        {
            LeadCurrentlyAvoiding = false;
            //Lead unit is always index 0, so set new lead to index 1
            LeadUnit = FightersInFormation[0];
            LeadBody = LeadUnit.GetComponent<Rigidbody>();
        }
        //Remove last slot in formation as it is now unoccupied
        SlotPositions.RemoveAt(SlotPositions.Count - 1);
        SlotOffsets.RemoveAt(SlotOffsets.Count - 1);
        FighterStructList.RemoveAt(InIndex);
        //Decrease number of active fighters
        ActiveUnitCount -= 1;
        //Check if there is only 1 unit remaining
        if (ActiveUnitCount == 1)
        {
            Debug.Log("FormationDestroyed");
            MoveFighterToOtherForm();
            //Destroy formation
            Manager.SpawnedFormations.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
        //Otherwise update all indexes for ships in formation
        else
        {
            for(int i = 0; i < ActiveUnitCount; i++)
            {
                GameObject Fighter = FightersInFormation[i];
                Fighter.GetComponentInChildren<FighterHealth>().FormationIndex = i;
                Fighter.GetComponentInChildren<FriendlyMarker>().MarkerActive = true;
            }
        }
    }

    public void EnterCombat()
    {
        InCombat = true;
        ReformTime = Time.time + ReformDelay;
        ThisHUDMarker.MarkerActive = false;
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

        //Create New Fighter struct
        FighterStruct CurrentStruct = new FighterStruct();
        CurrentStruct.FighterOBJ = ToAdd;
        CurrentStruct.Avoiding = false;
        FighterStructList.Add(CurrentStruct);

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

    public void UpdateFormation()
    {
        //Check if formation is currently in combat
        if (InCombat)
        {
            //Check if enough time has passed to start reforming
            if(Time.time > ReformTime)
            {
                //Set to no longer in combat so formation will activate next update
                InCombat = false;
                ThisHUDMarker.MarkerActive = true;
                
                for(int i = 0; i < FightersInFormation.Count; i++)
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
        //Loop through each fighter to check for obstacles
        for(int i = 0; i < ActiveUnitCount; i++)
        {
            GameObject ObstacleOBJ;
            Vector3 HitPoint;
            FighterStruct Fighter = FighterStructList[i];

            //Obstacle Detection
            if (CheckForObstacle(Fighter, SlotOffsets[i].z, out ObstacleOBJ, out HitPoint))
            {
                //Identify obstacle and act accordingly
                IdentifyObstacle(Fighter, ObstacleOBJ, HitPoint);
            }

            //Else check if currently making turn to avoid obstacle
            else if (Fighter.Avoiding)
            {
                ContinueAvoiding(Fighter);
            }

            //Else check if fighter needs to turn back to slot after avoiding
            else if (Fighter.ReturningToSlot)
            {
                ReturnToForm(Fighter, SlotPositions[i]);
            }
        }

        //Check for line of sight on player and lead unit not currently avoiding
        if (CheckForSight() && LeadCurrentlyAvoiding == false)
        {
            TurnToPlayer();
        }

        //Random Movement
        else if(LeadCurrentlyAvoiding == false)
        {
            RandomMovement();
        }

        LeadBody.AddForce(this.transform.forward * (MoveSpeed - 1), ForceMode.Impulse);

        //Update Slot positions
        UpdateSlotPositions();

        //Update Unit positions
        MoveUnitsToSlots();
    }

    private bool CheckForObstacle(FighterStruct CurrentShip, float SlotBackOffset, out GameObject HitObstacle, out Vector3 HitPoint)
    {
        RaycastHit HitOut = new RaycastHit();

        float RelevantCollisionRange = CollisionRange /*+ Mathf.Abs(SlotBackOffset)*/;

        //Get point in front of ship
        Vector3 ShipFront = CurrentShip.FighterOBJ.transform.position + CurrentShip.FighterOBJ.transform.forward * 5;
        //Check for obstacle within set range
        if (Physics.Raycast(ShipFront, CurrentShip.FighterOBJ.transform.forward, out HitOut, RelevantCollisionRange))
        {
            //Check if not already avoiding or if obstacle is too close anyway and not another enemy fighter
            if ((CurrentShip.Avoiding == false || HitOut.distance <= 75) && !HitOut.collider.transform.root.CompareTag("EnemyFighter"))
            {
                //Set HitObstacle Output to object hit
                HitObstacle = HitOut.collider.gameObject;
                HitPoint = HitOut.point;
                //Return True when obstacle
                return true;
            }
        }

        //If no fighter returns a hit
        HitObstacle = null;
        HitPoint = Vector3.zero;
        return false;
    }

    private void IdentifyObstacle(FighterStruct CurrentShip, GameObject ObstacleOBJ, Vector3 HitPoint)
    {
        //Get Obstacle Tag
        string ObstacleTag = ObstacleOBJ.tag;
        float DistanceToObstacle = Vector3.Distance(this.transform.position, ObstacleOBJ.transform.position);

        //Act accordingly based on tag
        switch (ObstacleTag)
        {
            //If hit object is player
            case "Player":
                //Check if Distance is too close
                if (DistanceToObstacle <= PlayerAvoidRange)
                {
                    CanSpotPlayer = false;
                    //Set Avoid Rotation
                    AvoidRotation = Quaternion.LookRotation((CurrentShip.FighterOBJ.transform.root.position - HitPoint) * 0.5f);
                    //Set currently avoiding
                    CurrentShip.Avoiding = true;
                    //Check if obstacle is blocking lead unit
                    if (CurrentShip.FighterOBJ == LeadUnit)
                    {
                        //Set lead to avoiding
                        LeadCurrentlyAvoiding = true;
                        //Start rotation
                        LeadUnit.transform.rotation = Quaternion.RotateTowards(LeadUnit.transform.rotation, AvoidRotation, RotationSpeed * Time.deltaTime);
                    }
                    else
                    {
                        //Set Ship to avoid
                        CurrentShip.Avoiding = true;
                        //Set Ships avoid rotation
                        CurrentShip.AvoidRotation = Quaternion.RotateTowards(CurrentShip.FighterOBJ.transform.rotation, AvoidRotation, RotationSpeed * Time.deltaTime);
                    }

                    StartCoroutine(SpotPlayerDelay());
                }
                //Else check if in range to engage
                else if (DistanceToObstacle <= PlayerEngageRange)
                {
                    foreach (FighterStruct Ship in FighterStructList)
                    {
                        //If ship is not avoiding
                        if(Ship.Avoiding == false && Ship.ReturningToSlot == false)
                        {
                            //Call firing
                            Ship.FighterOBJ.GetComponentInChildren<FighterFiring>().FireCannons(HitPoint);
                        }
                    }
                }
                //Otherwise formation will continue to move towards Player
                break;

            //If obstacle is other
            default:
                //Calculate Avoidance Turn
                AvoidRotation = Quaternion.LookRotation((CurrentShip.FighterOBJ.transform.root.position - HitPoint) * 0.5f);
                //Set Ship to avoid
                CurrentShip.Avoiding = true;
                //Set Ships avoid rotation
                CurrentShip.AvoidRotation = AvoidRotation;
                GameObject FighterOBJ = CurrentShip.FighterOBJ;
                //Check if ship is lead unit
                if (CheckIfLead(CurrentShip))
                {
                    //Set lead to avoiding
                    LeadCurrentlyAvoiding = true;
                }

                FighterOBJ.transform.rotation = Quaternion.RotateTowards(FighterOBJ.transform.rotation, AvoidRotation, RotationSpeed * Time.deltaTime);
                
                break;
        }
    }

    private void ContinueAvoiding(FighterStruct Fighter)
    {
        GameObject FighterOBJ = Fighter.FighterOBJ;
        //Check if arrived at rotation
        if (FighterOBJ.transform.rotation == Fighter.AvoidRotation)
        {
            //If Lead Unit avoiding
            if(CheckIfLead(Fighter))
            {
                //If yes, set currently avoiding to false
                LeadCurrentlyAvoiding = false;
                //Update Change Time
                NextChangeTime = Time.time + Random.Range(MinDirChange, MaxDirChange);
                //Update Random Position
                RandomPosition();
            }
            //Otherwise set to returning to slot position
            else
            {
                Fighter.ReturningToSlot = true;
            }
            Fighter.Avoiding = false;
        }
        else
        {
            //Continue to rotate towards
            FighterOBJ.transform.rotation = Quaternion.RotateTowards(FighterOBJ.transform.rotation, Fighter.AvoidRotation, RotationSpeed * Time.deltaTime);
        }
    }

    public void ReturnToForm(FighterStruct Fighter, Vector3 ShipSlot)
    {
        GameObject FighterOBJ = Fighter.FighterOBJ;
        Quaternion ToSlot = Quaternion.LookRotation(ShipSlot + (LeadUnit.transform.forward * 5) - FighterOBJ.transform.position, LeadUnit.transform.up);
        //Check if arrived at rotation
        if (FighterOBJ.transform.rotation == ToSlot)
        {
            Fighter.ReturningToSlot = false;
        }
        else
        {
            //Continue to rotate towards
            FighterOBJ.transform.rotation = Quaternion.RotateTowards(FighterOBJ.transform.rotation, ToSlot, RotationSpeed * Time.deltaTime);
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
        for(int i = 1; i < ActiveUnitCount; i++)
        {
            //Get variables required for specific ship and slot
            FighterStruct CurrentShipStruct = FighterStructList[i];
            GameObject Ship = FighterStructList[i].FighterOBJ;
            Rigidbody ShipBody = Ship.GetComponentInChildren<Rigidbody>();
            Vector3 TargetSlot = SlotPositions[i];
            float ShipMoveSpeed = MoveSpeed;
            Quaternion q = new Quaternion();
            Vector3 VectToPoint = new Vector3();
            float NeededSpeed = 0;

            //If ship is not avoiding its own obstacle
            if (CurrentShipStruct.Avoiding == false && CurrentShipStruct.ReturningToSlot == false)
            {
                //Turn to face slot position
                q = Quaternion.LookRotation(TargetSlot + (LeadUnit.transform.forward * 5) - Ship.transform.position, LeadUnit.transform.up);

                Ship.transform.rotation = Quaternion.RotateTowards(Ship.transform.rotation, q, RotationSpeed * Time.deltaTime);

                //Use Calculate force required to reach point with max move speed
                //Allows for units on outside of curve to catch up to formation and prevents units on inside from overshooting point
                VectToPoint = TargetSlot - Ship.transform.position;

                if (VectToPoint.magnitude < MoveSpeed / 100)
                {
                    NeededSpeed = (MoveSpeed - 1) / VectToPoint.magnitude;
                }
                else
                {
                    NeededSpeed = MoveSpeed / VectToPoint.magnitude;
                }

                if (NeededSpeed < MoveSpeed)
                {
                    ShipBody.AddForce(VectToPoint * NeededSpeed, ForceMode.Impulse);
                }
            }
            else
            {
                //Set to use same speed as Lead
                NeededSpeed = MoveSpeed - 1;
                //Apply max speed force to ship forward vector
                ShipBody.AddForce(Ship.transform.forward * NeededSpeed, ForceMode.Impulse);
            }
        }
    }

    private bool CheckIfLead(FighterStruct FighterIn)
    {
        if(FighterIn.FighterOBJ == LeadUnit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator SpotPlayerDelay()
    {
        yield return new WaitForSeconds(10f);
        CanSpotPlayer = true;
    }
}
