using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

/// <summary>
///
/// Manager for the Morse Code puzzle. Calculates button press as dot or dash. Outputs this as
/// a TMPro object on a UI canvas
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class MorseCodeReader : MonoBehaviour
{
    private float timeDown;

    [SerializeField] private float dashTime = 0.2f;

    private string morseString = "";

    [SerializeField] private TextMeshProUGUI morseStringText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        timeDown = Time.time;
        Debug.Log("Mouse Down on button");
    }

    private void OnMouseUpAsButton()
    {
        float timeTaken = Time.time - timeDown;
        Debug.Log(timeTaken.ToString());

        if (timeTaken > dashTime)
        {
            morseString += "-";
            morseStringText.text = morseString;
            return;
        }

        
        morseString += "·";
        morseStringText.text = morseString;
    }

    public void ResetPuzzle()
    {
        morseString = "";
        morseStringText.text = morseString;
    }

    public void AddSpace()
    {
        morseString += " ";
        morseStringText.text = morseString;
    }
}
