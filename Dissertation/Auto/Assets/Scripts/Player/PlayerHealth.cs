using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    //Setup Health and Shields
    public float Shields;
    public float Hull;
    private float TimeTakenDamage;
    public float RechargeWait;
    public float RechargeDelay;
    private bool CanRecharge;

    //Sound Setup
    public AudioSource DamageSFXSource;
    public AudioSource ShieldSource;

    public AudioClip ShieldDamageSFX;
    public AudioClip HullDamageSFX;

    public AudioClip ShieldOutClip;
    public AudioClip ShieldBackClip;

    //Game Over Screen
    public CanvasGroup GameOverScreen;
    public Text GOTopText;
    public Text GOBottomText;

    //Victory Canvas
    public CanvasGroup VictoryCanvas;
    public Text VCTopText;
    public Text VCBottomText;

    //Damage Decals
    public List<GameObject> DamageDecals;
    private int DamageLevels;

    public MissionBase CurrentMission;

    // Start is called before the first frame update
    void Start()
    {
        Shields = 100;
        Hull = 100;
        CanRecharge = true;
        DamageLevels = 10;

        for(int i = 0; i < DamageDecals.Count; i++)
        {
            DamageDecals[i].SetActive(false);
        }

        CurrentMission = GameObject.FindGameObjectWithTag("MissionControl").GetComponent<MissionBase>();
    }

    //Taking Damage
    public void TakeDamage(float DamageTaken)
    {
        TimeTakenDamage = Time.time + RechargeWait;
        //If Player has shields remaining
        if(Shields != 0)
        {
            //Play Shield Damage Sound
            DamageSFXSource.clip = ShieldDamageSFX;
            DamageSFXSource.Play();

            //Reduce Shields
            Shields -= DamageTaken;

            //Check if shield has been over damaged
            if(Shields <= 0)
            {
                //Play Shield Lost Sound
                ShieldSource.clip = ShieldOutClip;
                ShieldSource.Play();

                Shields = 0;
            }
        }

        //Player has lost shields and is down to health
        else
        {
            //Play Hull Damage Sound
            DamageSFXSource.clip = HullDamageSFX;
            DamageSFXSource.Play();

            //Reduce Hull
            Hull -= DamageTaken;
            DecalCheck();
        }

        if(Hull <= 0)
        {
            TriggerGameOver(CurrentMission.GetTopFailText(), CurrentMission.GetBottomFailText());
        }
    }

    //Shield recharge
    private void FixedUpdate()
    {
        //If Shields have taken damage and time since has passed recharge time
        if(Shields != 100 && Time.time >= TimeTakenDamage)
        {
            //Recharge every 3 seconds
            if (CanRecharge)
            {
                ShieldSource.clip = ShieldBackClip;
                ShieldSource.Play();
                CanRecharge = false;

                //If Shields have less than 5 to full charge
                if(5 > 100 - Shields)
                {
                    Shields += 100 - Shields;
                }
                //Else Increase Shields and Delay until next recharge
                else
                {
                    Shields += 5;
                    StartCoroutine(WaitForSeconds(RechargeDelay));
                }
            }
        }

        DecalCheck();
    }

    public IEnumerator WaitForSeconds(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        CanRecharge = true;
    }

    //Recharge Powerup
    public void HalfRecharge()
    {
        RechargeWait /= 2;
        StartCoroutine(RechargePowerup());
    }

    private IEnumerator RechargePowerup()
    {
        yield return new WaitForSeconds(30f);
        RechargeWait *= 2;
    }

    //Health Back Powerup
    public void HealthBack()
    {
        if(Hull <= 90)
        {
            Hull += 10;
        }
        else
        {
            Hull = 100;
        }
        DecalCheck();
    }

    public void DecalCheck()
    {
        if(Mathf.FloorToInt(Hull / 10) != DamageLevels)
        {
            DamageLevels = Mathf.FloorToInt(Hull / 10);
            ActivateDecals();
        }
    }

    private void ActivateDecals()
    {
        for (int i = 0; i < DamageLevels; i++)
        {
            DamageDecals[i].SetActive(false);
        }
        for (int i = DamageLevels; i < (DamageDecals.Count); i++)
        {
            DamageDecals[i].SetActive(true);
        }
    }

    public void TriggerGameOver(string TopString, string BottomString)
    {
        //Open Game Over Screen
        GameOverScreen.alpha = 1;
        GameOverScreen.interactable = true;
        GameOverScreen.blocksRaycasts = true;
        GOTopText.text = TopString;
        GOBottomText.text = BottomString;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Freeze Time
        Time.timeScale = 0;
    }

    public void TriggerVictory(string TopString, string BottomString)
    {
        VictoryCanvas.alpha = 1;
        VictoryCanvas.interactable = true;
        VictoryCanvas.blocksRaycasts = true;
        VCTopText.text = TopString;
        VCBottomText.text = BottomString;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Freeze Time
        Time.timeScale = 0;
    }
}
