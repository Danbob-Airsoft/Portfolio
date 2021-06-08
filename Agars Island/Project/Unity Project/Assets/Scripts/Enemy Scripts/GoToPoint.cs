using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

/// <summary>
///
/// Sets the AI destination to the target location
///
/// Created by: Daniel Bailey
/// </summary>

public class GoToPoint : BT_Behaviour
{
    private Transform Self;
    private localTree LocalBB;
    private NavMeshAgent Agent;
    private GameObject Player;

    public GoToPoint(Transform _self)
    {
        //Set required variables
        Self = _self;
        LocalBB = Self.GetComponent<localTree>();

        Agent = Self.GetComponent<NavMeshAgent>();

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override NodeState tick()
    {
        Agent.destination = LocalBB.getMoveToLocation();

        //Get Agent path
        var path = Agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.blue);
        }
        //Disable player obstacle
        Player.transform.GetChild(1).gameObject.SetActive(false);

        //Return Node succeeded
        nodeState = NodeState.NODE_SUCCESS;
        return NodeState.NODE_SUCCESS;

        //nodeState = NodeState.NODE_RUNNING;
        //return NodeState.NODE_RUNNING;
    }
}
