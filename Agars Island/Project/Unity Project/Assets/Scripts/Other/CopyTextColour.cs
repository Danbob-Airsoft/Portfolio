using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///
/// Matches a light's colour to that of a text element
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>

public class CopyTextColour : MonoBehaviour
{
    [SerializeField] private Text textToCopy;

    private Light thisLight;
    // Start is called before the first frame update
    void Start()
    {
        thisLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        thisLight.color = textToCopy.color;
    }
}
