using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

/// <summary>
///
/// Sets the AI as moving to a specific location if line of sight is not made
///
/// Created by: Daniel Bailey
/// </summary>

public class SetToMarkedPos : BT_Behaviour
{
    private Transform Self;
    private localTree localBB;
    private NavMeshAgent agent;
    private GameObject Player;

    public SetToMarkedPos(Transform _self)
    {
        Self = _self;
        localBB = Self.GetComponent<localTree>();
        agent = Self.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override NodeState tick()
    {
        //Set target location to stored location and mark as moving to
        localBB.setMoveToLocation(localBB.StoredPosition);
        localBB.FixedMoveLocation = true;

        return NodeState.NODE_SUCCESS;
    }
}
