using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDestruction : MissionBase
{
    public GameObject ObjectiveShip;
    private CapitalHealth ObjectiveHP;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ObjectiveHP = ObjectiveShip.GetComponent<CapitalHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectiveHP.Hull <= 0)
        {
            //Mission Complete
            Player.GetComponent<PlayerHealth>().TriggerVictory(MissionCompTopString, MissionCompBottomString);
        }

        //Mission Failure
        else if (Time.timeSinceLevelLoad > TargetSurvivalTime)
        {
            Player.GetComponent<PlayerHealth>().TriggerGameOver(MissionFailedTopString, MissionFailedBottomString);
        }
    }
}
