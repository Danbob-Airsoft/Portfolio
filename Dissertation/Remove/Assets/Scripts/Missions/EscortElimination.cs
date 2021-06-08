using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortElimination : MissionBase
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
        //Mission Complete
        if (Player.GetComponent<PlayerScore>().Score == TargetKillScore)
        {
            Player.GetComponent<PlayerHealth>().TriggerVictory(MissionCompTopString, MissionCompBottomString);
        }

        //Mission failed
        else if(TargetSurvivalTime >= Time.timeSinceLevelLoad)
        {
            Player.GetComponent<PlayerHealth>().TriggerGameOver(MissionFailedTopString, MissionFailedBottomString);
        }

    }
}
