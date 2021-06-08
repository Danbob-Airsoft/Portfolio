using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

/// <summary>
///
/// Sets the target location to a random, reachable point around the player
///
/// Created by: Daniel Bailey
/// </summary>

public class RandomMovement : BT_Behaviour
{
    private Transform Self;
    private localTree localBB;
    private NavMeshAgent agent;

    private float WanderDistance;
    private float MinDirTime;
    private float MaxDirTime;
    private float NextDirTime;

    private GameObject Player;

    public RandomMovement(Transform _self, float InMinDirTime, float InMaxDirTime, float InWanderRange)
    {
        //Set required variables
        Self = _self;
        localBB = Self.GetComponent<localTree>();
        agent = Self.GetComponent<NavMeshAgent>();

        //Set time to choose next point
        NextDirTime = Time.time;

        //Set wander distance and time range between choosing directions
        WanderDistance = InWanderRange;
        MinDirTime = InMinDirTime;
        MaxDirTime = InMaxDirTime;

        //Set Player
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override NodeState tick()
    {
        Debug.Log("Moving Randomly");
        //Bool for suitable point
        bool SuitablePoint = false;
        //If moving to fixed location, toggle to not
        if (localBB.FixedMoveLocation)
        {
            localBB.FixedMoveLocation = false;
        }

        //If point reached or time to select a new point
        if (Time.time >= NextDirTime || agent.remainingDistance == 0)
        {
            NavMeshHit NavHit;
            //Update Timer
            NextDirTime = Time.time + Random.Range(MinDirTime, MaxDirTime);

            //While no usable point found
            while(SuitablePoint == false)
            {
                //Get random direction and multiply by random range
                Vector3 RandomDirection = Random.insideUnitCircle * Random.Range(10, WanderDistance);
                //Add Random direction to players positon
                RandomDirection += Player.transform.position;
                //Set to same height as player
                RandomDirection.y = Player.transform.position.y;
                //Find Navmesh position closest to point
                NavMesh.SamplePosition(RandomDirection, out NavHit, WanderDistance, -1);

                //Set path
                NavMeshPath Path = new NavMeshPath();
                agent.CalculatePath(NavHit.position, Path);
                //If path is complete and not too close to player
                if (Path.status == NavMeshPathStatus.PathComplete && Vector3.Distance(RandomDirection, Player.transform.position) > 10f)
                {
                    //Mark as suitable
                    localBB.setMoveToLocation(NavHit.position);
                    SuitablePoint = true;
                }
            }

            return NodeState.NODE_SUCCESS;
        }

        return NodeState.NODE_FAILURE;
    }
}
