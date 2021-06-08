using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{
    //Player Object and Inventory
    private GameObject Player;
    private PlayerInventory Inventory;
    //Total tasks remaining
    public float TasksRemaining;
    //All books returned flag
    public bool AllBooksReturned;
    //All tasks complete flag
    private bool AllTasksComplete;
    private GameController LevelController;

    //UI Variables
    public CanvasGroup LevelCompleteCanvas;
    public CanvasGroup HUDCanvas;
    public Button NextLevelButton;

    private AudioSource ThisSource;
    public AudioClip CompleteClip;

    private bool CompleteTriggered;

    // Start is called before the first frame update
    void Start()
    {
        LevelController = this.transform.GetComponent<GameController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Inventory = Player.GetComponent<PlayerInventory>();
        ThisSource = this.GetComponent<AudioSource>();
        CompleteTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if all books have been returned and first detection
        if (LevelController.BooksToReturn.Count == 0 && AllBooksReturned == false)
        {
            //Set books section of game to complete
            AllBooksReturned = true;
        }
        //Check if all books are returned and 0 guest tasks remain
        if (AllBooksReturned && TasksRemaining == 0)
        {
            //Set all objectives to complete
            AllTasksComplete = true;
        }
        //Check if all tasks complete and not carrying any books
        if (AllTasksComplete && Inventory.BooksCarrying.Count == 0 && CompleteTriggered == false)
        {
            CompleteTriggered = true;
            //End Game
            LevelComplete();
        }
    }

    public void LevelComplete()
    {
        ThisSource.PlayOneShot(CompleteClip);

        //Check if final level
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            NextLevelButton.interactable = false;
        }
        else
        {
            //Save level as complete
            PlayerPrefs.SetInt("LevelsOpen", SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Activate Level Complete Screen
        LevelCompleteCanvas.alpha = 1;
        LevelCompleteCanvas.interactable = true;
        LevelCompleteCanvas.blocksRaycasts = true;

        //Hide HUD
        HUDCanvas.alpha = 0;
        HUDCanvas.interactable = false;
        HUDCanvas.blocksRaycasts = false;

        //Freeze Time
        Time.timeScale = 0;
    }
}
