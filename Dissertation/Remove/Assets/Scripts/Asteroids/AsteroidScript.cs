using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public float DamageToTake;
    public bool Border;

    private void OnCollisionStay(Collision collision)
    {

        if(collision.gameObject.name == "Player")
        {
            if (Border)
            {
                collision.gameObject.GetComponent<PlayerMovement>().ThisBody.AddForce(this.transform.up * 200, ForceMode.Impulse);
                collision.gameObject.GetComponent<PlayerMovement>().MoveSpeed = 0f;
            }
            else
            {
                Vector3 VectAway = collision.gameObject.transform.position - this.transform.position;
                //Make value same
                VectAway /= VectAway.magnitude;

                collision.gameObject.GetComponent<PlayerMovement>().ThisBody.AddForce(VectAway * 100, ForceMode.Impulse);
                collision.gameObject.GetComponent<PlayerMovement>().MoveSpeed = -7f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.tag == "EnemyFighter")
        {
            Debug.Log("Fighter has somehow hit an obstacle");
            collision.gameObject.GetComponentInChildren<FighterHealth>().TakeDamage(99);
        }

        else if(collision.transform.root.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(DamageToTake);
        }
    }
}
