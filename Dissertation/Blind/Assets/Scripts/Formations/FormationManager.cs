using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    public GameObject EmptyFormation;
    public float MaxFormationNumber;
    public int MinUnitCount;
    public int MaxUnitCount;

    public List<GameObject> SpawnedFormations;

    // Start is called before the first frame update
    void Start()
    {
        SpawnedFormations = new List<GameObject>();

        //Spawn Required number of formations and store in child gameobjects
        for(int i = 0; i < MaxFormationNumber; i++)
        {
            AddFormation();
        }
    }

    //Calls on Fixed Update due to Update Formation using RigidBody Physics
    void FixedUpdate()
    {
        //Check for missing formations
        if (SpawnedFormations.Count < MaxFormationNumber)
        {
            AddFormation();
        }

        //Call update for each formation
        foreach(GameObject Formation in SpawnedFormations)
        {
            Formation.GetComponent<FormationBase>().UpdateFormation();
        }
    }

    void AddFormation()
    {
        GameObject NewFormationObj = Instantiate(EmptyFormation, this.transform);
        NewFormationObj.GetComponent<FormationBase>().UnitTotalCount = Random.Range(MinUnitCount, MaxUnitCount);
        //Add to list of spawned formations
        SpawnedFormations.Add(NewFormationObj);
    }
}
