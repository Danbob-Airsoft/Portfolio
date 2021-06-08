using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour
{
    [Header("Book Inventory Variables")]
    public List<GameObject> BooksCarrying;
    public List<TextMeshProUGUI> InventorySlots;
    public TextMeshProUGUI BooksRemainingText;
    public int MaxBooks;
    private GameObject InventoryHUDBase;

    [Header("Trolly variables")]
    public bool NearTrolly;

    [Header("Returning Book Variables")]
    public bool NearReturnSpot;
    public bool NearGuestReturn;
    public string SpotGenre;

    [Header("Guest Variables")]
    public bool NearGuest;
    public GameObject GuestInRange;

    [Header("Task Inventory Variables")]
    [HideInInspector] public List<GameObject> TaskInventory;
    public TMP_Dropdown InventoryDropDown;
    private TMP_Text LabelText;
    public int SelectedIndex;
    public Image TaskPointer;
    public Image ComplaintsBar;
    private bool DropdownShowing;

    [Header("Other Variables")]
    private GameController GameController;

    [Header("Audio Variables")]
    private AudioSource ThisSource;
    public AudioClip BookCollectClip;
    public AudioClip BookReturnClip;

    // Start is called before the first frame update
    void Start()
    {
        //Set required Variables
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        InventoryHUDBase = BooksRemainingText.transform.parent.gameObject;
        LabelText = InventoryDropDown.transform.GetChild(0).GetComponent<TMP_Text>();
        ThisSource = this.GetComponent<AudioSource>();

        InventoryDropDown.ClearOptions();
        LabelText.text = "Current Tasks";
        UpdateBookRemainingText();
    }

    // Update is called once per frame
    void Update()
    {
        //If within range of a collection point and left clicks
        if (Input.GetMouseButtonDown(0) && BooksCarrying.Count < MaxBooks && !EventSystem.current.IsPointerOverGameObject())
        {
            if (NearTrolly)
            {
                //Take Book from trolly if their is one
                if (GameController.BooksToReturn.Count != 0)
                {
                    //Play collect sound
                    PlaySound(BookCollectClip);
                    //Add to inventory
                    BooksCarrying.Add(GameController.BooksToReturn[0]);
                    //Remove from trolly
                    GameController.BooksToReturn.RemoveAt(0);
                    //Update Books Remaining Text
                    BooksRemainingText.text = "Books Remaining: " + (GameController.BooksToReturn.Count).ToString();
                    //Update Inventory UI
                    UpdateInventoryUI();
                }
            }

            else if ((NearReturnSpot || NearGuestReturn) && SpotGenre != "")
            {
                //Create new Book
                GameObject NewBook = Instantiate(GameController.BookPrefab, GameController.BookStorageOBJ.transform);
                BookBase NewData = NewBook.AddComponent<BookBase>();
                //Add Specific Genre Details
                NewData.Genre = SpotGenre;
                NewData.BookNumber = BooksCarrying.Count;
                //Add to Inventory
                BooksCarrying.Add(NewBook);
                //Update Inventory UI
                UpdateInventoryUI();

                //Check if completing guest return task
                if (NearGuestReturn)
                {
                    //Mark task as complete
                    GuestInRange.GetComponent<TaskBase>().CompleteTask();
                }
                //Play collect sound
                PlaySound(BookCollectClip);
            }
        }

        //Right Click and near spot or guest to return to any books of genre
        if(Input.GetMouseButtonDown(1) && (NearReturnSpot || NearGuest) && BooksCarrying.Count > 0)
        {
            RemoveBook();
        }

        //Press C to toggle Inventory Display
        if (Input.GetKeyDown(KeyCode.C))
        {
            InventoryHUDBase.SetActive(!InventoryHUDBase.activeSelf);
        }

        //Show task menu on key press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (DropdownShowing)
            {
                InventoryDropDown.Hide();
            }
            else
            {
                InventoryDropDown.Show();
            }

            DropdownShowing = !DropdownShowing;
        }

        //Check if Player has tasks
        if(TaskInventory.Count != 0)
        {
            //Enable task pointer if not already
            if(TaskPointer.enabled == false)
            {
                TaskPointer.enabled = true;
            }
            //Get Task at selected index
            GameObject SelectedTask = TaskInventory[SelectedIndex];

            //Get Look at rotation to task
            Quaternion LookAtTask = Quaternion.LookRotation(SelectedTask.transform.position - this.transform.position);
            TaskPointer.transform.up = SelectedTask.transform.position - this.transform.position;
        }

        //Else hide task pointer
        else
        {
            TaskPointer.enabled = false;
        }
    }

    public void RemoveBook()
    {
        //Check for matching genres
        for (int i = 0; i < BooksCarrying.Count; i ++)
        {
            //Get Book and book data
            GameObject Book = BooksCarrying[i];
            BookBase Bookdata = Book.GetComponent<BookBase>();
            //Compare to genre needed
            if (Bookdata.Genre == SpotGenre)
            {
                //Remove from list of books
                BooksCarrying.RemoveAt(i);
                Destroy(Book);
                UpdateInventoryUI();
                //If completing guest task set task to complete
                if (NearGuest)
                {
                    GuestInRange.GetComponent<TaskBase>().CompleteTask();
                }
                //Play return sound
                PlaySound(BookReturnClip);

                break;
            }
        }
    }

    private void UpdateInventoryUI()
    {
        int i;
        //Loop through max inventory
        for(i = 0; i < BooksCarrying.Count; i++)
        {
            InventorySlots[i].text = BooksCarrying[i].GetComponent<BookBase>().Genre;
        }

        if(i < MaxBooks)
        {
            for(int j = i; j < MaxBooks; j++)
            {
                InventorySlots[j].text = "";
            }
        }
    }

    public void UpdateTaskInventory()
    {
        //Clear Dropdown
        InventoryDropDown.ClearOptions();
        List<string> CurrentTasksList = new List<string>();
        //Loop through each task in Inventory
        foreach (GameObject TaskOBJ in TaskInventory)
        {
            TaskBase TaskData = TaskOBJ.GetComponent<TaskBase>();
            //Add Data to string list
            CurrentTasksList.Add(TaskData.TaskDescription);
        }

        foreach(string Title in CurrentTasksList)
        {
            InventoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = Title });
        }

        if(TaskInventory.Count != 0)
        {
            LabelText.text = TaskInventory[SelectedIndex].GetComponent<TaskBase>().TaskDescription;
        }
    }

    public void SelectedTask(int NewTaskIndex)
    {
        if(TaskInventory.Count != 0)
        {
            LabelText.text = TaskInventory[NewTaskIndex].GetComponent<TaskBase>().TaskDescription;
        }
        SelectedIndex = NewTaskIndex;
    }

    private void PlaySound(AudioClip ToPlay)
    {
        //Play Sound
        ThisSource.PlayOneShot(ToPlay);
    }

    public void UpdateBookRemainingText()
    {
        if(BooksRemainingText != null)
        {
            BooksRemainingText.text = "Books Remaining: " + (GameController.BooksToReturn.Count).ToString();
        }

    }

    public void UpdateCompaints(float Current, float Max)
    {
        ComplaintsBar.fillAmount = Current / Max;
    }
}
