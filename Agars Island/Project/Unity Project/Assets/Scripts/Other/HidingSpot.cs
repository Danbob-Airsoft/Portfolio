using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Hiding Spot. Manages player behaviour while in a hiding spot
///
/// Created by: Daniel Bailey
/// Edited by: Lewis Arnold
/// </summary>
public class HidingSpot : Interactable
{
    private GameObject Player;
    private PlayerMovement MoveScript;

    public Transform animStartTransform;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MoveScript = Player.GetComponent<PlayerMovement>();
    }

    public override void Interact()
    {
        if(MoveScript.IsHiding == false)
        {
            //Set IsHiding
            MoveScript.IsHiding = true;
            //Prevent Player Movement
            MoveScript.enabled = false;
        }
        else
        {
            //Set IsHiding
            MoveScript.IsHiding = false;
            StartCoroutine(MoveEnableDelay());
        }
        
        // LA - 27/02/21 ; 20:49
        base.Interact();
    }

    private IEnumerator MoveEnableDelay()
    {
        yield return new WaitForSeconds(2f);
        //Enable Movement
        MoveScript.enabled = true;
    }
}
