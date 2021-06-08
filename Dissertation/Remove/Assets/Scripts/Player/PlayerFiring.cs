using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerFiring : MonoBehaviour
{
    private LayerMask Mask;

    //Info for Cannons and Bullet launching
    public GameObject[] CannonLocations;
    private Vector3 VectToTarget;
    public GameObject Laser;

    //Bullet Variables
    public ParticleSystem CannonPartilces;

    public Camera CurrentCamera;

    private AudioSource ThisSource;


    //---------------------------------------Primary Firing Variables-------------------------------------
    //Ammo Info
    public int Ammo;
    public int MaxAmmo;
    public int LaserDamage;

    //Cooldown and spread
    public float WeaponCooldown;
    private float NextFire;

    public bool CanPReload;

    //----------------------------------------Secondary Firing Variables-----------------------------------
    private float NextSecondFire;
    public int SecondaryAmmo;
    public int SecondaryMaxAmmo;
    public float SecondaryWeaponCooldown;
    public List<GameObject> Missiles;
    public List<GameObject> ActiveMissiles;
    public GameObject MissileObj;

    private bool CanSReload;

    //-----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        Mask = 1 << 8;
        Mask = ~Mask;
        NextFire = Time.time;
        NextSecondFire = Time.time;
        Ammo = MaxAmmo;
        SecondaryAmmo = SecondaryMaxAmmo;
        ActiveMissiles = new List<GameObject>(Missiles);
        CanPReload = true;
        CanSReload = true;
        LaserDamage = 2;
        ThisSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        //------------------------------------------Check Primary Fire Key-------------------------------------
        //Check if can fire, not in cooldown
        if (KeyBindingManager.GetKey(KeyAction.FireLaser) && Time.time > NextFire && Time.timeScale != 0)
        {
            //-------------------------------------If Has Ammo-----------------------------
            if (Ammo > 0)
            {
                //Increase time to next shot
                NextFire = Time.time + WeaponCooldown;

                //Raycast from front of ship (Targeting Marker)
                RaycastHit hit;
                if (Physics.Raycast(CurrentCamera.transform.position, CurrentCamera.transform.forward, out hit, 1000f, Mask))
                {
                    if (hit.transform.CompareTag("EnemyFighter"))
                    {
                        //For each Cannon
                        foreach (GameObject Cannon in CannonLocations)
                        {
                            //Create Vector between Hit point and each cannon
                            VectToTarget = Cannon.transform.position - (hit.point + hit.transform.root.right * 15);
                            //Look at point
                            Cannon.transform.LookAt(hit.point);

                            FireLaser(VectToTarget, Cannon);

                            //Reduce Ammo
                            Ammo -= 1;
                        }
                    }

                    else
                    {
                        foreach (GameObject Cannon in CannonLocations)
                        {
                            Cannon.transform.rotation = new Quaternion(0, 0, 0, 0);
                            VectToTarget = Cannon.transform.forward * 200f;

                            FireLaser(VectToTarget, Cannon);

                            //Reduce Ammo
                            Ammo -= 1;
                        }
                    }

                }
                else
                {
                    foreach (GameObject Cannon in CannonLocations)
                    {
                        Cannon.transform.rotation = new Quaternion(0, 0, 0, 0);
                        VectToTarget = Cannon.transform.forward * 200f;

                        FireLaser(VectToTarget, Cannon);

                        //Reduce Ammo
                        Ammo -= 1;
                    }

                }
            }

            //--------------------------------------Else Play overheat sound------------------------------------
            else
            {
                if (!ThisSource.isPlaying && CanPReload)
                {
                    ThisSource.Play();
                    CanPReload = false;
                    StartCoroutine(Reload(WeaponCooldown * 2));
                }
            }
        }


        //----------------------------------Check Secondary Fire Key---------------------------------------
        if (KeyBindingManager.GetKey(KeyAction.FireMissile) && Time.time > NextSecondFire && Time.timeScale != 0)
        {
            //If Player has Secondary Ammo
            if (SecondaryAmmo > 0)
            {
                //Increase time to next shot
                NextSecondFire = Time.time + SecondaryWeaponCooldown;

                RaycastHit hit;
                //Raycast From Camera forwards
                if (Physics.Raycast(CurrentCamera.transform.position, CurrentCamera.transform.forward, out hit, Mathf.Infinity, Mask))
                {
                    for (int i = 0; i !=4; i++)
                    {
                        if(ActiveMissiles[i].activeSelf == true)
                        {
                            //Get Top Missile
                            GameObject CurrentMissile = ActiveMissiles[i];
                            GameObject SpawnedMissile = Instantiate(MissileObj, CurrentMissile.transform.position, this.transform.rotation);
                            //Disable CurrentMissile
                            CurrentMissile.SetActive(false);

                            //Rotate cannon to hit point
                            SpawnedMissile.transform.LookAt(hit.point);

                            Rigidbody MissileBody = SpawnedMissile.transform.GetChild(0).GetComponent<Rigidbody>();

                            //Add force to Laser
                            MissileBody.AddForce(MissileBody.transform.right * 20, ForceMode.Impulse);

                            //Reduce Secondary Ammo
                            SecondaryAmmo -= 1;
                            break;
                        }
                    }
                }
            }
            //Else Auto Reload
            else
            {
                CanSReload = false;
                StartCoroutine(SecondaryReload(SecondaryWeaponCooldown * 2));
            }
        }

        //--------------------------------Manual Weapon Reload Trigger-----------------------------------
        if (KeyBindingManager.GetKey(KeyAction.Reload) && Time.timeScale != 0)
        {
            if(CanPReload == true)
            {
                CanPReload = false;
                StartCoroutine(Reload(WeaponCooldown * 2));
            }
            if(CanSReload == true)
            {
                CanSReload = false;
                StartCoroutine(SecondaryReload(SecondaryWeaponCooldown * 2));
            }
        }
    }

    //----------------------------------------------------Primary Reload-----------------------------------------
    private IEnumerator Reload(float WeaponCooldown)
    {
        NextFire = Time.time + (WeaponCooldown * 4);
        yield return new WaitForSeconds(WeaponCooldown * 4);
        //Refill ammo
        Ammo = MaxAmmo;
        CanPReload = true;
    }

    //-----------------------------------------------------Secondary Reload---------------------------------------
    private IEnumerator SecondaryReload(float WeaponCooldown)
    {
        NextSecondFire = Time.time + (WeaponCooldown);
        yield return new WaitForSeconds(WeaponCooldown);
        //Restock Missile Array
        foreach (GameObject Missile in ActiveMissiles)
        {
            Missile.SetActive(true);
        }
        //Reset Missiles
        SecondaryAmmo = SecondaryMaxAmmo;
        CanSReload = true;
    }

    //----------------------------------------------------Wait For Seconds Cooldown--------------------------------
    private IEnumerator WeaponCooldownTimer(float WeaponCooldown)
    {
        yield return new WaitForSeconds(WeaponCooldown);
    }

    //---------------------------------------------------Primary Laser Fire----------------------------------------
    public void FireLaser(Vector3 VectToPoint, GameObject Cannon)
    {
        GameObject FXSource = Cannon.transform.Find("Muzzle_plasma").gameObject;
        GameObject SFXSource = Cannon.transform.Find("SFX").gameObject;
        AudioSource SFXClip = SFXSource.GetComponent<AudioSource>();
        //Get Child Particle system
        CannonPartilces = FXSource.GetComponent<ParticleSystem>();

        //Play muzzle flash
        CannonPartilces.Play(true);
        //Play Sound
        SFXClip.Play();

        //Spawn Laser
        GameObject FiredLaser = Instantiate(Laser, Cannon.transform.position, Cannon.transform.rotation);
        FiredLaser.transform.Rotate(90, 0, 0);
        Rigidbody LaserBody = FiredLaser.GetComponent<Rigidbody>();
        FiredLaser.GetComponent<PlayerProjectile>().Damage = LaserDamage;

        //Add force to Laser
        LaserBody.AddForce(LaserBody.transform.up * 0.3f, ForceMode.Impulse);
    }

    //----------------------------------------------- Infinite Ammo Powerup --------------------------------
    public void InfiniteAmmo()
    {
        Ammo = int.MaxValue;
        StartCoroutine(InfiniteAmmoCooldown(30f));
    }

    private IEnumerator InfiniteAmmoCooldown(float CooldownTime)
    {
        yield return new WaitForSeconds(CooldownTime);
        Ammo = MaxAmmo;
    }

    //--------------------------------------------- Double Damage Powerup ---------------------------------
    public void DoubleDamage()
    {
        LaserDamage = LaserDamage * 2;
        StartCoroutine(DamagePowerupCooldown(30f));
    }

    private IEnumerator DamagePowerupCooldown(float CooldownTime)
    {
        yield return new WaitForSeconds(CooldownTime);
        LaserDamage = LaserDamage / 2;
    }
}
