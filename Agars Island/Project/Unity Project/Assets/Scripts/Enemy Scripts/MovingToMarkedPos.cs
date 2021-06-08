using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

/// <summary>
///
/// Checks if the AI is marked as moving to a marked location
///
/// Created by: Daniel Bailey
/// </summary>

public class MovingToMarkedPos : BT_Behaviour
{
    //Required variables
    private Transform Self;
    private localTree localBB;

    public MovingToMarkedPos(Transform _self)
    {
        //Set Required variables
        Self = _self;
        localBB = Self.GetComponent<localTree>();

    }

    public override NodeState tick()
    {
        //If agent is moving to a preset location (players last known position from alternate route or sight lost)
        if (localBB.FixedMoveLocation)
        {
            Debug.Log("Moving to marked position");
            //Return success to continue movement to that point
            return NodeState.NODE_SUCCESS;
        }

        //Else return fail to allow for random movement
        return NodeState.NODE_FAILURE;
    }
}
