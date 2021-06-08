using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Toggles the flashlight
/// Also houses code for reloading and checking battery
///
/// Created by: Daniel Bailey
/// Edited by: Lewis Arnold, Luke Baldwin
/// </summary>
public class ToggleFlashlight : MonoBehaviour
{
    [Header("Required Variables")]
    //Flashlight script
    private Flashlight_PRO LightScript;
    //Enable/ disable toggling of light
    [HideInInspector] public bool canToggle = true;
    //Current battery
    public float Battery;
    //Max battery
    [SerializeField] public float maxBattery = 100f;
    //Speed light drains at when on
    public float DrainSpeed;
    //Inventory script
    private InventoryTracker Inventory;
    //Does the player have flashlight
    [HideInInspector] public bool hasFlashlight = false;
    //Is the player inspecting the light
    private bool Inspecting;
    //Animation for inspecting the flashlight
    private Animator flashLightAnim;

    // Start is called before the first frame update
    void Start()
    {
        //Get scripts for flashlight
        LightScript = this.GetComponentInChildren<Flashlight_PRO>();
        flashLightAnim = LightScript.gameObject.GetComponent<Animator>();
        //Get inventory component
        Inventory = this.GetComponent<InventoryTracker>();
        //Set toggle and inspect bools
        canToggle = true;
        Inspecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If the light is on
        if (LightScript.is_enabled)
        {
            //Drain Flashlight
            Battery -= DrainSpeed * Time.deltaTime;
            //If battery is empty
            if(Battery <= 0)
            {
                //Switch light off
                LightScript.Switch();
                //Set to can't toggle
                canToggle = false;
            }
        }

        if (hasFlashlight)
        {
            //Toggle Light on and off
            if (Input.GetMouseButtonDown(0) && canToggle)
            {
                LightScript.Switch();
            }

            //Reload battery
            if (Input.GetKeyDown(KeyCode.R) && Inventory.batteries > 0)
            {
                Inventory.RemoveBattery(1);
                Battery = maxBattery;
                canToggle = true;
                flashLightAnim.SetTrigger("ReloadBattery");
            }

            //Inspect Battery Toggle
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (!Inspecting)
                {
                    //Play inspection animation
                    flashLightAnim.SetTrigger("CheckBattery");
                }
                else
                {
                    //Play reverse animation
                    flashLightAnim.SetTrigger("HolsterTorch");
                }

                //Set bool to new value
                Inspecting = !Inspecting;
            }
        }
    }
}
