using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
///
/// Controller for the main menu
/// 
/// Created by: Daniel Bailey
/// Edited by:
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Menu Canvas's")]
    public CanvasGroup MainCanvas;
    public CanvasGroup ControlsCanvas;
    public CanvasGroup CreditsCanvas;

    //Load Game scene
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    //Close Game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Open Controls Canvas
    public void ToControls()
    {
        ToggleCanvas(MainCanvas, false);
        ToggleCanvas(ControlsCanvas, true);
    }

    //Open Credits Canvas
    public void ToCredits()
    {
        ToggleCanvas(MainCanvas, false);
        ToggleCanvas(CreditsCanvas, true);
    }

    //Open Main Canvas
    public void BackToMenu()
    {
        ToggleCanvas(MainCanvas, true);
        ToggleCanvas(ControlsCanvas, false);
        ToggleCanvas(CreditsCanvas, false);
    }

    //Toggle canvas to given bool
    private void ToggleCanvas(CanvasGroup ToggleGroup, bool ToggleTo)
    {
        ToggleGroup.interactable = ToggleTo;
        ToggleGroup.blocksRaycasts = ToggleTo;
        if(ToggleTo == true)
        {
            ToggleGroup.alpha = 1;
        }
        else
        {
            ToggleGroup.alpha = 0;
        }
    }
}
