using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileLaser : MonoBehaviour
{

    public float LaserDamage;
    bool HasDamaged = false;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject != this.gameObject && collision.transform.root.tag != "EnemyFighter")
        {
            Destroy(this.gameObject);
        }
        if (collision.transform.root.tag == "Player" && HasDamaged ==  false)
        {
            HasDamaged = true;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(LaserDamage);
        }
    }
}
