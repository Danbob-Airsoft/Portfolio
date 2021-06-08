using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBook : TaskBase
{
    //Genre of book selected
    public string GenreChosen;
    //Level Game Controller
    private GameController ThisController;
    //Player Object and inventory
    private GameObject Player;
    private PlayerInventory Inventory;

    // Start is called before the first frame update
    void Start()
    {
        //Set variables needed
        GameController ThisController = this.transform.root.GetComponent<GameController>();
        GenreChosen = ThisController.GenresToChoose[Random.Range(0, ThisController.GenresToChoose.Length)];
        Player = GameObject.FindGameObjectWithTag("Player");
        Inventory = Player.GetComponent<PlayerInventory>();
        WorldCanvas = transform.parent.GetChild(1).GetComponent<Canvas>();
        WorldCanvas.enabled = true;

        //Update Task Description
        TaskDescription = "Take this " + GenreChosen + " book back for me!";
        Player.GetComponent<PlayerInventory>().UpdateTaskInventory();
    }

    //On Object eneter trigger box
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Set Inventory spot details to this guest
        if (other.gameObject.CompareTag("Player") && Completed == false)
        {
            Inventory.NearGuestReturn = true;
            Inventory.SpotGenre = GenreChosen;
            Inventory.GuestInRange = this.gameObject;
        }
    }

    //On Object exit trigger box
    private void OnTriggerExit2D(Collider2D other)
    {
        //Clear Inventory of guest details
        if (other.gameObject.CompareTag("Player") && Completed == false)
        {
            Inventory.NearGuestReturn = false;
            Inventory.SpotGenre = "";
            Inventory.GuestInRange = null;
        }
    }

    public override void CompleteTask()
    {        
        //Clear Spot data
        Inventory.NearGuestReturn = false;
        Inventory.SpotGenre = "";
        Inventory.GuestInRange = null;
        base.CompleteTask();
        //Destroy Task Object
        Destroy(this.gameObject);
    }
}
