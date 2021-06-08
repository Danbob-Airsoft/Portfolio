using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class GuestSeatChecker : MonoBehaviour
{
    private GuestSpawner ThisSpawner;
    GuestTaskScript ThisTaskScript;
    AILerp ThisDestSetter;
    public bool Exiting;
    private Canvas WorldCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Exiting = false;
        ThisDestSetter = this.GetComponent<AILerp>();
        ThisTaskScript = this.GetComponent<GuestTaskScript>();
        WorldCanvas = transform.GetChild(1).GetComponent<Canvas>();
        WorldCanvas.enabled = false;
        ThisSpawner = this.transform.root.GetComponent<GuestSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        //Has guest reached seat
        if (ThisDestSetter.reachedDestination)
        {
            //If leaving, destroy guest
            if (Exiting)
            {
                Destroy(this.gameObject);
            }
            //Else enable task chance generator and disable pathfinding
            else
            {
                ThisDestSetter.enabled = false;
                ThisTaskScript.enabled = true;
                transform.rotation = new Quaternion(0, 0, 0, 1);
                this.enabled = false;
            }
        }
    }
}
