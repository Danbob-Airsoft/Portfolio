using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Checkpoint for the first keycard
/// 
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class FirstKeycardCheckpoint : GiveCheckPoint
{
    [SerializeField] private CheckpointManager mgr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckPoint()
    {
        Debug.Log("IM A CHILD :D");
        if (!mgr.doorLockedTrigger) return;
        base.CheckPoint();
        Debug.Log("checkpoint given");
    }
}
