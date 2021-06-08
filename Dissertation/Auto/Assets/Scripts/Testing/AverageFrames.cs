﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class AverageFrames : MonoBehaviour
{

    int FramesPassed = 0;
    float FPSTotal = 0f;

    private float TestEndTime;

    private void Start()
    {
        TestEndTime = Time.time + 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < TestEndTime)
        {
            ClearLog();
            float FPS = 1 / Time.unscaledDeltaTime;

            FPSTotal += FPS;
            FramesPassed++;
            Debug.Log("Average Frames: " + (FPSTotal / FramesPassed));
        }
    }

    public void ClearLog() //you can copy/paste this code to the bottom of your script
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
