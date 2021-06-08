using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
///
/// Button script for toggling the morse code audio
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class MorseCodeSwitch : Switch
{
    [SerializeField] private AudioSource morseAudio;
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        morseAudio.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Switched()
    {
        base.Switched();

        if (On)
            morseAudio.volume = 1;
        else
            morseAudio.volume = 0;
    }
}
