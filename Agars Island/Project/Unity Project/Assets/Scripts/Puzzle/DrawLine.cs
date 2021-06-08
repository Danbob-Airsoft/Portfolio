using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
///
/// World-space drawing on a surface using the mouse
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
/// 
public class DrawLine : MonoBehaviour
{

    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;

    private List<Vector2> mousePositions = new List<Vector2>();

    private Camera main;

    public GameObject testObj;

    public float distanceBetweenPoints = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        // Get mouse position
        //Debug.Log("Dragging");
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = main.WorldToScreenPoint(transform.position).z; // gets the z position of the object in screen space
        
        // convert our mouse position (x, y) and use the z position of object in screen space
        // to generate a world position
        Vector3 newLocation = Camera.main.ScreenToWorldPoint(mousePos);
        
        CreateLine(newLocation);
    }

    private void OnMouseDrag()
    {
        // Get mouse position
        //Debug.Log("Dragging");
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = main.WorldToScreenPoint(transform.position).z; // gets the z position of the object in screen space
        
        // convert our mouse position (x, y) and use the z position of object in screen space
        // to generate a world position
        Vector3 newLocation = Camera.main.ScreenToWorldPoint(mousePos);

        UpdateLine(newLocation);
    }

    private void CreateLine(Vector3 location)
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
        lineRenderer = currentLine.GetComponent<LineRenderer>();

        location.y = transform.position.y;

        Vector3 locationLeft = location;
        locationLeft.z -= 0.01f;
        
        Vector3 locationRight = location;
        locationRight.z += 0.01f;

        lineRenderer.SetPosition(0, locationLeft);
        lineRenderer.SetPosition(1, locationRight);

        //GameObject obj = Instantiate(testObj, location, Quaternion.identity, transform);
        
        //obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
    }

    void UpdateLine(Vector3 location)
    {
        location.y = transform.position.y;

        if (Vector3.Distance(location, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) >
            distanceBetweenPoints)
        {

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, location);

            //GameObject obj = Instantiate(testObj, location, Quaternion.identity, transform);

            //obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
        }
    }
}
