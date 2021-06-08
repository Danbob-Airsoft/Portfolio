using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
///
/// Handles the inventory during the game
///
/// Created by: Lewis Arnold
/// Edited by: Daniel Bailey
/// </summary>
public class InventoryTracker : MonoBehaviour
{
    //Number of batteries being carried
    public int batteries;
    //Number of sanity meds being carried
    public int meds;
    //Current access level
    private int keycardLevel;
    //Note variables
    private List<GameObject> NotesList;
    public TMP_Dropdown NoteSelector;
    public TextMeshProUGUI NoteDescBox;
    private TextMeshProUGUI NoteLabel;
    //Inventory UI variables
    [SerializeField] private TextMeshProUGUI batteryText;
    [SerializeField] private TextMeshProUGUI medsText;
    [SerializeField] private TextMeshProUGUI PickUpText;
    //Pickup animation
    private Animator pickUpTextAnim;
    //Flashlight script
    private ToggleFlashlight TF;
    //Audio sources
    [SerializeField] private AudioSource batteryPickup;
    [SerializeField] private AudioSource paperPickup;
    [SerializeField] private AudioSource genericPickup;

    private TutorialManager tutManager;

    // Start is called before the first frame update
    void Start()
    {
        NotesList = new List<GameObject>();
        int[] loadedData = LoadInventory.Load();
        //batteries = loadedData[0];
        //meds = loadedData[1];
        //keycardLevel = loadedData[2];
        //Set batteries, meds and keycard level
        batteries = 0;
        meds = 0;
        keycardLevel = 0;
        UpdateText();

        //Set animator
        if (PickUpText)
        {
            pickUpTextAnim = PickUpText.gameObject.GetComponent<Animator>();
        }
        //Set other variables
        TF = GetComponent<ToggleFlashlight>();
        NoteLabel = NoteSelector.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        UpdateNotesScreen();

        tutManager = GameObject.FindWithTag("Manager").GetComponent<TutorialManager>();
    }

    // TODO: Change this to call from a single object to avoid dependency issues
    private void OnApplicationQuit()
    {
        int[] saveData = { batteries, meds, keycardLevel };
        LoadInventory.Save(saveData);
    }

    //---------------------------------------- Inventory Management -----------------------------------

    //Add battery
    public void AddBattery(int number)
    {
        batteries += number; UpdateText();
        ShowPickupText("Picked up battery");
    }

    //Remove battery
    public void RemoveBattery(int number) 
    { 
        batteries -= number; UpdateText(); 
    }

    //Add sanity meds
    public void AddMeds(int number)
    {
        meds += number; UpdateText();
        ShowPickupText("Picked up meds");
    }

    //Remove sanity meds
    public void RemoveMeds(int number)
    {
        meds -= number; UpdateText();
    }

    //Set keycard level
    public void SetKeycardLevel(int level)
    {
        keycardLevel = level;
        ShowPickupText("Picked up keycard level " + level.ToString());
    }

    public int GetKeycardLevel() 
    { 
        return keycardLevel; 
    }
    //Enable flashlight usage
    public void GiveFlashlight()
    {
        TF.hasFlashlight = true;
        TF.canToggle = true;
        Debug.Log("Player given flashlight");
        CheckpointManager.instance.FlashlightGot();
        tutManager.StartFlashLighTut();
        ShowPickupText("Picked up flashlight");
    }

    //---------------------------------------- Note Collection -----------------------------------
    public void NoteCollection(GameObject Note)
    {
        //Add note to list of collected notes
        NotesList.Add(Note);
        //Call Note Updater
        UpdateNotesScreen();
        //Disable Note
        Note.transform.position = new Vector3(-87f, 3f, 10f);
        // Show GUI
        ShowPickupText("Picked up note '" + Note.GetComponent<NoteDetails>().LinkedNoteData.title + "'");
        //Debug.LogError(Note.GetComponent<NoteDetails>().LinkedNoteData.title);

    }

    private void UpdateNotesScreen()
    {
        //Clear DropDownList
        NoteSelector.ClearOptions();
        //Make a new list of note titles
        List<string> NoteTitles = new List<string>();
        //Loop through each note in collected notes
        foreach (GameObject Note in NotesList)
        {
            //Get Note Data
            NoteData NoteData = Note.GetComponent<NoteDetails>().LinkedNoteData;
            //Add to list for side bar
            NoteTitles.Add(NoteData.title);
        }
        //Add each title to new dropdown option
        foreach (string Title in NoteTitles)
        {
            NoteSelector.options.Add(new TMP_Dropdown.OptionData() { text = Title });
        }

        //Check if player has notes
        if(NotesList.Count != 0)
        {
            GameObject CurrentSelection = NotesList[NoteSelector.value];
            NoteData SelectedNote = CurrentSelection.GetComponent<NoteDetails>().LinkedNoteData;
            NoteDescBox.text = SelectedNote.description;
        }

        //Reset Label
        NoteLabel.text = "Notes";
    }

    //-------------------------------------- Display Pickup Text ----------------------------------
    private void ShowPickupText(string text)
    {
        if (pickUpTextAnim)
        {
            PickUpText.text = text;
            if (pickUpTextAnim.GetCurrentAnimatorStateInfo(0).IsName("InventHidden"))
                pickUpTextAnim.SetTrigger("FadeIn");

        }
    }

    private void UpdateText()
    {
        batteryText.text = batteries.ToString();
        medsText.text = meds.ToString();
    }

    public void ChangeSelectedNote(int IndexSelected)
    {
        //Set selected on dropdown
        NoteSelector.value = IndexSelected;
        //Get Note in inventory at index
        GameObject SelectedNote = NotesList[IndexSelected];
        NoteData SelectedDetails = SelectedNote.GetComponent<NoteDetails>().LinkedNoteData;
        //Set Note Text to selected notes data
        NoteDescBox.text = SelectedDetails.description;
        //Reset Label
        NoteLabel.text = "Notes";
    }
}
