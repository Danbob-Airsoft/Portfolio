using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveProtection : MissionBase
{
    public GameObject ObjectiveShip;
    private CapitalHealth ObjectiveHP;

    private GameObject Player;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ObjectiveHP = ObjectiveShip.GetComponent<CapitalHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        //If Objective ship is dead
        if(ObjectiveHP.Hull <= 0)
        {
            //Mission Failed
            Player.GetComponent<PlayerHealth>().TriggerGameOver(MissionFailedTopString, MissionFailedBottomString);
        }

        //If Objective ship survives for time or makes to destination
        else if (Time.timeSinceLevelLoad >= TargetSurvivalTime)
        {
            //Mission Complete
            Player.GetComponent<PlayerHealth>().TriggerVictory(MissionCompTopString, MissionCompBottomString);
        }
    }
}
