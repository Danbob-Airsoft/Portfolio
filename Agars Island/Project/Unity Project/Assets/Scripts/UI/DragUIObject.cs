using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///
/// Allows a UI object to be dragged on the screen
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
/// 
[RequireComponent(typeof(RectTransform))]
public class DragUIObject : MonoBehaviour, IDragHandler
{

    public bool lockHorizontal;
    public bool lockVertical;
    
    private RectTransform rectTransform;

    private RectTransform moveableArea;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    public bool debugStatements;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        
        if (debugStatements)
        {
            Debug.Log(col.size.ToString() + " : " + rectTransform.sizeDelta.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.anchoredPosition += new Vector2(eventData.delta.x * (lockHorizontal == true ? 0 : 1), eventData.delta.y * (lockVertical == true ? 0 : 1));

        //rb.velocity = eventData.delta;
        
        //rb.MovePosition(rectTransform.anchoredPosition + new Vector2(eventData.delta.x * (lockHorizontal == true ? 0 : 1), eventData.delta.y * (lockVertical == true ? 0 : 1)));
    }
}