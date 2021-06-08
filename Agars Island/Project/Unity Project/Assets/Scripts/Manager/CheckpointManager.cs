using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// Manages the checkpoints in the game.
/// Responsible for setting flags and
/// controlling the objectives seen on screen.
///
/// Created by: Lewis Arnold
/// Edited by: Daniel Bailey
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    [HideInInspector] public static CheckpointManager instance;

    [HideInInspector] public bool GeneratorFixed;
    [HideInInspector] public bool GeneratorOn;
    [HideInInspector] public bool doorLockedTrigger = false;

    [SerializeField] private GameObject enemy;
    [SerializeField] private TextMeshProUGUI taskText;
    
    [SerializeField] private TextMeshProUGUI taskCompText;
    private Animator taskCompAnim;
    
    [SerializeField] private TextMeshProUGUI newTaskText;
    private Animator newTaskAnim;
    
    [SerializeField] private GameObject Player;

    [HideInInspector] public UnityEvent FlashLightEvents;
    [HideInInspector] public UnityEvent GeneratorFixEvents;
    [HideInInspector] public UnityEvent GeneratorOnEvents;
    
    // INIT
    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        taskText.text = "☐ Find a light source";
        taskCompAnim = taskCompText.gameObject.GetComponent<Animator>();

        newTaskAnim = newTaskText.gameObject.GetComponent<Animator>();
        
        Player = GameObject.FindGameObjectWithTag("Player");
        doorLockedTrigger = false;

        newTaskText.text = "Find a light source";
        newTaskAnim.SetTrigger("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// 
    /// Following is a series of public functions, called from different
    /// GameObjects to indicate that a checkpoint has been reached
    /// 

    public void FlashlightGot()
    {
        taskCompText.text = "|COMPLETE| Find a light source";
        taskCompAnim.SetTrigger("FadeIn");
        
        taskText.text = "☐ Fix the backup generator";
        newTaskText.text = "Fix the backup generator";
        newTaskAnim.SetTrigger("FadeIn");
    }

    public void DoorLocked()
    {
        doorLockedTrigger = true;
        
        taskText.text = "☐ Search for a keycard to grant access to the rest of the facility";
        newTaskText.text = "Search for a keycard to grant access to the rest of the facility";
        newTaskAnim.SetTrigger("FadeIn");
    }

    public void FoundKeycard()
    {
        taskCompText.text = "|COMPLETE| Found a keycard";
        
        taskCompAnim.SetTrigger("FadeIn");
        
        taskText.text = "☐ Fix the backup generator";
        newTaskText.text = "Fix the backup generator";
        newTaskAnim.SetTrigger("FadeIn");
    }

    public void ReleaseEnemy()
    {
        taskCompText.text = "|COMPLETE| Fix the backup generator";
        taskCompAnim.SetTrigger("FadeIn");
        
        taskText.text = "☐ Get back to the lobby";
        
        newTaskText.text = "Get back to the lobby";
        newTaskAnim.SetTrigger("FadeIn");
        
        
        enemy.SetActive(true);
        GeneratorOn = true;
        Player.GetComponent<PlayerSanity>().enabled = true;
    }

    public void PlayPhone()
    {
        taskText.text = "☐ Pick up the phone";
        
        newTaskText.text = "Pick up the phone";
        newTaskAnim.SetTrigger("FadeIn");
    }
}
