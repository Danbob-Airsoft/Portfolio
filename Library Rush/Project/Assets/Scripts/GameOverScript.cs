using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    //Timer Variables
    public float TimeLimit;
    private float GameOverTime;

    //Complaint Variables
    public float MaxComplaints;
    private float CurrentComplaints;

    //UI Variables
    public CanvasGroup GameOverCanvas;
    public CanvasGroup HUDCanvas;
    private GameObject Player;
    public Image TimeImage;
    public TMPro.TextMeshProUGUI GameOverTextBox;

    private bool FailedTriggered;
    private AudioSource ThisSource;
    public AudioClip FailedClip;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        GameOverTime = Time.timeSinceLevelLoad + TimeLimit;
        TimeImage = Player.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<Image>();
        ThisSource = this.GetComponent<AudioSource>();
        FailedTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddComplaint();
        }
        //Check if Player has run out of time Or too many complaints
        if (Time.timeSinceLevelLoad >= GameOverTime && FailedTriggered == false)
        {
            GameOverTextBox.text = "B.R.A.V.A! You haven't put all the books back and it's closing time! Looks like your programming needs tweaking!";
            FailedTriggered = true;
            TriggerGameOver();
        }
        //Else update Time Remainig
        else
        {
            float FillAmount = Time.timeSinceLevelLoad / TimeLimit;
            TimeImage.fillAmount = FillAmount;
        }
    }

    public void AddComplaint()
    {
        //Add Complaint
        CurrentComplaints += 1;
        //Check if max complaints reached
        if(CurrentComplaints == MaxComplaints)
        {
            GameOverTextBox.text = "B.R.A.V.A! Too many guests have complained that you haven't been helping them! Looks like your programming needs tweaking!";
            TriggerGameOver();
        }
        else
        {
            Player.GetComponent<PlayerInventory>().UpdateCompaints(CurrentComplaints, MaxComplaints);
        }
    }

    private void TriggerGameOver()
    {
        ThisSource.PlayOneShot(FailedClip);

        //Activate Game Over Screen
        GameOverCanvas.alpha = 1;
        GameOverCanvas.interactable = true;
        GameOverCanvas.blocksRaycasts = true;

        //Hide HUD
        HUDCanvas.alpha = 0;
        HUDCanvas.interactable = false;
        HUDCanvas.blocksRaycasts = false;

        //Freeze Time
        Time.timeScale = 0;
    }
}
