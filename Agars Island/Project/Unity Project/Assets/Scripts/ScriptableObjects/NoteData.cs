using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
///
/// SO for NoteData
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
/// 
[CreateAssetMenu(fileName = "NoteData", menuName = "UrbanLizard/NoteData", order = 1)]
public class NoteData : ScriptableObject
{
    public string title;

    public string description;
}
