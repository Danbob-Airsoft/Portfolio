using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{
    public CanvasGroup MainCanvas;
    public CanvasGroup LevelSelectCanvas;
    public CanvasGroup ControlsCanvas;
    public CanvasGroup SettingsCanvas;
    public CanvasGroup HUDCanvas;
    public CanvasGroup LevelIntroCanvas;

    public Button DeleteSaveButton;

    public List<Button> LevelButtons;

    private AudioSource ThisSource;
    public AudioClip ButtonClip;

    public AudioMixer ThisMixer;
    public Slider VolumeSlider;
    private bool CanPause;

    private void Start()
    {
        ThisSource = this.GetComponent<AudioSource>();

        //If in main menu
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            //Check if save data exists
            if (!PlayerPrefs.HasKey("LevelsOpen"))
            {
                //Disable delete save data button
                DeleteSaveButton.interactable = false;

                //Set Save Data to 1
                PlayerPrefs.SetInt("LevelsOpen", 1);
            }
            else
            {
                DeleteSaveButton.interactable = true;
            }
        }
        else
        {
            Time.timeScale = 0;
            ToggleCanvas(LevelIntroCanvas, true);
            ToggleCanvas(HUDCanvas, false);
            CanPause = false;
        }

        //Check if volume value exists
        if (!PlayerPrefs.HasKey("GameVolume"))
        {
            //If not, set to default
            PlayerPrefs.SetFloat("GameVolume", 0);
        }
        else
        {
            //Else set slider to value
            SetVolume(PlayerPrefs.GetFloat("GameVolume"));
            VolumeSlider.value = PlayerPrefs.GetFloat("GameVolume");
        }
    }

    public void StartLevel()
    {
        ToggleCanvas(LevelIntroCanvas, false);
        ToggleCanvas(HUDCanvas, true);
        CanPause = true;
        Time.timeScale = 1;
    }

    //Reload current scene
    public void RetryLevel()
    {
        PlaySound(ButtonClip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Load Main Menu Scene
    public void LoadMainMenu()
    {
        PlaySound(ButtonClip);
        SceneManager.LoadScene(0);
    }

    //Load next level scene
    public void LoadNextLevel()
    {
        PlaySound(ButtonClip);
        //Load Next level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Open Level Select Canvas
    public void LoadLevelSelect()
    {
        PlaySound(ButtonClip);
        ToggleCanvas(LevelSelectCanvas, true);
        ToggleCanvas(MainCanvas, false);
        //Enable buttons for number of levels unlocked
        for(int i = 0; i <= PlayerPrefs.GetInt("LevelsOpen") - 1; i++)
        {
            Debug.Log(i);
            LevelButtons[i].interactable = true;
        }
    }

    //Load specific level
    public void LoadSpecificLevel(int LevelToLoad)
    {
        PlaySound(ButtonClip);
        SceneManager.LoadScene(LevelToLoad);
    }

    //Open Controls Canvas
    public void LoadControls()
    {
        PlaySound(ButtonClip);
        ToggleCanvas(MainCanvas, false);
        ToggleCanvas(ControlsCanvas, true);
    }

    //Open Settings Canvas
    public void LoadSettings()
    {
        PlaySound(ButtonClip);
        ToggleCanvas(MainCanvas, false);
        ToggleCanvas(SettingsCanvas, true);
    }

    //Open Main Menu Canvas
    public void BackToMenu()
    {
        PlaySound(ButtonClip);
        ToggleCanvas(MainCanvas, true);
        ToggleCanvas(ControlsCanvas, false);
        ToggleCanvas(SettingsCanvas, false);
        ToggleCanvas(LevelSelectCanvas, false);
    }

    //Delete Save Data
    public void DeleteSaveData()
    {
        PlaySound(ButtonClip);
        //Disable delete save data button
        DeleteSaveButton.interactable = false;

        //Set Save Data to 1
        PlayerPrefs.SetInt("LevelsOpen", 1);

        for (int i = PlayerPrefs.GetInt("LevelsOpen"); i < LevelButtons.Count; i++)
        {
            Debug.Log(i);
            LevelButtons[i].interactable = false;
        }
    }

    //Toggle Specific Canvas
    public void ToggleCanvas(CanvasGroup ToToggle, bool ToggleTo)
    {
        if (ToggleTo)
        {
            ToToggle.alpha = 1;
            ToToggle.blocksRaycasts = true;
            ToToggle.interactable = true;
        }
        else
        {
            ToToggle.alpha = 0;
            ToToggle.blocksRaycasts = false;
            ToToggle.interactable = false;
        }
    }

    //Quit Game
    public void QuitGame()
    {
        PlaySound(ButtonClip);
        Application.Quit();
    }

    public void PauseGame()
    {
        if (CanPause)
        {
            if (Time.timeScale == 0)
            {
                //Hide Pause Menu
                ToggleCanvas(MainCanvas, false);

                //Hide Settings Menu
                ToggleCanvas(SettingsCanvas, false);

                //Hide Controls Menu
                ToggleCanvas(ControlsCanvas, false);

                //Enable HUD
                ToggleCanvas(HUDCanvas, true);

                //Set Timescale
                Time.timeScale = 1;
            }
            else
            {
                ToggleCanvas(HUDCanvas, false);

                ToggleCanvas(MainCanvas, true);

                //Set Timescale
                Time.timeScale = 0;
            }
        }
    }

    public void ReturnToPause()
    {
        ToggleCanvas(SettingsCanvas, false);

        ToggleCanvas(ControlsCanvas, false);

        ToggleCanvas(MainCanvas, true);

        PlaySound(ButtonClip);
    }

    public void SetVolume(float VolumeIn)
    {
        ThisMixer.SetFloat("Volume", VolumeIn);
        PlayerPrefs.SetFloat("GameVolume", VolumeIn);
    }

    public void SetFullScreen(bool ToggleTo)
    {
        Screen.fullScreen = ToggleTo;
        PlaySound(ButtonClip);
    }

    //Play sound
    private void PlaySound(AudioClip ToPlay)
    {
        ThisSource.PlayOneShot(ToPlay);
    }
}
