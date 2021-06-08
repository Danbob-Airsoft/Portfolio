using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

/// <summary>
///
/// Sets the AI as moving towards a marked ambush point
///
/// Created by: Daniel Bailey
/// </summary>

public class MoveToAmbush : BT_Behaviour
{
    //Tree variables
    private Transform Self;
    private localTree localBB;
    private NavMeshAgent agent;
    private GameObject Player;

    //Bools for checking if frame is same the destination has been set (Prevents path from appearing complete before updating)
    bool DestSetFrame;

    //Ambush Positions
    private GameObject[] AmbushPositions;

    //Ambush Movement Variables
    private float NextAmbRouteCheck;
    private float AmbushRouteDelay;

    public MoveToAmbush(Transform _self, float InRouteDelay)
    {
        //Set required variables
        Self = _self;
        localBB = Self.GetComponent<localTree>();
        agent = Self.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");

        //Set time to check next route
        NextAmbRouteCheck = Time.time;
        //Get all ambush positions
        AmbushPositions = GameObject.FindGameObjectsWithTag("AmbushPos");
        //Set delay between updating routes
        AmbushRouteDelay = InRouteDelay;

        DestSetFrame = false;
        //Disable ambush attack cone
        Self.transform.GetChild(0).gameObject.SetActive(false);

    }

    public override NodeState tick()
    {
        //Check if agent is unable to ambush
        if (!localBB.CanAmbush)
        {
            //Fail node and force alt route
            return NodeState.NODE_FAILURE;
        }

        //If small delay has past, recalculate routes to ensure still moving to closest
        if (Time.time >= NextAmbRouteCheck)
        {
            DestSetFrame = false;
            GameObject ClosestPoint = null;
            float ClosestDistance = float.MaxValue;
            NextAmbRouteCheck = Time.time + AmbushRouteDelay;
            //Enable Obstacle on player to prevent selecting ambush point in sight
            Player.transform.GetChild(1).gameObject.SetActive(true);

            //Scan each ambush point for closest
            foreach (GameObject Point in AmbushPositions)
            {
                float PathDistance = CalculatePathDistance(Point);
                float PointY = Point.transform.position.y;

                //Check if point is closer
                if (PathDistance < ClosestDistance && CheckForSameLevel(Point))
                {
                    ClosestDistance = PathDistance;
                    ClosestPoint = Point;
                    //Set Destination
                    localBB.setMoveToLocation(ClosestPoint.transform.position);
                    agent.SetDestination(ClosestPoint.transform.position);
                    DestSetFrame = true;
                }
            }
        }

        float dist = agent.remainingDistance;
        //Check if arrived at ambush position
        if (dist <= 0 && !DestSetFrame)
        {            
            //Disable Cone
            Player.transform.GetChild(1).gameObject.SetActive(false);
            //Enable attack cone
            Self.transform.GetChild(0).gameObject.SetActive(true);
            //Disable movement
            agent.enabled = false;
            //Disable mesh renderer and collider
            //Self.GetComponent<MeshRenderer>().enabled = false;
            Self.GetComponent<CapsuleCollider>().enabled = false;
            //Return node succeeded
            return NodeState.NODE_SUCCESS;
        }
        //If Route updated this frame, toggle to no longer
        if (DestSetFrame)
        {
            DestSetFrame = false;
        }
        Debug.Log("Moving to ambush point");
        //Mark node as running
        return NodeState.NODE_RUNNING;
    }

    private float CalculatePathDistance(GameObject Point)
    {
        NavMeshPath Path = new NavMeshPath();
        //Calculate path to point
        if (NavMesh.CalculatePath(Self.transform.position, Point.transform.position, NavMesh.AllAreas, Path))
        {
            float PathLength = 0;
            //If Path is valid
            if (Path.status == NavMeshPathStatus.PathComplete)
            {
                for (int i = 1; i < Path.corners.Length; ++i)
                {
                    PathLength += Vector3.Distance(Path.corners[i - 1], Path.corners[i]);

                }
                return PathLength;
            }
        }
        //If Calculation Fails or Path is incomplete
        return float.MaxValue;

    }

    private bool CheckForSameLevel(GameObject Point)
    {
        AmbPointData PointLevel = Point.GetComponent<AmbPointData>();
        //If point is on same level
        if ((Self.transform.position.y < 0 && PointLevel.Floor == "Lower") || (Self.transform.position.y > 0 && PointLevel.Floor == "Upper"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
