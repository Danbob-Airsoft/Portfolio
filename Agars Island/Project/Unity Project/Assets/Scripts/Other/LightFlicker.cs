using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Flickers a light
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    private Light light;

    [Range(0, 10)] [SerializeField] private int threshold;

    [SerializeField] private float changeTime;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > changeTime)
        {
            int randoCalrissian = Random.Range(0, 10);

            if (randoCalrissian > threshold)
            {
                light.enabled = true;
            }
            else
            {
                light.enabled = false;
            }

            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
