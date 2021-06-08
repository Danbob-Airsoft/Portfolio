using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///
/// Loads scenes
/// 
/// Created by: Lewis Arnold
/// Edited by: Daniel Bailey
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
