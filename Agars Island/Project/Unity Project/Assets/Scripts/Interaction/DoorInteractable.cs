using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Handles door interaction
/// 
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class DoorInteractable : Interactable
{

    private bool open = false;

    private Transform player;

    public bool locked = false;
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {

        
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            if (!locked)
            {
                if (open)
                {
                    anim.SetTrigger("Close");
                }
                else
                {
                    if (Vector3.Dot(transform.forward, player.forward) > 0)
                    {
                        anim.SetTrigger("OpenBackward");
                    }
                    else
                    {
                        anim.SetTrigger("OpenForward");
                    }
                }

                open = !open;
            }
            else
            {
                anim.SetTrigger("Locked");
            }
        }
        else
        {
            Debug.Log("Door already in animation");
        }
    }
}
