using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// Rotates a sprite when clicked, and marks as complete when rotated by maxAngleChange
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class ScrewTurnOnClick : MonoBehaviour
{
    private bool pressed;

    private float maxAngleChange = 100f;

    private float angleChange;

    [SerializeField] private float screwSpeed = 1f;
    [SerializeField] private float fallSpeed = 1f;

    private ShortWiresPuzzleManager manager;

    private bool complete = false;
    // Start is called before the first frame update
    void Start()
    {
        manager = transform.parent.GetComponentInParent<ShortWiresPuzzleManager>();
        
        manager.DeclareScrew();
    }

    private void Update()
    {
        if (pressed)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - (screwSpeed));
            angleChange += screwSpeed;
        }

        if (angleChange >= maxAngleChange)
        {
            if (!complete)
            {
                manager.ScrewComplete();
                GetComponent<BoxCollider>().enabled = false;
                complete = true;
            }

            Fall();
        }
    }

    // Update is called once per frame
    public void ButtonDownOnScrew()
    {
        pressed = true;
    }
    
    public void ButtonUpOnScrew()
    {
        pressed = false;
    }

    private void Fall()
    {
        if (transform.position.y > -1000)
            transform.position = new Vector3(transform.position.x, transform.position.y - (fallSpeed*Time.deltaTime), transform.position.z);
    }

    private void OnMouseDown()
    {
        pressed = true;
    }

    private void OnMouseUp()
    {
        pressed = false;
    }

    private void OnMouseExit()
    {
        pressed = false;
    }
}
