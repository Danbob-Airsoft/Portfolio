using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
///
/// Drag a world-space object in world space, along a set axis
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
/// 
public class DragObject : MonoBehaviour
{

    private Camera main;
    private Vector3 anchorPosition;

    [SerializeField] private LayerMask boundsLayer;

    private float xBounds, yBounds;

    private int deltaIndex = 0;
    private Vector3[] mouseDeltas = new Vector3[4];
    private Vector3 mouseDelta;
    private Vector3 prevMouse;

    private Vector3 prevMouseWorldPosition;

    private bool canDrag;
    
    
    
    // DEBUG
    [SerializeField] private TextMeshProUGUI deltaText;
    
    private enum Orientation
    {
        RIGHT,
        UP
    }

    [SerializeField] private Orientation orientation;
    
    
    
    // DEBUG DRAWING
    private bool drawShere;
    private Vector3 rayHitPosition = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
        anchorPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        // calculate mouse Delta position per 4 frame rolling average
        // opportunity to move to a manager script for performance
        // but this shouldn't be an issue
        mouseDeltas[deltaIndex] = Input.mousePosition - prevMouse;

        prevMouse = Input.mousePosition;


        mouseDelta = Vector3.zero;
        for (int i = 0; i < mouseDeltas.Length; i++)
        {
            mouseDelta += mouseDeltas[i];
        }
        
        if (deltaText)
            deltaText.text = mouseDelta.ToString();

        // Clamp Mouse Delta to 1 and -1
        if (mouseDelta.x > 0f)
            mouseDelta.x = 1f;
        else if (mouseDelta.x < -0f)
            mouseDelta.x = -1f;
        else
            mouseDelta.x = 0;

        if (mouseDelta.y > 0f)
            mouseDelta.y = 1f;
        else if (mouseDelta.y < -0f)
            mouseDelta.y = -1f;
        else
            mouseDelta.y = 0;


        deltaIndex++;
        if (deltaIndex >= mouseDeltas.Length) deltaIndex = 0;
    }

    private void OnMouseDrag()
    {
        if (canDrag)
        {
            CalculateStop();

            // #### Get mouse position ####

            Vector3 mousePos = Input.mousePosition;

            mousePos.z =
                main.WorldToScreenPoint(transform.position).z; // gets the z position of the object in screen space

            // convert our mouse position (x, y) and use the z position of object
            // to generate a world position
            Vector3 newLocation = main.ScreenToWorldPoint(mousePos);
            newLocation.z = transform.position.z;

            // #### Calculate Movement ####

            if (prevMouseWorldPosition != Vector3.zero)
            {
                // Translate our world position to a local position relative to our parent object
                

                // Calculate movement of mouse in world space
                Vector3 movement = newLocation - prevMouseWorldPosition;
                
                // change movement based on orientation
                if (orientation == Orientation.UP)
                {
                    movement.x = 0;
                }
                else if (orientation == Orientation.RIGHT)
                {
                    movement.y = 0;
                }
                
                // #### Update our location ####

                // move the object
                Vector3 newPosition = transform.localPosition + movement;
                // reset z position
                newPosition.z = anchorPosition.z;
                Debug.Log(newPosition);
                
                // apply movement
                transform.localPosition = newPosition;
                
                ClampPosition();

                //transform.position = newLocation;



                //transform.localPosition = newLocalPosition;
            }

            prevMouseWorldPosition = newLocation;
        }
    }

    private void OnMouseExit() { canDrag = false; prevMouseWorldPosition = Vector3.zero; }
    private void OnMouseEnter() { canDrag = true; prevMouseWorldPosition = Vector3.zero; }

    void CalculateStop()
    {
        // cast a ray depending on orientation

        if (orientation == Orientation.RIGHT)
        {
            // cast a horizontal ray the direcition the mouse is moving
            Vector3 direction = transform.right * mouseDelta.x;

            
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(transform.position, direction, out hitInfo, Mathf.Infinity, boundsLayer))
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                xBounds = hitInfo.transform.position.x;

                // Debug
                rayHitPosition = hitInfo.transform.position;
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
            }
        }
        
        if (orientation == Orientation.UP)
        {
            // cast a horizontal ray the direcition the mouse is moving
            Vector3 direction = transform.up * mouseDelta.y;

            
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(transform.position, direction, out hitInfo, Mathf.Infinity, boundsLayer))
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                yBounds = hitInfo.transform.position.y;
                
                // Debug
                rayHitPosition = hitInfo.transform.position;
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
            }
        }
    }
    
    void ClampPosition()
    {
        //clamping
        if (orientation == Orientation.RIGHT)
        {
            switch (mouseDelta.x)
            {
                case 1:
                    Debug.Log(transform.position.x + ">" + xBounds);
                    if (transform.position.x > xBounds)
                    {
                        transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
                        Debug.Log("Moved to " + transform.position);
                    }

                    break;

                case -1:
                    Debug.Log(transform.position.x + "<" + xBounds);
                    if (transform.position.x < xBounds)
                    {
                        transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
                        Debug.Log("Moved to " + transform.position);
                    }

                    break;
            }
        }

        if (orientation == Orientation.UP)
        {
            switch (mouseDelta.y)
            {
                case 1:
                    if (transform.position.y > yBounds)
                    {
                        transform.position = new Vector3(transform.position.x, yBounds, transform.position.z);
                    }

                    break;

                case -1:
                    if (transform.position.y < yBounds)
                    {
                        transform.position = new Vector3(transform.position.x, yBounds, transform.position.z);
                    }

                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rayHitPosition, 0.3f);
    }
}