using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will trigger a checkpoint based on a trigger box
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
///

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(GiveCheckPoint))]
public class CheckPointTrigger : MonoBehaviour
{
    private GiveCheckPoint chkpnt;

    [SerializeField]private CheckpointManager checkpointMgr;
    // Start is called before the first frame update
    void Start()
    {
        chkpnt = GetComponent<GiveCheckPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkpointMgr.GeneratorOn)
        {
            if (other.CompareTag("Player"))
                chkpnt.CheckPoint();
        }
    }
}
