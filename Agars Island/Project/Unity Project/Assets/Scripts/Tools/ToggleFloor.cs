using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Toggles a group of objects on/off
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class ToggleFloor : MonoBehaviour
{
    public List<GameObject> FloorObjects;
    [HideInInspector] public bool showing;
}
