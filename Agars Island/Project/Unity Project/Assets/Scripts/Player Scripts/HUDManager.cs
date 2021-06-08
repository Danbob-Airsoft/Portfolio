using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using RadiatorGames.UI.SwapCanvasGroup;
using UnityEngine.UI;

/// <summary>
///
/// Manages all the HUD of the player. Responsible for correctly switching canvas panels.
/// Responsible for toggling player movement in certain HUD states
///
/// Created by: Lewis Arnold
/// Edited by: Daniel Bailey
/// </summary>
public class HUDManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup HUD;
    [SerializeField] private CanvasGroup inventoryGroup;
    private bool inventoryOpen = false;
    
    // this boolean is not *needed*
    // however, checking if the canvas group is null is very expensive
    // whereas boolean checking is as cheap as it gets
    
    // PERFORMANCEEEEEEEEE
    // run this shit on a pentium duo
    private bool puzzleOpen = false;
    private CanvasGroup puzzleGroup = null;

    private bool altUIOpen = false;
    private Collider altCollider;

    private List<CanvasGroup> allCanvases = new List<CanvasGroup>();

    private static PlayerMovement playerMovement;
    private static ToggleFlashlight TF;

    [SerializeField] private CanvasGroup pauseGroup;
    private bool pauseOpen;

    [SerializeField] private CanvasGroup NotesGroup;

    private static float rotSpeed;
    private bool PlayerDead;

    [Header("Hints")]
    [SerializeField] private CanvasGroup InteractHint;

    private PlayerSanity playerSanity;

    //Game Over Canvas
    public CanvasGroup GameOverCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
        TF = GetComponent<ToggleFlashlight>();
        playerMovement = GetComponent<PlayerMovement>();
        playerSanity = GetComponent<PlayerSanity>();
        rotSpeed = playerMovement.RotationSpeed;
        PlayerDead = false;
                
        GroupSwapper.HideCanvasGroup(pauseGroup);
        GroupSwapper.HideCanvasGroup(inventoryGroup);
        GroupSwapper.HideCanvasGroup(NotesGroup);
        GroupSwapper.ShowCanvasGroup(HUD);
        MouseModeGame();

        allCanvases.Add(HUD);
        allCanvases.Add(inventoryGroup);
        allCanvases.Add(pauseGroup);
        allCanvases.Add(NotesGroup);

        if (!playerMovement || !TF)
        {
            Debug.LogAssertion("COULD NOT GET PLAYER OR FLASHLIGHT. SCRIPT WILL NOT WORK");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && PlayerDead == false)
        {

            if (inventoryOpen)
            {
                GroupSwapper.HideCanvasGroup(pauseGroup);
                GroupSwapper.HideCanvasGroup(inventoryGroup);
                GroupSwapper.HideCanvasGroup(NotesGroup);
                GroupSwapper.ShowCanvasGroup(HUD);
                MouseModeGame();
                inventoryOpen = !inventoryOpen;
            }
            else if (puzzleOpen || altUIOpen)
            {
                ResetHUD();
            }

            else
            {
                GroupSwapper.ShowCanvasGroup(inventoryGroup);
                GroupSwapper.HideCanvasGroup(HUD);
                GroupSwapper.HideCanvasGroup(pauseGroup);
                MouseModeUI();
                inventoryOpen = !inventoryOpen;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PlayerDead == false)
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Hides all the player's HUD canvas groups, and optionally shows another canvasgroup
    /// </summary>
    /// <param name="puzzleCanvasGroup">Optional: new canvas group to open</param>
    public void HidePlayerHUDs(CanvasGroup puzzleCanvasGroup)
    {
        puzzleGroup = puzzleCanvasGroup;
        puzzleOpen = true;
        
        altUIOpen = false;
        inventoryOpen = false;

        if (puzzleOpen)
            GroupSwapper.ShowCanvasGroup(puzzleCanvasGroup);

        //playerMovement.RotationSpeed = 0f;

        foreach (CanvasGroup group in allCanvases)
        {
            GroupSwapper.HideCanvasGroup(group);
        }
        
        MouseModeUI();
    }

    public void HidePlayerHUDs(bool altUIOpen = false, Collider altCollider = null)
    {
        puzzleOpen = false;
        this.altUIOpen = altUIOpen;
        this.altCollider = altCollider;
        puzzleGroup = null;
        inventoryOpen = false;

        //playerMovement.RotationSpeed = 0f;

        foreach (CanvasGroup group in allCanvases)
        {
            GroupSwapper.HideCanvasGroup(group);
        }

        if (altUIOpen)
            altCollider.enabled = false;
        
        MouseModeUI();
    }

    public void ResetHUD()
    {
        if (altUIOpen)
            altCollider.enabled = true;
        
        Debug.Log("Reset HUD");
        puzzleOpen = false;
        altUIOpen = false;
        inventoryOpen = false;
        
        // hide puzzle group first otherwise it'll be set to null in HidePlayerHUDs
        if (puzzleGroup)
            GroupSwapper.HideCanvasGroup(puzzleGroup);
        
        HidePlayerHUDs();

        GroupSwapper.ShowCanvasGroup(HUD);
        //playerMovement.RotationSpeed = rotSpeed;

        MouseModeGame();
    }


    public static void MouseModeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMovement.enabled = true;
        TF.canToggle = true;
    }

    public static void MouseModeUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.enabled = false;
        TF.canToggle = false;
    }

    public bool ShouldInteract()
    {
        Debug.Log("!" + altUIOpen.ToString() + "||" + puzzleOpen.ToString());
        
        Debug.Log((!(altUIOpen || puzzleOpen)).ToString());
        
        
        if (altUIOpen || puzzleGroup)
            Debug.Log("True");
        
        
        return !(altUIOpen || puzzleOpen);
    }

    public void ShowPickUpHint()
    {
        
    }

    public void ShowInteractHint()
    {
        GroupSwapper.ShowCanvasGroup(InteractHint);
    }

    public void HideInteractHint()
    {
        GroupSwapper.HideCanvasGroup(InteractHint);
    }

    public void ShowNotesScreen()
    {
        GroupSwapper.HideCanvasGroup(inventoryGroup);
        GroupSwapper.ShowCanvasGroup(NotesGroup);
    }

    public void LeaveNotesScreen()
    {
        GroupSwapper.ShowCanvasGroup(inventoryGroup);
        GroupSwapper.HideCanvasGroup(NotesGroup);
    }

    public void TogglePause()
    {
        if (pauseOpen)
        {
            GroupSwapper.ShowCanvasGroup(HUD);
            GroupSwapper.HideCanvasGroup(pauseGroup);
            MouseModeGame();
        }
        else
        {
            GroupSwapper.HideCanvasGroup(HUD);
            GroupSwapper.HideCanvasGroup(inventoryGroup);
            GroupSwapper.HideCanvasGroup(NotesGroup);
            GroupSwapper.ShowCanvasGroup(pauseGroup);
            inventoryOpen = false;
            MouseModeUI();
        }

        pauseOpen = !pauseOpen;
    }

    public void TriggerGameOver()
    {
        PlayerDead = true;

        GroupSwapper.HideCanvasGroup(HUD);
        GroupSwapper.HideCanvasGroup(NotesGroup);
        GroupSwapper.HideCanvasGroup(inventoryGroup);

        GroupSwapper.ShowCanvasGroup(GameOverCanvas);
        MouseModeUI();
    }

    public void ResetGameOver()
    {
        PlayerDead = false;
        MouseModeGame();
        Time.timeScale = 1f;
    }
}
