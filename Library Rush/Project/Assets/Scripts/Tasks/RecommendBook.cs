using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecommendBook : TaskBase
{
    public float LoginTime;
    private float TaskEndtime;
    private bool TaskActive;
    private AudioSource TaskSource;

    private void Start()
    {
        TaskActive = false;
        LoginTime = 1f;
        TaskEndtime = Time.time + Mathf.Infinity;
        WorldCanvas = transform.parent.GetChild(1).GetComponent<Canvas>();
        WorldCanvas.enabled = true;
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerInventory>().UpdateTaskInventory();
        TaskSource = this.GetComponent<AudioSource>();
    }

    //On trigger check for player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if player caused trigger
        if (collision.CompareTag("Player"))
        {
            TaskSource.Play();
            //Set timer for finishing tasks
            TaskActive = true;
            TaskEndtime = Time.time + LoginTime;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if player caused trigger
        if (other.CompareTag("Player"))
        {
            //Reset Timer
            TaskActive = false;
            TaskEndtime = Time.time + Mathf.Infinity;
        }
    }

    private void Update()
    {
        //Check if timer has passed
        if (TaskActive && Time.time >= TaskEndtime)
        {
            //Task Complete
            base.CompleteTask();
            //Destroy Task Object
            Destroy(this.gameObject);
        }
        //else loop progress sound until complete
        else if(TaskActive && !TaskSource.isPlaying)
        {
            TaskSource.Play();
        }
    }
}
