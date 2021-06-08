using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PowerUpScript : MonoBehaviour
{
    public AudioSource PowerupSource;

    private void Start()
    {
        PowerupSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if player overlapping
        if(other.transform.root.gameObject.CompareTag("Player"))
        {
            PowerupSource.Play();

            other.transform.root.GetComponent<PlayerHUD>().PowerupText.text = "";
            other.transform.root.GetComponent<PlayerHUD>().PowerupNotTime = Time.time + 5.0F;
            //Check effect to apply
            if (this.gameObject.CompareTag("DoubleDamage"))
            {
                other.transform.root.GetComponent<PlayerHUD>().PowerupText.text = "Double Damage!";
                other.transform.root.GetComponent<PlayerFiring>().DoubleDamage();
            }
            else if (this.gameObject.CompareTag("HalfRecharge"))
            {
                other.transform.root.GetComponent<PlayerHUD>().PowerupText.text = "Shield Recharge Wait Halfed!";
                other.transform.root.GetComponent<PlayerHealth>().HalfRecharge();
            }
            else if (this.gameObject.CompareTag("HealthRestore"))
            {
                other.transform.root.GetComponent<PlayerHUD>().PowerupText.text = "10% Health Restored!";
                other.transform.root.GetComponent<PlayerHealth>().HealthBack();
            }
            else if (this.gameObject.CompareTag("SpeedBoost"))
            {

                other.transform.root.GetComponent<PlayerHUD>().PowerupText.text = "Speed Increased!";
                other.transform.root.GetComponent<PlayerMovement>().SpeedBoost();
            }
            else if (this.gameObject.CompareTag("UnlimitedAmmo"))
            {
                other.transform.root.GetComponent<PlayerHUD>().PowerupText.text = "UNLIMITED AMMO!!";
                other.transform.root.GetComponent<PlayerFiring>().InfiniteAmmo();
            }

            //Destroy Powerup
            Destroy(this.gameObject);
        }
    }
}
