using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadiatorGames.UI;
using RadiatorGames.UI.SwapCanvasGroup;

/// <summary>
///
/// [Deprecated]
/// Activates a puzzle
/// 
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class ActivatePuzzle : Interactable
{

    private HUDManager HUDmanager;
    [SerializeField] private CanvasGroup puzzleCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        HUDmanager = GameObject.FindWithTag("Player").GetComponent<HUDManager>();
        GroupSwapper.HideCanvasGroup(puzzleCanvas);
        base.Init();
    }

    public override void Interact()
    {
        HUDmanager.HidePlayerHUDs(puzzleCanvas);
        base.Interact();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
