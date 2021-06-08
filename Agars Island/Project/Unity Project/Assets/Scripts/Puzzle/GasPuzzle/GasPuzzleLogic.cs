using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RadiatorGames.Math;

/// <summary>
///
/// Controls the entire gas puzzle functionality
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class GasPuzzleLogic : MonoBehaviour
{
    [SerializeField] private Slider playerSlider;

    [SerializeField] private Slider gasFullSlider;
    private Image gasSliderBGImage;

    private float fillDelta = 0f;
    private float gasFillAmount = 0f;

    [SerializeField] private float pressureMax;
    [SerializeField] private float pressureSustainTime;
    private float timeAtPressureMax;

    public Transform gasArea;

    private bool dumpingPressure = false;
    public float pressureDumpTime = 0f;
    private float pressureDumpTimer = 0f;

    private ParticleSystem pressureParticle;

    // Start is called before the first frame update
    void Start()
    {
        gasSliderBGImage = gasFullSlider.GetComponentInChildren<Image>();
        pressureParticle = transform.parent.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dumpingPressure)
        {
            // check puzzle isn't complete
            if (gasFullSlider.value < gasFullSlider.maxValue)
            {
                // get player input
                fillDelta = playerSlider.value;

                // add gas to the cylinder
                gasFillAmount += fillDelta * Time.deltaTime;

                gasFullSlider.value = gasFillAmount;

                // if we're filling too quickly
                if (fillDelta >= pressureMax)
                {
                    timeAtPressureMax = Mathf.Clamp(timeAtPressureMax + Time.deltaTime, 0, pressureSustainTime);

                    // we have filled too quickly for too long
                    if (timeAtPressureMax >= pressureSustainTime)
                    {
                        // dump
                        gasFillAmount = 0;
                        pressureDumpTimer = pressureDumpTime;
                        dumpingPressure = true;
                        pressureParticle.Play();
                        return; // return prevents the player from failing the puzzle but completing it too. Very minor case
                    }
                }
                else
                {
                    timeAtPressureMax = Mathf.Clamp(timeAtPressureMax - Time.deltaTime, 0, pressureSustainTime);
                }

                // visual feedback of pressure overload
                float R = Mapping.Map(0, pressureSustainTime, 0, 1, timeAtPressureMax);
                float A = Mapping.Map(0, pressureSustainTime, 0, 1, timeAtPressureMax);

                gasSliderBGImage.color = new Color(R, 0, 0, A);
            }
            else
            {
                // puzzle is complete!
                gasArea.GetComponent<EditParticleScale>().TurnOff();
            }
        }
        else
        {
            // allowing the pressure to even out
            pressureDumpTimer -= Time.deltaTime;

            if (pressureDumpTimer <= 0)
            {
                dumpingPressure = false;
                timeAtPressureMax = 0;
                playerSlider.value = 0;
            }
        }
    }
}