using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// Changes the text relating to the generator puzzle
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
/// 
public class ChangeGenText : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private List<ExplodableMonitor> monitors;
    
    public void GeneratorFixed()
    {
        text.text = "BackupGenerator: Ready";
        text.color = new Color(255f/255f,165f/255f,0, 255/255);
    }
    
    public void GeneratorOn()
    {
        if (CheckpointManager.instance.GeneratorFixed)
        {
            text.text = "BackupGenerator: On";
            text.color = Color.green;
            CheckpointManager.instance.ReleaseEnemy();
            
            foreach (var monitor in monitors)
            {
                monitor.TurnOn();
            }
        }
    }
}
