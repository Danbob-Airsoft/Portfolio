using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskBase : MonoBehaviour
{
    //Task Details
    public bool Completed;
    public string TaskTitle;
    public string TaskDescription;
    public Canvas WorldCanvas;

    public virtual void CompleteTask()
    {
        //Remove from Task inventory
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerInventory Inventory = Player.GetComponent<PlayerInventory>();
        int ThisIndex = Inventory.TaskInventory.IndexOf(this.gameObject);
        Inventory.TaskInventory.Remove(this.gameObject);
        //Update UI
        Inventory.UpdateTaskInventory();

        if (Inventory.SelectedIndex == ThisIndex)
        {
            Inventory.SelectedTask(0);
        }
        //Disable UI
        WorldCanvas.enabled = false;
        //Set completed to true (Triggers other scripts)
        Completed = true;
    }

    public void UpdateTaskUI(float TimePassed, float MaxTime)
    {
        Image FillBar = WorldCanvas.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        FillBar.fillAmount = TimePassed / MaxTime;
    }
}
