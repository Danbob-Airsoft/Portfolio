using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Used to allow certain cameras to see sunlight.
/// Use-case in player not needing bright ambient light for atmosphere
/// but security cameras being too dark to see so more ambient light required
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class CullLights : MonoBehaviour
{

    [SerializeField] private GameObject sun;
    [SerializeField] private List<GameObject> lightsToCull;
    
    void OnPreCull () {
        if (sun != null)
            sun.SetActive(false);
    }

    void OnPreRender() {
        if (sun != null)
            sun.SetActive(false);
    }
    void OnPostRender() {
        if (sun != null)
            sun.SetActive(true);
    }
}
