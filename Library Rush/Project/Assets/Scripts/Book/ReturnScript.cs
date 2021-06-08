using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnScript : MonoBehaviour
{
    public string SpotGenre;

    private void Start()
    {
        //Set Spot Tag
        this.gameObject.tag = SpotGenre;
    }

    //On Object enter trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if Object is Player
        if(collision.gameObject.CompareTag("Player"))
        {
            //Set NearSpot and SpotNear, for returning books to spot
            collision.GetComponent<PlayerInventory>().NearReturnSpot = true;
            collision.GetComponent<PlayerInventory>().SpotGenre = SpotGenre;
        }
    }

    //On Object exit trigger collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if Object is Player
        if (collision.gameObject.CompareTag("Player"))
        {
            //Clear spot variables
            collision.GetComponent<PlayerInventory>().NearReturnSpot = false;
            collision.GetComponent<PlayerInventory>().SpotGenre = "";
        }
    }
}
