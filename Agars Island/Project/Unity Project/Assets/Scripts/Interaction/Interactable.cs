using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all interactables in the world
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class Interactable : MonoBehaviour
{
    [Tooltip("If an animation is to be played when interacted with (e.g. door open)," +
             "attach the Animator here. Animations are triggered using trigger 'Interact'")]
    public Animator anim;

    [HideInInspector] public GiveCheckPoint chkpnt;

    [HideInInspector] public bool canInteract = true;
    public string interactableType;
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Inits the base class, as Base.Start can't be called from children.
    /// </summary>
    public void Init()
    {
        chkpnt = GetComponent<GiveCheckPoint>();
        anim = GetComponent<Animator>();
    }
    
    /// <summary>
    /// Virtual function to be overridden by children.
    /// Base.Interact should still be called to ensure
    /// animations are still played if you want them.
    /// </summary>
    virtual public void Interact()
    {
        if (canInteract)
        {
            //Debug.Log("Base Interact");
            if (anim)
                anim.SetTrigger("Interact");
        }
        else
        {
            Debug.Log("Interactable disabled");
        }
    }
}
