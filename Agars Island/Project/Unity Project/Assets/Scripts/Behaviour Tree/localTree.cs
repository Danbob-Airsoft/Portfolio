using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

/* This class is used as a base class to all BT_Tree's created.
 * It provides a space for a local blackboard for storing and
 * accessing variables needed throughout the tree
 * 
 * Already included is some functions for getting and setting a
 * 'global' tree variable "moveToLocation"
*/

/// <summary>
///
/// See above
/// 
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class localTree : MonoBehaviour
{

    public BT_Tree tree = new BT_Tree();

    public Vector3 moveToLocation;
    public bool FixedMoveLocation;
    public Vector3 StoredPosition;
    public bool ForceCharge;
    public bool InAmbush;
    public bool CanAmbush;

    public void setMoveToLocation(Vector3 location)
    {
        moveToLocation = location;
    }

    public Vector3 getMoveToLocation()
    {
        return moveToLocation;
    }

}