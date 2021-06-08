using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ThaalianSpawner : MonoBehaviour
{
    public GameObject[] SpawnPoints;
    public List<GameObject> SpawnedFighters;
    public List<GameObject> AllFighters;
    public int FighterCount;

    public GameObject FigherPrefab;

    private GameObject ChosenGate;

    private float NextSpawnTime;

    public AudioSource SpawnerSFX;

    public int TotalFighterCount;

    // Start is called before the first frame update
    void Start()
    {
        //Quaternion RandomRotation = Random.rotation;
        SpawnPoints = GameObject.FindGameObjectsWithTag("ThaalianJumpgate");

        for(int i = 0; i < TotalFighterCount; i++)
        {
            GameObject NewShip = Instantiate(FigherPrefab, new Vector3(2000, 2000, 2000), new Quaternion(0, 0, 0, 1));
            NewShip.SetActive(false);
            AllFighters.Add(NewShip);
        }

        FighterCount = 0;
        //Find all SpawnPoints
        AddNewEnemies(10);
        NextSpawnTime = Time.time + Random.Range(20, 30);
    }

    // Update is called once per frame
    private void Update()
    {
        //Check for Inactive ships
        foreach(GameObject Ship in SpawnedFighters)
        {
            if (Ship.activeSelf == false)
            {
                //Select Random Respawn Gate
                ChosenGate = ChooseNewGate();
                //Reset Ship
                Ship.transform.GetComponentInChildren<FighterHealth>().ShipHealth = Ship.transform.GetComponentInChildren<FighterHealth>().MaxHealth;
                Ship.transform.GetComponentInChildren<FighterMovement>().MoveSpeed = 5;
                Ship.transform.GetComponentInChildren<FighterMovement>().RotationSpeed = 100;
                MeshRenderer[] Meshes = Ship.transform.root.GetChild(1).GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer Mesh in Meshes)
                {
                    Mesh.enabled = true;
                }
                //Enable Gate
                //Get Random Position in gate bounds
                MeshRenderer GateCollider = ChosenGate.transform.GetChild(1).GetComponent<MeshRenderer>();
                GateCollider.enabled = true;
                float RandomX = Random.Range(GateCollider.bounds.min.x + 25, GateCollider.bounds.max.x - 25);
                float RandomY = Random.Range(GateCollider.bounds.min.y + 25, GateCollider.bounds.max.y - 25);
                //Move Ship to Gate
                Ship.gameObject.transform.position = new Vector3(RandomX, RandomY, ChosenGate.transform.GetChild(3).transform.position.z);
                //Set Ship to Active
                Ship.SetActive(true);
            }
        }

        //Check if new enemies should be added
        if(Time.time >= NextSpawnTime && SpawnedFighters.Count < 40)
        {
            //Increase Next spawn time
            NextSpawnTime = Time.time + Random.Range(20, 25);

            //Add new ships
            AddNewEnemies(10);
        }
    }

    public void AddNewEnemies(int NumberToAdd)
    {
        int Count = 0;
        for(Count = FighterCount; Count < FighterCount + NumberToAdd; Count++)
        {
            //Choose a Random Gate
            ChosenGate = ChooseNewGate();

            //Get Random Position in gate bounds
            MeshRenderer GateCollider = ChosenGate.transform.GetChild(1).GetComponent<MeshRenderer>();
            float RandomX = Random.Range(GateCollider.bounds.min.x + 25, GateCollider.bounds.max.x - 25);
            float RandomY = Random.Range(GateCollider.bounds.min.y + 25, GateCollider.bounds.max.y - 25);

            Vector3 NewPos = new Vector3(RandomX, RandomY, ChosenGate.transform.GetChild(3).transform.position.z);

            //Find closest position on Mesh Renderer
            //NewPos = Physics.ClosestPoint(NewPos, GateCollider, GateCollider.transform.position, GateCollider.transform.rotation);

            //Spawn Ship on Gate
            GameObject NewShip = AllFighters[Count];
            NewShip.SetActive(true);
            NewShip.transform.position = NewPos;
            NewShip.transform.rotation = ChosenGate.transform.rotation;

            //Add Ship to active ships
            SpawnedFighters.Add(NewShip);

        }
        FighterCount += NumberToAdd;
    }

    private GameObject ChooseNewGate()
    {
        int RandomIndex = Random.Range(0, SpawnPoints.Length -1);
        GameObject SelectedGate = SpawnPoints[RandomIndex];
        MeshRenderer GateCollider = SelectedGate.transform.GetChild(1).GetComponent<MeshRenderer>();
        SpawnerSFX = SelectedGate.GetComponent<AudioSource>();
        SpawnerSFX.Play();
        StartCoroutine(EffectDisable(GateCollider));
        return SelectedGate;
    }

    private IEnumerator EffectDisable(MeshRenderer Chosengate)
    {
        Chosengate.enabled = true;

        yield return new WaitForSeconds(5);

        Chosengate.enabled = false;
    }
}
