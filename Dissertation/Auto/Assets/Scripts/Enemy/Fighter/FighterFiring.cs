using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FighterFiring : MonoBehaviour
{
    public GameObject Laser;
    //public GameObject Missile;

    public GameObject[] Cannons;

    public float CannonCooldown;
    private float NextFire;

    public int MaxAmmo;
    public int CurrentAmmo;

    public float BaseAccuracy;
    public float AlteredAccuracy;


    // Start is called before the first frame update
    void Start()
    {
        NextFire = Time.time;
        Reload();
    }

    public void Reload()
    {
        CurrentAmmo = MaxAmmo;
    }

    public void FireCannons(Vector3 Target)
    {
        //Check if can shoot and have ammo
        if ((Time.time >= NextFire) && (CurrentAmmo >= Cannons.Length))
        {
            NextFire = Time.time + CannonCooldown;
            //For each Cannon
            foreach (GameObject Cannon in Cannons)
            {
                AudioSource CannonSFX = Cannon.transform.parent.GetComponent<AudioSource>();
                CannonSFX.Play();

                //Spawn Laser on cannon location
                GameObject SpawnedLaser = Instantiate(Laser, Cannon.transform.position, Cannon.transform.rotation);
                Rigidbody Laserbody = SpawnedLaser.GetComponent<Rigidbody>();
                //Look at player
                SpawnedLaser.transform.LookAt(Target);

                AlteredAccuracy = new Vector3(Cannon.transform.position.x - Target.x, Cannon.transform.position.y - Target.y,
                    Cannon.transform.position.z - Target.z).magnitude / 200;

                //float LaserSpread = (Target.magnitude / 10000) * Accuracy;
                //Add Laser Spread
                SpawnedLaser.transform.Rotate(Random.Range(-AlteredAccuracy, AlteredAccuracy),
                    Random.Range(-AlteredAccuracy, AlteredAccuracy),
                    Random.Range(-AlteredAccuracy, AlteredAccuracy));



                //Add Force
                Laserbody.AddForce(SpawnedLaser.transform.forward * 0.5f, ForceMode.Impulse);
                //Reduce Ammo
                CurrentAmmo--;
            }
            //Time.timeScale = 0;

            //Check if Ammo needs reloading
            if(CurrentAmmo < Cannons.Length)
            {
                //Increase next fire time
                NextFire = Time.time + 5;

                //Restore Ammo
                CurrentAmmo = MaxAmmo;
            }
        }
    }
}
