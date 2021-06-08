using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadiatorGames.UI.SwapCanvasGroup;

/// <summary>
///
/// World-space canvases are used for puzzles. This "activates" those canvases by telling
/// the HUD manager to change our state so the player can interact with it
/// 
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
[RequireComponent(typeof(Collider))]
public class ActivateWorldCanvas : Interactable
{
    private HUDManager HUDmanager;
    // Start is called before the first frame update
    void Start()
    {
        HUDmanager = GameObject.FindWithTag("Player").GetComponent<HUDManager>();
    }
    
    public override void Interact()
    {
        HUDmanager.HidePlayerHUDs(true, GetComponent<Collider>());
        base.Interact();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
