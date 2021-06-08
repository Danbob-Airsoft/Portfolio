using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///
/// Edits a particle system in a child both at edit time and runtime
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>

[ExecuteAlways]
public class EditParticleScale : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private float emmissionRate;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            if (particleSystem)
            {
                var emission = particleSystem.emission;
                emission.rateOverTime = transform.localScale.x * 10;


                var shape = particleSystem.shape;
                shape.scale = transform.localScale;
            }
            else
            {
                try
                {
                    particleSystem = GetComponentInChildren<ParticleSystem>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }

    public void TurnOff()
    {
        var emission = particleSystem.emission;
        emission.rateOverTime = 0;

        particleSystem = null;
    }
}
