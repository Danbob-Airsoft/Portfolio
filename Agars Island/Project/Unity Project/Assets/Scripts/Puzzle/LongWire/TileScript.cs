using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// NEEDS DESCRIPTION
///
/// Created by: Daniel Bailey
/// Edited by:
/// </summary>
public class TileScript : MonoBehaviour
{
    //List of acceptable roations for the puzzle to be complete
    public List<float> DesiredRotations;
    //Tile starting rotation
    public Quaternion StartRotation;

    private void Start()
    {
        //Set start rotation to current
        StartRotation = this.transform.rotation;
    }

    //Set Up Rotation
    private void OnMouseDown()
    {
        //When clicked rotate tile
        RotateTile();
    }

    public void RotateTile()
    {
        //Roate 90*
        this.transform.Rotate(0, 90, 0);
        //Check overall puzzle completion
        this.transform.parent.GetComponent<LongWireController>().CheckPuzzle();
    }

    public void ResetTile()
    {
        //Set tile to starting rotation
        this.transform.rotation = StartRotation;
    }
}
