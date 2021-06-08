using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int Damage;
    public ParticleSystem Explosion;
    public bool Exploding;
    public PlayerHUD HUDScript;

    private void Start()
    {
        Exploding = false;
        HUDScript = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerHUD>();
    }

    private void Update()
    {
        if(Exploding == true)
        {
            if (!Explosion.isPlaying)
            {
                //Destroy Projectile
                Destroy(this.transform.root.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player" && !collision.gameObject.CompareTag(this.gameObject.tag))
        {
            //Check for Enemy Fighter (Both lasers and missiles can damage fighters)
            if (collision.gameObject.tag == "EnemyFighter")
            {
                //Laser
                if (this.gameObject.tag == "PlayerLaser")
                {
                    //Deal Damage to Enemy
                    collision.gameObject.transform.GetComponentInChildren<FighterHealth>().TakeDamage(Damage);
                    HUDScript.DisplayHitMarker();
                    //Destroy Projectile
                    Destroy(this.transform.root.gameObject);
                }
                // Missile
                else
                {
                    //Deal Damage to Enemy
                    collision.gameObject.GetComponentInChildren<FighterHealth>().TakeDamage(20);
                    HUDScript.DisplayHitMarker();
                }
            }

            //Check if need to explode
            if (this.gameObject.CompareTag("PlayerMissile"))
            {
                this.transform.GetComponent<MeshCollider>().enabled = false;
                this.transform.GetComponent<MeshRenderer>().enabled = false;
                Explosion.Play();
                Exploding = true;
            }

            else
            {
                Destroy(this.gameObject);
            }


            //Check for Enemy Capital Ship
            /*else if (collision.gameObject.tag == "EnemyCapital")
            {
                //Check if this is Missile (Lasers do not damage capital ships)
                if (this.gameObject.tag == "PlayerMissile")
                {
                    //Deal Damage to ship
                    collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(5);
                }
            }*/

        }
    }
}
