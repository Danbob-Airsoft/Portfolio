using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCapture : MissionBase
{
    private GameObject Player;
    public int TargetKillScore;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //If fighter escort is eliminated and Objective's engines are destroyed
        if(Player.GetComponent<PlayerScore>().Score == TargetKillScore)
        {
            Player.GetComponent<PlayerHealth>().TriggerVictory(MissionCompTopString, MissionCompBottomString);
        }

        //Mission failed
        else if(Time.timeSinceLevelLoad > TargetSurvivalTime)
        {
            Player.GetComponent<PlayerHealth>().TriggerGameOver(MissionFailedTopString, MissionFailedBottomString);
        }
    }
}
