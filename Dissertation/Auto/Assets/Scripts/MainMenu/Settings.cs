using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public AudioMixer GameMixer;
    public AudioMixer MusicMixer;
    public Slider SensitivitySlider;
    public Slider MusicSlider;
    public Slider GameSlider;
    public TMP_Dropdown ResolutionDropDown;

    public Resolution[] Resolutions;
    private int DefaultIndex;

    public AudioSource MenuSource;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 3f);
        }
        SensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");

        float Volume;
        MusicMixer.GetFloat("MusicVolume", out Volume);
        MusicSlider.value = Volume;
        GameMixer.GetFloat("GameVolume", out Volume);
        GameSlider.value = Volume;

        Resolutions = Screen.resolutions;
        ResolutionDropDown.ClearOptions();

        int CurrentResIndex = 0;
        List<string> Options = new List<string>();
        for (int i = 0; i < Resolutions.Length; i++)
        {
            if (!Options.Contains(Resolutions[i].width + " X " + Resolutions[i].height))
            {
                string Option = Resolutions[i].width + " X " + Resolutions[i].height;
                Options.Add(Option);
            }

            if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResIndex = i;
                DefaultIndex = i;
            }
        }

        ResolutionDropDown.AddOptions(Options);
        ResolutionDropDown.value = CurrentResIndex;


    }

    public void SetGameVolume(float Volume)
    {
        GameMixer.SetFloat("GameVolume", Volume);
    }

    public void SetMusicVolume(float Volume)
    {
        MusicMixer.SetFloat("MusicVolume", Volume);
    }

    public void QualitySet(int qualityIndex)
    {
        //Play Sound
        PlaySound();

        //Set Quality
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        //Play Sound
        PlaySound();

        Screen.fullScreen = isFullScreen;
    }

    public void SetSensitivity(float Sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
        PlayerPrefs.Save();
        if(this.transform.root.tag == "Player")
        {
            this.gameObject.transform.root.GetComponent<PlayerMovement>().ChangeSensitivity(Sensitivity);
        }
    }

    public void SetRes(int ResIndex)
    {
        //Play Sound
        PlaySound();

        //Set Resolution
        Resolution res = Resolutions[ResIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        ResolutionDropDown.RefreshShownValue();
    }

    public void ResetAudio()
    {
        //Play Sound
        PlaySound();

        //Reset Sliders
        GameSlider.value = 0;
        MusicSlider.value = 0;
    }

    public void ResetSensitivity()
    {
        //Play Sound
        PlaySound();

        SensitivitySlider.value = 3;
    }

    public void LoadNextMission()
    {
        if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void PlaySound()
    {
        MenuSource.Play();
    }
}
