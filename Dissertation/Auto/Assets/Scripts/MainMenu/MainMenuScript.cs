using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuScript : MonoBehaviour
{
    public CanvasGroup MainScreen;
    public CanvasGroup ControlsScreen;
    public CanvasGroup CreditsScreen;
    public CanvasGroup SettingsScreen;
    public CanvasGroup LoadingScreen;
    public Slider Slider;
    public Text LoadingText;

    AsyncOperation Loading;

    public AudioSource MenuSource;

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 2);
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        //Play Sound
        PlaySound();

        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        Loading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Loading.allowSceneActivation = false;
        MainScreen.alpha = 0;
        MainScreen.blocksRaycasts = false;
        MainScreen.interactable = false;
        LoadingScreen.alpha = 1;
        LoadingScreen.blocksRaycasts = true;
        LoadingScreen.interactable = true;

        while(!Loading.isDone)
        {
            float Progress = Loading.progress / 0.9f;
            Slider.value = Progress;

            if(Loading.progress >= 0.9f)
            {
                LoadingText.text = "Loading Complete: Press Space to continue!";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Loading.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }


    public void OpenControlsScreen()
    {
        //Play Sound
        PlaySound();

        //Close Menu
        MainScreen.alpha = 0;
        MainScreen.blocksRaycasts = false;
        MainScreen.interactable = false;

        //Open Controls
        ControlsScreen.alpha = 1;
        ControlsScreen.blocksRaycasts = true;
        ControlsScreen.interactable = true;
    }

    public void OpenSettingsScreen()
    {
        //Play Sound
        PlaySound();

        MainScreen.alpha = 0;
        MainScreen.blocksRaycasts = false;
        MainScreen.interactable = false;

        SettingsScreen.alpha = 1;
        SettingsScreen.interactable = true;
        SettingsScreen.blocksRaycasts = true;
    }

    public void OpenCreditsScreen()
    {
        //Play Sound
        PlaySound();

        //Close Menu
        MainScreen.alpha = 0;
        MainScreen.blocksRaycasts = false;
        MainScreen.interactable = false;

        //Open Credits
        CreditsScreen.alpha = 1;
        CreditsScreen.blocksRaycasts = true;
        CreditsScreen.interactable = true;
    }

    public void BackToMainMenu()
    {
        //Play Sound
        PlaySound();

        //Open Menu
        MainScreen.alpha = 1;
        MainScreen.blocksRaycasts = true;
        MainScreen.interactable = true;

        //Close all other Canvas's
        ControlsScreen.alpha = 0;
        ControlsScreen.blocksRaycasts = false;
        ControlsScreen.interactable = false;

        CreditsScreen.alpha = 0;
        CreditsScreen.blocksRaycasts = false;
        CreditsScreen.interactable = false;

        SettingsScreen.alpha = 0;
        SettingsScreen.interactable = false;
        SettingsScreen.blocksRaycasts = false;
    }

    public void QuitGame()
    {
        //Play Sound
        PlaySound();

        Application.Quit();
    }

    private void PlaySound()
    {
        MenuSource.Play();
    }
}
