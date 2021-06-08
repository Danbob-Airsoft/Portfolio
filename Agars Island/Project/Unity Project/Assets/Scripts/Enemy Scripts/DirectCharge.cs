using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

/// <summary>
///
/// Sets the AI to charge the player when sight is next established, regardless of sanity
///
/// Created by: Daniel Bailey
/// </summary>

public class DirectCharge : BT_Behaviour
{
    //Required Variables
    private Transform Self;
    private localTree localBB;

    private GameObject Player;

    public DirectCharge(Transform _self)
    {
        //Set required variables
        Self = _self;
        localBB = Self.GetComponent<localTree>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override NodeState tick()
    {
        //Disable Player Obstacle Cone
        Player.transform.GetChild(1).gameObject.SetActive(false);

        //Set move to location
        localBB.setMoveToLocation(localBB.StoredPosition);
        localBB.FixedMoveLocation = true;

        return NodeState.NODE_SUCCESS;
    }
}
