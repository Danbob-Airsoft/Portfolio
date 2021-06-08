using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

/// <summary>
///
/// Stores the last location the AI spotted the player at
///
/// Created by: Daniel Bailey
/// </summary>

public class StoreLastPosition : BT_Behaviour
{
    private Transform Self;
    private localTree localBB;
    private NavMeshAgent agent;
    private GameObject Player;

    public StoreLastPosition(Transform _self)
    {
        //Set required variables
        Self = _self;
        localBB = Self.GetComponent<localTree>();
        agent = Self.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override NodeState tick()
    {
        //Store last location Player was seen at
        localBB.StoredPosition = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        return NodeState.NODE_SUCCESS;
    }

}
