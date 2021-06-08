using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollyScript : MonoBehaviour
{
    //On object entering trigger box
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if object is player
        if (collision.gameObject.name == "Player")
        {
            //Set Near Trolly (For taking books from trolly)
            collision.gameObject.GetComponent<PlayerInventory>().NearTrolly = true;
        }
    }

    //On object leaving trigger box
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if object is player
        if (collision.gameObject.name == "Player")
        {
            //Set Near Trolly (For taking books from trolly)
            collision.gameObject.GetComponent<PlayerInventory>().NearTrolly = false;
        }
    }
}
