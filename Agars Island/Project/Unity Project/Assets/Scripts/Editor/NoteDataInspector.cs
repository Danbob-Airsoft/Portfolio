using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
///
/// Custom Editor for NoteData
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>    
    
[CustomEditor(typeof(NoteData))]
public class NoteDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        NoteData _target = (NoteData) target;
        EditorUtility.SetDirty(_target);
        
        _target.title = GUILayout.TextField(_target.title);
        _target.description = GUILayout.TextArea(_target.description);

        //base.OnInspectorGUI();
    }
}
