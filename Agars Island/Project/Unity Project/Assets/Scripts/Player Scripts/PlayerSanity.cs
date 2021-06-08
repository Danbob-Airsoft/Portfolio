using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// Manages Player sanity & feedback to the player for sanity
/// 
/// Created by: Daniel Bailey
/// Edited by: Lewis Arnold
/// </summary>
public class PlayerSanity : MonoBehaviour
{
    [Header("Required Variables")]
    //Current Sanity
    public float Sanity;
    //Distance AI can be within to drain
    public float DrainDistance;
    //Speed at which to drain sanity
    public float DrainSpeed;
    //Max sanity
    private float maxSanity = 100f;
    
    // Feedback
    [SerializeField] private Slider sanityBar;
    private CameraPostProcess sanityPP;

    [SerializeField] private AudioSource heartbeat;

    //Enemy object
    private GameObject Enemy;
    //Game manager
    private CheckpointManager GameManager;
    //Inventory tracker for sanity usage
    private InventoryTracker Inventory;
    public LayerMask Mask;

    //Game Over Screen

    //Sanity sounds
    [SerializeField] private AudioSource violinsSound;

    // Start is called before the first frame update
    void Start()
    {
        //Set enemy and Inventory
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        Enemy.SetActive(false);
        Inventory = this.GetComponent<InventoryTracker>();

        sanityPP = GetComponentInChildren<CameraPostProcess>();

        GameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<CheckpointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float drainAmount = 0;
        //Get distance to enemey
        float DistToEnemy = Vector3.Distance(this.transform.position, Enemy.transform.position);
        //Set sound volume
        violinsSound.enabled = Enemy.activeInHierarchy;
        violinsSound.volume = RadiatorGames.Math.Mapping.Map(1, 40, 1, 0, DistToEnemy);
        
        //If enemy is close enough to drain sanity
        if(DistToEnemy <= DrainDistance && GameManager.GeneratorOn)
        {
            drainAmount = DrainSpeed / 4;
            //Get vector to enemy
            Vector3 VectToEnemy = Enemy.transform.position - this.transform.position;
            RaycastHit Hit;
            //Get world viewport to check if enemy is on screen
            Vector3 MyPosition = Camera.main.WorldToViewportPoint(Enemy.transform.position);
            //If enemy is infront and onscreen
            if ((MyPosition.x >= 0.0f && MyPosition.x <= 1.0f) && (MyPosition.y >= 0.0f && MyPosition.y <= 1.0f) && MyPosition.z >= 0)
            {
                //Draw raycast for debug
                Debug.DrawRay(this.transform.position + (this.transform.forward * 2), VectToEnemy, Color.red);
                //Raycast towards enemy (check for obstacles obscuring view)
                if (Physics.Raycast(this.transform.position, VectToEnemy, out Hit, 75f, ~Mask))
                {
                    //Check if hit enemy
                    if (Hit.transform.CompareTag("Enemy"))
                    {
                        //Drain Sanity
                        drainAmount = DrainSpeed;
                    }
                }
            }

            Sanity -= drainAmount * Time.deltaTime;
        }
        //Update sanity bar
        //sanityBar.value = (Sanity / maxSanity) * 100f;
        sanityPP.SetVignetteRadius(RadiatorGames.Math.Mapping.Map(0f, maxSanity, 0.65f, 1.0f, Sanity));
        heartbeat.volume = RadiatorGames.Math.Mapping.Map(0, maxSanity, 1, 0, Sanity);
        heartbeat.pitch = RadiatorGames.Math.Mapping.Map(0, maxSanity, 1, 0, Sanity);

        //Use sanity meds if key pressed and inventory contains meds
        if(Input.GetKeyDown(KeyCode.M) && Inventory.meds > 0)
        {
            Inventory.RemoveMeds(1);
            Sanity += 25f;
            Mathf.Clamp(Sanity, 0, 100);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            this.GetComponent<HUDManager>().TriggerGameOver();
            Time.timeScale = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (Enemy)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Enemy.transform.position, DrainDistance);
        }
    }

    public void KillAudio()
    {
        violinsSound.volume = 0f;
        heartbeat.volume = 0f;
    }
}
