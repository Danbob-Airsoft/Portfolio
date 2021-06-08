using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///
/// Loads scenes
/// 
/// Created by: Lewis Arnold
/// Edited by: 
/// </summary>
public class GameEnd : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;

    private SceneLoader sceneLoader;
    // Start is called before the first frame update
    void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GameFadeOut()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindWithTag("Enemy").GetComponent<AIControl>().enabled = false;
        StartCoroutine(SceneFadeOut(fadeGroup, 4f));
    }

    private IEnumerator SceneFadeOut(CanvasGroup group, float time)
    {
        float elapsedTime = 0f;
        while (group.alpha < 1)
        {
            group.alpha = RadiatorGames.Math.Mapping.Map(0, time, 0, 1, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        sceneLoader.LoadScene(2);
    }
}
