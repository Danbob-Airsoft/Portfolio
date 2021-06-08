using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FighterHealth : MonoBehaviour
{
    public float MaxHealth;
    public float ShipHealth;

    public List<GameObject> Powerups;

    public PlayerScore ScoreScript;

    public ParticleSystem Explosion;

    public FighterMovement ThisMovement;

    public AudioSource DamageSource;
    public FormationBase ThisFormationBase;
    public int FormationIndex;

    private bool ShipDead;

    private void Start()
    {
        ScoreScript = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerScore>();
        ThisMovement = this.gameObject.GetComponent<FighterMovement>();
        DamageSource = this.gameObject.GetComponent<AudioSource>();
        ShipHealth = MaxHealth;
        ShipDead = false;
    }

    public void TakeDamage(int DamageTaken)
    {
        //Reduce health and play sound
        ShipHealth -= DamageTaken;
        DamageSource.Play();
        //Disperse formation and mark as in combat
        ThisFormationBase.EnterCombat();

        //Check if fighter is dead
        if (ShipHealth <= 0 && ShipDead == false)
        {
            ShipDead = true;
            int RandomChance = Random.Range(1, 25);
            if(RandomChance == 11)
            {
                Instantiate(Powerups[Random.Range(0, Powerups.Capacity -1)], this.transform.position, this.transform.rotation);
            }
            ScoreScript.AddScore(5);
            Explosion.Play();
            ThisMovement.MoveSpeed = 0;
            ThisMovement.RotationSpeed = 0;
            ThisFormationBase.RemoveFighter(this.transform.parent.gameObject, FormationIndex);
            StartCoroutine(Exploding());
        }
    }

    private IEnumerator Exploding()
    {
        MeshRenderer[] Meshes = this.transform.root.GetChild(1).GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer Mesh in Meshes)
        {
            Mesh.enabled = false;
        }
        yield return new WaitForSeconds(Explosion.main.duration);
        Destroy(this.transform.root.gameObject);
    }
}
