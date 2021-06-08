using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBookTask : TaskBase
{
    public string GenreToCollect;
    private PlayerInventory Inventory;

    // Start is called before the first frame update
    void Start()
    {
        GameController ThisController = this.transform.root.GetComponent<GameController>();
        GenreToCollect = ThisController.GenresToChoose[Random.Range(0, ThisController.GenresToChoose.Length)];
        WorldCanvas = transform.parent.GetChild(1).GetComponent<Canvas>();
        WorldCanvas.enabled = true;
        //Update Task Description
        TaskDescription = "Fetch me a " + GenreToCollect + " book";
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerInventory>().UpdateTaskInventory();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        //Set Inventory spot details to this guest
        if (other.gameObject.CompareTag("Player") && Completed == false)
        {
            Inventory = other.GetComponent<PlayerInventory>();
            Inventory.NearGuest = true;
            Inventory.SpotGenre = GenreToCollect;
            Inventory.GuestInRange = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Clear Inventory of guest details
        if (other.gameObject.CompareTag("Player") && Completed == false)
        {
            Inventory = other.GetComponent<PlayerInventory>();
            Inventory.NearGuest = false;
            Inventory.SpotGenre = "";
            Inventory.GuestInRange = null;
        }
    }

    public override void CompleteTask()
    {
        Inventory.NearGuest = false;
        Inventory.SpotGenre = "";
        Inventory.GuestInRange = null;
        base.CompleteTask();
        //Destroy Task Object
        Destroy(this.gameObject);
    }
}
