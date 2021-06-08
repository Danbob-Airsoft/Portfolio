using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class used to be a checkpoint.
/// CheckPoint() can be called which
/// will trigger the events in triggerEvent;
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class GiveCheckPoint : MonoBehaviour
{
    // switch used to only trigger a checkpoint once
    private bool triggered = false;
    
    [Tooltip("UnityEvents to trigger when the checkpoint is triggered")]
    [SerializeField] private UnityEvent triggerEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Triggers triggerEvent events
    /// Will only invoke the events once
    /// no matter how many calls
    /// </summary>
    public virtual void CheckPoint()
    {
        if (triggered) return;
        
        triggered = true;
        triggerEvent.Invoke();
        Debug.Log("Checkpoint triggered");
    }
}
