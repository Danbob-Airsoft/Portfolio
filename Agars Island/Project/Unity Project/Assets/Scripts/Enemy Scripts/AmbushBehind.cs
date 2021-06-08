using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

/// <summary>
///
/// Locates an ambush point behind the player to move the AI to during a sneak attack
///
/// Created by: Daniel Bailey
/// </summary>

public class AmbushBehind : BT_Behaviour
{
    //Required variables
    private Transform Self;
    private localTree localBB;
    private NavMeshAgent agent;
    private GameObject Player;

    //Ambush positions
    private GameObject[] AmbushPositions;

    public AmbushBehind(Transform _self)
    {
        //Set all required variables
        Self = _self;
        localBB = Self.GetComponent<localTree>();
        agent = Self.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        //Get all marked Ambush positions
        AmbushPositions = GameObject.FindGameObjectsWithTag("AmbushPos");
    }

    public override NodeState tick()
    {
        Debug.Log("Finding Point Behind Player");
        //Check for nearest ambush point behind player
        GameObject ClosestPoint = null;
        float ClosestDistance = int.MaxValue;
        foreach (GameObject Point in AmbushPositions)
        {
            //Check if angle to point is equal to behind player and point is on same level
            if (CalculateBehind(Point) && CheckForSameLevel(Point))
            {
                //Check if point is closest to player
                if (Vector3.Distance(Player.transform.position, Point.transform.position) < ClosestDistance)
                {
                    ClosestPoint = Point;
                    ClosestDistance = Vector3.Distance(Player.transform.position, Point.transform.position);
                }
            }
        }

        //If Player is somehow facing all ambush points
        if(ClosestDistance == int.MaxValue)
        {
            ClosestDistance = int.MaxValue;
            //Rescan list and just find closest point
            foreach (GameObject Point in AmbushPositions)
            {
                //Check if point is closest to player
                if (Vector3.Distance(Player.transform.position, Point.transform.position) < ClosestDistance && CheckForSameLevel(Point))
                {
                    ClosestPoint = Point;
                    ClosestDistance = Vector3.Distance(Player.transform.position, Point.transform.position);
                }
            }
        }
        //Debug.Log(ClosestPoint);
        //Enable force charge
        localBB.ForceCharge = true;
        //Enable renderer and Collider
        //Self.GetComponent<MeshRenderer>().enabled = true;
        Self.GetComponent<CapsuleCollider>().enabled = true;
        //Move to found point
        Self.transform.position = ClosestPoint.transform.position;
        //Disable player obstacle cone
        Player.transform.GetChild(1).gameObject.SetActive(false);
        //Enable Movement
        agent.enabled = true;
        //Return Node Succeeded
        return NodeState.NODE_SUCCESS;
    }

    private bool CheckForSameLevel(GameObject Point)
    {
        AmbPointData PointLevel = Point.GetComponent<AmbPointData>();
        //If point is on same level and not point already at
        if ((Self.transform.position.y < 0 && PointLevel.Floor == "Lower") || (Self.transform.position.y > 0 && PointLevel.Floor == "Upper"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CalculateBehind(GameObject Point)
    {
        Vector3 MyPosition = Camera.main.WorldToViewportPoint(Point.transform.position);
        if(MyPosition.z < 0)
        {
            Debug.Log("Point: " + Point + " is behind");
            return true;
        }
        return false;
    }
}
