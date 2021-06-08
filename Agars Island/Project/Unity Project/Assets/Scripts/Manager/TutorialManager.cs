using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

/// <summary>
///
/// Controls the tutorial text fading based on different events
/// 
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class TutorialManager : MonoBehaviour
{
    private bool firstLoad = true; // change to flag from checkpoint manager
    
    private bool inventoryUsed = false;
    private bool inventoryClosed = false;
    private bool lookedAtBattery = false;
    private bool torchDown = false;
    private bool torchReloaded = false;

    [SerializeField] private Animator TABAnim;
    [SerializeField] private Animator TABCloseAnim;
    [SerializeField] private Animator EAnim;
    [SerializeField] private Animator SeeBatteryAnim;
    [SerializeField] private Animator TorchDownAnim;
    [SerializeField] private Animator ReloadBatteryAnim;

    private ToggleFlashlight flashLight;
    
    // Start is called before the first frame update
    void Start()
    {
        TABCloseAnim.Play("InventHidden");
        
        EAnim.Play("InventHidden");

        flashLight = GameObject.FindWithTag("Player").GetComponent<ToggleFlashlight>();

        if (firstLoad)
        {
            Thread.Sleep(1000);
            TABAnim.SetTrigger("FadeIn");
            firstLoad = false;
            Debug.Log("Tut started");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inventoryUsed)
            {
                TABAnim.SetTrigger("FadeOut");
                
                TABCloseAnim.SetTrigger("FadeIn");
                inventoryUsed = true;
                return;
            }

            if (!inventoryClosed)
            {
                TABCloseAnim.SetTrigger("FadeOut");
                
                EAnim.SetTrigger("FadeIn");
                inventoryClosed = true;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(inventoryClosed)
                EAnim.SetTrigger("FadeOut");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (flashLight.hasFlashlight && !torchDown)
            {
                if (!lookedAtBattery)
                {
                    SeeBatteryAnim.SetTrigger("FadeOut");
                    TorchDownAnim.SetTrigger("FadeIn");
                    lookedAtBattery = true;
                }
                else
                {
                    TorchDownAnim.SetTrigger("FadeOut");
                    ReloadBatteryAnim.SetTrigger("FadeIn");
                    torchReloaded = true;
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (torchReloaded)
            {
                ReloadBatteryAnim.SetTrigger("FadeOut");
            }
        }
    }

    public void StartFlashLighTut()
    {
        SeeBatteryAnim.SetTrigger("FadeIn");
    }
}
