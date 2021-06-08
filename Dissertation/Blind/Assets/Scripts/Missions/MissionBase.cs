using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBase : MonoBehaviour
{
    public string MissionFailedTopString;
    public string MissionFailedBottomString;

    public string MissionCompTopString;
    public string MissionCompBottomString;

    public float TargetSurvivalTime;

    virtual public string GetTopFailText()
    {
        return MissionFailedTopString;
    }

    virtual public string GetBottomFailText()
    {
        return MissionFailedBottomString;
    }
}
