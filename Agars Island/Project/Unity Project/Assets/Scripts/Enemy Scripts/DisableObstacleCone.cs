using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

/// <summary>
///
/// Disables the cone around the player, allowing the AI to get close and attack
///
/// Created by: Daniel Bailey
/// </summary>

public class DisableObstacleCone : BT_Behaviour
{
    //Required variables
    private Transform Self;
    private GameObject Player;

    public DisableObstacleCone(Transform _Self)
    {
        //Set required variables
        Self = _Self;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override NodeState tick()
    {
        //Disable Player cone
        Player.transform.GetChild(1).gameObject.SetActive(false);
        return NodeState.NODE_SUCCESS;
    }
}
