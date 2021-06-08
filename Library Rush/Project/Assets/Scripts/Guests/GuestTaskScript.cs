using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GuestTaskScript : MonoBehaviour
{
    //Min and Max time between task chance
    public float MinTaskTime;
    public float MaxTaskTime;
    //Time next chance for task will happen
    private float NextTaskChanceTime;
    //Waiting for task to be complete flag
    private bool WaitingForTask;
    //Min and Max time between task start and fail
    public float MixTaskFailTime;
    public float MaxTaskFailTime;
    //Task Start Time
    private float TaskStartTime;
    private float FailTime;

    //Difficulty level
    public float DifficultyLevel;
    //Enterance point
    public Transform SpawnPosition;
    //Task created
    private TaskBase GeneratedTask;
    //List of tasks based on seat chosen
    private List<GameObject> SelectableTasks;
    //Seat chosen
    public GameObject OccupiedSeat;

    //Level controller
    private GameObject LevelController;
    //Complete checking script
    private LevelCompleteScript CompleteChecker;
    //Player Object
    private GameObject Player;

    //Pathfinding scripts
    AILerp ThisPath;
    AIDestinationSetter ThisDestSetter;

    //Time before leaving without spawning task
    public float ExitDelay;
    private float ExitTime;

    //Audio Variables
    private AudioSource ThisSource;
    public AudioClip TaskGenerationClip;
    public AudioClip TaskCompleteClip;
    public AudioClip TaskFailedClip;

    // Start is called before the first frame update
    void Start()
    {
        //Set required variables
        Player = GameObject.FindGameObjectWithTag("Player");
        NextTaskChanceTime = Time.time + Random.Range(MinTaskTime, MaxTaskTime);
        WaitingForTask = false;
        ThisPath = this.GetComponent<AILerp>();
        ThisDestSetter = this.GetComponent<AIDestinationSetter>();
        ExitTime = Time.time + ExitDelay;
        LevelController = this.transform.root.gameObject;
        CompleteChecker = LevelController.GetComponent<LevelCompleteScript>();

        SelectableTasks = OccupiedSeat.GetComponent<SeatStruct>().SeatTasks;

        ThisSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if time for next task opportunity and no task has been generated
        if (Time.time >= NextTaskChanceTime && WaitingForTask == false)
        {
            //Generate Random 
            int ChanceGenerated = Random.Range(1, 20);
            //If Chance is greater than the difficulty, start a task
            if (DifficultyLevel >= ChanceGenerated)
            {
                //Choose random task from potentials
                GameObject RandomTask = Instantiate(SelectableTasks[Random.Range(0, SelectableTasks.Count)], this.transform);
                //Set Task Position (for collision box)
                RandomTask.transform.localPosition = new Vector3(0, 0, 0);
                //Get task component for data
                GeneratedTask = RandomTask.GetComponent<TaskBase>();
                //Set waiting for task to be complete
                WaitingForTask = true;
                //Add Task to number remaining
                CompleteChecker.TasksRemaining += 1;
                //Set Exit time if task not complete in time
                FailTime = Random.Range(MixTaskFailTime, MaxTaskFailTime);
                ExitTime = Time.time + FailTime;
                TaskStartTime = Time.time;

                //Add to Tasks HUD
                Player.GetComponent<PlayerInventory>().TaskInventory.Add(RandomTask);

                //Play Sounds
                PlaySound(TaskGenerationClip);
            }

            //Else increase time to next check
            else
            {
                //Reset Timer for next chance
                NextTaskChanceTime = Time.time + Random.Range(MinTaskTime, MaxTaskTime);
            }
        }

        //Else if Task has been generated
        else if(WaitingForTask)
        {
            //Check if completed
            if (GeneratedTask.Completed)
            {
                PlaySound(TaskCompleteClip);
                CompleteChecker.TasksRemaining -= 1;
                MoveToExit();
            }

            //Otherwise update task UI
            else
            {
                //Update task UI
                GeneratedTask.UpdateTaskUI(Time.time - TaskStartTime, FailTime);
            }

            //Check if player has not completed task within set time
            if(Time.time >= ExitTime)
            {
                PlaySound(TaskFailedClip);
                CompleteChecker.TasksRemaining -= 1;
                //Add complaint
                this.transform.root.GetComponent<GameOverScript>().AddComplaint();
                //Remove Task from task list
                PlayerInventory Inventory = Player.GetComponent<PlayerInventory>();
                //Check if task is currently selected task
                int ThisIndex = Inventory.TaskInventory.IndexOf(this.gameObject);
                Inventory.TaskInventory.Remove(GeneratedTask.gameObject);
                if (Inventory.SelectedIndex == ThisIndex)
                {
                    Inventory.SelectedTask(0);
                }
                //Update Task Inventory
                Inventory.UpdateTaskInventory();
                //Disable UI
                GeneratedTask.WorldCanvas.enabled = false;
                //Cancel Task
                Destroy(GeneratedTask.gameObject);
                //Move to exit
                MoveToExit();
            }

        }

        //Else if enough time has passed without a task or player has returned all books
        else if(Time.time >= ExitTime || CompleteChecker.AllBooksReturned == true)
        {
            Debug.Log("Leaving Without Task");
            MoveToExit();
        }
    }

    private void MoveToExit()
    {
        //Assign seat to not taken
        OccupiedSeat.GetComponent<SeatStruct>().Taken = false;
        OccupiedSeat.GetComponent<SeatStruct>().TakenBy = null;
        GuestSpawner ThisSpawner = this.transform.root.GetComponent<GuestSpawner>();

        //Increase Number of available seats
        ThisSpawner.NumberOfSeatsRemaining += 1;

        //Enable Pathfinding script
        ThisPath.enabled = true;

        //Set target to entrance
        ThisDestSetter.target = SpawnPosition;

        //Enable Seat Checker
        this.GetComponent<GuestSeatChecker>().enabled = true;
        this.GetComponent<GuestSeatChecker>().Exiting = true;

        //Disable this script
        this.enabled = false;
    }

    private void PlaySound(AudioClip ToPlay)
    {
        ThisSource.PlayOneShot(ToPlay);
    }
}
