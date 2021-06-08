using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

/// <summary>
///
/// Moves the wire in world-space along the axis it is in already
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class MoveWire : MonoBehaviour
{
    private Camera main;

    private Vector3 startPoint;

    public Transform wireBox;


    private float originalSize;
    
    private Vector3 wireBoxScale;

    public Transform wireBegin;

    public GameObject wireEnd;

    public LayerMask wireEndLayer;

    private BoxCollider collider;
    private Vector3 originalColliderSize;

    private bool wireComplete;

    [SerializeField] private ShortWiresPuzzleManager manager;
    [SerializeField] private int wireNumber;
    
    // Feedback
    [SerializeField] private AudioSource electricity_noise;
    
    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;

        startPoint = transform.position;

        wireBoxScale = wireBox.localScale;

        originalSize = wireBox.GetComponent<Renderer>().bounds.size.z; // this is the size of the object's DEPTH (what looks to be its length from the player's POV)

        collider = GetComponent<BoxCollider>();

        originalColliderSize = collider.size;

        manager = transform.parent.parent.GetComponent<ShortWiresPuzzleManager>();

        if (!electricity_noise)
            electricity_noise = GetComponent<AudioSource>();
        if (!electricity_noise)
            Debug.LogWarning("No audio source on short wires movable");
    }

    // Update is called once per frame
    void Update()
    {
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

        //Debug.Log(newLocation);

        // update our location
        SetPosition(newLocation);

        collider.size = originalColliderSize * 2;
        
        // turn on feedback
        electricity_noise.volume = 1;
    }

    private void OnMouseUpAsButton()
    {
        wireComplete = false;
        //RaycastHit hitEnd;
        //bool atEnd = Physics.SphereCast(transform.position, 1f, transform.up, out hitEnd, 1f, wireEndLayer);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.1f, transform.forward, 0.1f, wireEndLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.transform.name);

            if (hits[i].collider.gameObject == wireEnd)
            {
                Debug.Log("Wire complete!");
                wireComplete = true;
                SetPosition(wireEnd.transform.position);
                manager.CompleteWire(wireNumber);
            }
        }
        
        if (hits.Length == 0 || wireComplete == false)
        {
            SetPosition(startPoint);
            manager.FailWire(wireNumber);
        }

        collider.size = originalColliderSize;
        
        // turn off feedback
        electricity_noise.volume = 0;
    }

    float getScaleFromSize(float newSize)
    {
        return (newSize / originalSize) * wireBoxScale.x;
    }

    private void OnDrawGizmos()
    {
        Bounds bounds = wireBox.GetComponent<Renderer>().bounds;

        //Debug.LogWarning(bounds.size.y.ToString());

        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }

    private void SetPosition(Vector3 position)
    {
        //move to new position
        transform.position = position;
        
        // set out local Z position to 0, so we are still flush with the box behind
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

        position = transform.position;
        
        // Calculate rotation
        Vector3 direction = position - wireBegin.position;
        transform.right = direction;

        
        // Make the wire as long as it needs to be
        float dist = direction.magnitude;
        
        Vector3 rescale = wireBox.localScale;
        rescale.x = getScaleFromSize(dist);
        

        wireBox.localScale = rescale;
    }
}
