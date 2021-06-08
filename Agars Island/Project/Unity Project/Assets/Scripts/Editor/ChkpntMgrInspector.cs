using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
///
/// Custom Editor for the CheckpointManager
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>

#if UNITY_EDITOR
[CustomEditor(typeof(CheckpointManager))]
public class ChkpntMgrInspector : Editor
{
    private bool showEvents;

    private bool showFlashlightEvents;
    private bool showGeneratorFixEvents;
    private bool showGeneratorOnEvents;

    private bool cheatCodes;
    
    public override void OnInspectorGUI()
    {
        CheckpointManager manager = (CheckpointManager) target;

        showEvents = GUILayout.Toggle(showEvents, "Edit events");

        if (showEvents)
        {
            TogglableEventGUI(ref showFlashlightEvents, "FlashLightEvents");
            
            TogglableEventGUI(ref showGeneratorFixEvents, "GeneratorFixEvents");

            TogglableEventGUI(ref showGeneratorOnEvents, "GeneratorOnEvents");
        }

        cheatCodes = GUILayout.Toggle(cheatCodes, "ENABLE CHEAT CODE MODE WOOOAAHHH");

        if (cheatCodes)
        {
            if(GUILayout.Button("Flashlight"))
                manager.FlashLightEvents.Invoke();
            if (GUILayout.Button("GeneratorFix"))
            {
                manager.FlashLightEvents.Invoke();
                manager.GeneratorFixEvents.Invoke();
            }
            if (GUILayout.Button("GeneratorOn"))
            {
                manager.FlashLightEvents.Invoke();
                manager.GeneratorFixEvents.Invoke();
                manager.GeneratorOnEvents.Invoke();
            }
        }

        base.OnInspectorGUI();
    }

    private void TogglableEventGUI(ref bool toggle, string _event)
    {
        toggle = GUILayout.Toggle(toggle, _event);
        if (toggle)
            ShowEventGUI(_event);
    }
    
    private void ShowEventGUI(string _event)
    {
        SerializedProperty onCheck = serializedObject.FindProperty(_event); // <-- UnityEvent

        EditorGUILayout.PropertyField(onCheck);

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif