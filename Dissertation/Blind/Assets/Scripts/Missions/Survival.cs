using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MissionBase
{
    private GameObject Player;

    public bool InfiniteSurvival;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //If not in Infinite Survival mode (Only true for training level)
        if (!InfiniteSurvival)
        {
            //Check if player has survived long enough
            if (Time.timeSinceLevelLoad >= TargetSurvivalTime)
            {
                //Display victory screen
                Player.GetComponent<PlayerHealth>().TriggerVictory(MissionCompTopString, MissionCompBottomString);
            }
        }
    }

    override public string GetTopFailText()
    {
        if (InfiniteSurvival)
        {
            float SurvivedTime = Mathf.Round(Time.timeSinceLevelLoad);
            MissionFailedTopString = ("The battle lasted: " + SurvivedTime.ToString() + " Seconds");
        }
        return MissionFailedTopString;
    }

    override public string GetBottomFailText()
    {
        if (InfiniteSurvival)
        {
            int PlayerScore = Player.gameObject.GetComponent<PlayerScore>().Score;
            MissionFailedBottomString = ("You Achieved a score of: " + PlayerScore.ToString() + "! Well done pilot!");
        }
        return MissionFailedBottomString;
    }
}
