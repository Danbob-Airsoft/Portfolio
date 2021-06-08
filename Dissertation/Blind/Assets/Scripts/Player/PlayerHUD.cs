using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayerHUD : MonoBehaviour
{
    //--------------------------Camera and HUD------------------------------
    public Camera FrontCam;
    public Camera BackCam;
    public CanvasGroup HUDCanvas;

    private PlayerFiring FireScript;

    //---------------------------Health Bars--------------------------------
    private PlayerHealth HealthScript;
    private float Shields;
    private float Hull;
    private float Ammo;
    public Image ShieldBar;
    public Image HealthBar;
    public Image AmmoBar;

    //---------------------------Pause Menu--------------------------------
    public Canvas MainCanvas;
    public CanvasGroup PauseMenu;
    public CanvasGroup SettingsCanvas;
    public CanvasGroup ControlsCanvas;
    
    //Other
    private PlayerMovement MovementScript;
    public Text PowerupText;
    public float PowerupNotTime;
    public Image HitMarker;
    private Canvas MarkerCanvas;
    public AudioSource MenuSource;


    // Start is called before the first frame update
    void Start()
    {
        BackCam.enabled = false;
        MovementScript = this.GetComponent<PlayerMovement>();
        MovementScript.ActiveCamera = FrontCam;
        HealthScript = this.GetComponent<PlayerHealth>();
        FireScript = this.GetComponent<PlayerFiring>();
        FireScript.CurrentCamera = FrontCam;
        MarkerCanvas = this.transform.GetChild(5).GetComponent<Canvas>();

        //Disable Pause Menu
        PauseMenu.alpha = 0;
        PauseMenu.interactable = false;
        PauseMenu.blocksRaycasts = false;
        SettingsCanvas.alpha = 0;
        SettingsCanvas.interactable = false;
        SettingsCanvas.blocksRaycasts = false;
        HitMarker.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //-----------------------------Change Camera-----------------------------
        if (KeyBindingManager.GetKeyDown(KeyAction.ChangeView))
        {
            if (FrontCam.enabled)
            {
                BackCam.enabled = true;
                //BackCanvas.enabled = true;
                FrontCam.enabled = false;
                MainCanvas.worldCamera = BackCam;
                MovementScript.ActiveCamera = BackCam;
                FireScript.CurrentCamera = BackCam;
            }

            else
            {
                BackCam.enabled = false;
                //BackCanvas.enabled = false;
                FrontCam.enabled = true;
                MovementScript.ActiveCamera = FrontCam;
                MainCanvas.worldCamera = FrontCam;
                FireScript.CurrentCamera = FrontCam;
            }
        }

        //---------------------------Pause Screen----------------------
        if (KeyBindingManager.GetKeyDown(KeyAction.Pause))
        {
            if (Time.timeScale == 1)
            {
                //Pause
                Cursor.visible = true;
                //Stop Time
                Time.timeScale = 0;
                MarkerCanvas.enabled = false;
                //Set Camera to First Person
                FrontCam.enabled = true;
                BackCam.enabled = false;
                MovementScript.ActiveCamera = FrontCam;

                //Disable HUD?
                HUDCanvas.alpha = 0;
                HUDCanvas.interactable = false;
                HUDCanvas.blocksRaycasts = false;

                //Enable Pause Menu
                PauseMenu.alpha = 1;
                PauseMenu.interactable = true;
                PauseMenu.blocksRaycasts = true;
                MainCanvas.worldCamera = FrontCam;

                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Resume();
            }
        }

        //Powerup Text
        if(PowerupText.enabled == true && Time.time >= PowerupNotTime)
        {
            PowerupText.text = "";
        }

        //---------------------------Updating HUD-------------------
        Shields = HealthScript.Shields;
        Hull = HealthScript.Hull;
        Ammo = FireScript.Ammo;
        ShieldBar.fillAmount = (Shields / 100);
        HealthBar.fillAmount = (Hull / 100);
        AmmoBar.fillAmount = (Ammo / 50);
    }


    //---------------------------------------Pause Screen Buttons---------------------------
    public void Resume()
    {
        if(HealthScript.Hull != 0)
        {
            PlaySound();
            Cursor.visible = false;
            //Start Time
            Time.timeScale = 1;

            //Enable HUD
            HUDCanvas.alpha = 1;
            HUDCanvas.interactable = true;
            HUDCanvas.blocksRaycasts = true;

            //Disable Pause Menu
            PauseMenu.alpha = 0;
            PauseMenu.interactable = false;
            PauseMenu.blocksRaycasts = false;

            SettingsCanvas.alpha = 0;
            SettingsCanvas.interactable = false;
            SettingsCanvas.blocksRaycasts = false;

            ControlsCanvas.alpha = 0;
            ControlsCanvas.interactable = false;
            ControlsCanvas.blocksRaycasts = false;

            Cursor.lockState = CursorLockMode.Locked;
            MarkerCanvas.enabled = true;
        }
    }

    public void ReturnToMenu()
    {
        PlaySound();
        //Load Main Menu
        SceneManager.LoadScene(0);
        //Start Time
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        PlaySound();
        //Open Settings Canvas
        SettingsCanvas.alpha = 1;
        SettingsCanvas.interactable = true;
        SettingsCanvas.blocksRaycasts = true;
        //Close Pause Canvas
        PauseMenu.alpha = 0;
        PauseMenu.interactable = false;
        PauseMenu.blocksRaycasts = false;
    }

    public void OpenControls()
    {
        PlaySound();
        ControlsCanvas.alpha = 1;
        ControlsCanvas.interactable = true;
        ControlsCanvas.blocksRaycasts = true;
        //Close Pause Canvas
        PauseMenu.alpha = 0;
        PauseMenu.interactable = false;
        PauseMenu.blocksRaycasts = false;
    }

    public void BackToPause()
    {
        PlaySound();
        //Hide Settings Canvas
        SettingsCanvas.alpha = 0;
        SettingsCanvas.interactable = false;
        SettingsCanvas.blocksRaycasts = false;

        ControlsCanvas.alpha = 0;
        ControlsCanvas.interactable = false;
        ControlsCanvas.blocksRaycasts = false;
        //Show Pause
        PauseMenu.alpha = 1;
        PauseMenu.interactable = true;
        PauseMenu.blocksRaycasts = true;
    }

    public void RestartLevel()
    {
        PlaySound();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisplayHitMarker()
    {
        if (!HitMarker.enabled)
        {
            StartCoroutine(WaitToHide());
        }
    }

    private IEnumerator WaitToHide()
    {
        HitMarker.enabled = true;
        yield return new WaitForSeconds(0.1f);
        HitMarker.enabled = false;
    }

    private void PlaySound()
    {
        MenuSource.Play();
    }
}
