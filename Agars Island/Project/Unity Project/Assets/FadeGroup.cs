using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Loads scenes
/// 
/// Created by: Lewis Arnold
/// Edited by: 
/// </summary>
public class FadeGroup : MonoBehaviour
{
    [SerializeField] private CanvasGroup cGroup;

    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float fadeTime = 10f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SceneFadeOut(cGroup, fadeTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator SceneFadeOut(CanvasGroup group, float time)
    {
        yield return new WaitForSeconds(waitTime);
        float elapsedTime = 0f;
        while (group.alpha < 1)
        {
            group.alpha = RadiatorGames.Math.Mapping.Map(0, time, 0, 1, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
