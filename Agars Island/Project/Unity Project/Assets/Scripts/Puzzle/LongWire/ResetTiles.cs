using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Resets Long Wire puzzle
///
/// Created by: Daniel Bailey
/// Edited by:
/// </summary>
public class ResetTiles : MonoBehaviour
{
    private void OnMouseDown()
    {
        //Trigger total puzzle reset
        this.transform.parent.GetComponent<LongWireController>().ResetPuzzle();
    }
}
