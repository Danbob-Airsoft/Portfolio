using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
///
/// Custom inspector giving functionality for ToggleFloor
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
/// 
[CustomEditor(typeof(ToggleFloor))]
public class ToggleFloorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ToggleFloor toggleFloor = (ToggleFloor) target;
        
        if (GUILayout.Button("Toggle floor"))
        {
            
            foreach (GameObject gameObject in toggleFloor.FloorObjects)
            {
                gameObject.SetActive(toggleFloor.showing);
            }

            toggleFloor.showing = !toggleFloor.showing;
        }
        
        base.OnInspectorGUI();
    }
}
