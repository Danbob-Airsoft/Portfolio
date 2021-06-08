using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMovement : MonoBehaviour
{
    private Rigidbody ThisBody;

    public float MoveSpeed;
    public float RotationSpeed;
    public GameObject Player;
    public GameObject ShipFront;

    public float NextDirectionTime;
    public Vector3 RandomPosition;

    private FighterFiring FireScript;
    private bool CurrentlyAvoiding;
    private GameObject HitObject;

    private RaycastHit ObjectHit;

    private bool CanSpotPlayer;
    private Quaternion NewRotation;

    // Start is called before the first frame update
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ThisBody = this.transform.parent.GetComponent<Rigidbody>();
        FireScript = this.GetComponent<FighterFiring>();
        NextDirectionTime = Time.time;
        RandomPosition = new Vector3(0, 0, 0);
        CurrentlyAvoiding = false;
        ObjectHit = new RaycastHit();

        CanSpotPlayer = true;
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        SoloMovement();
    }

    public void SoloMovement()
    {
        //Check for obstacle in Agent Path
        if (CheckForObstacle(ObjectHit))
        {
            //Obstacle found, Identify Obstacle
            IdentifyObstacle(ObjectHit);
        }

        else
        {
            //Check for player in line of sight
            RaycastHit PlayerHit = new RaycastHit();
            if(CheckForSight(PlayerHit))
            {
                //Player Spotted but not in range to shoot
                var q = Quaternion.LookRotation(Player.transform.position - transform.position);
                transform.root.rotation = Quaternion.RotateTowards(transform.rotation, q, RotationSpeed * Time.deltaTime);
            }

            else
            {
                if (CurrentlyAvoiding == true)
                {
                    AvoidObstacle(HitObject.transform.position);
                }
                else
                {
                    NewRotaiton();
                }
            }
        }

        //Move Fighter Forwards
        ThisBody.AddForce(this.transform.forward * MoveSpeed, ForceMode.Impulse);
    }

    private bool CheckForObstacle(RaycastHit hit)
    {
        Physics.Raycast(ShipFront.transform.position, ShipFront.transform.forward, out hit, 300f);
        if (hit.transform != null)
        {
            ObjectHit = hit;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void IdentifyObstacle(RaycastHit hit)
    {
        //Check for Player
        if (hit.transform.root.CompareTag("Player"))
        {
            //Agent is about to ram, avoid!
            if (hit.distance <= 99f)
            {
                HitObject = Player;
                CurrentlyAvoiding = true;
                AvoidObstacle(Player.transform.position);
                CanSpotPlayer = false;
                StartCoroutine(SightCooldown());
            }

            //If in range but far enough away to not ram
            else if (hit.distance <= 300f && hit.distance >= 100f)
            {
                //Shoot Player
                FireScript.FireCannons(hit.point + (Player.transform.forward * Player.GetComponent<PlayerMovement>().MoveSpeed));
            }

            else
            {
                //Player not in range to shoot
                var q = Quaternion.LookRotation(Player.transform.position - transform.position);
                transform.root.rotation = Quaternion.RotateTowards(transform.rotation, q, RotationSpeed * Time.deltaTime);
                this.transform.root.Rotate(new Vector3(0, -90, 0));
            }
        }

        else if(hit.transform.tag == "Border" && hit.distance <= 100f)
        {
            //Make ship do a 180 turn
            CurrentlyAvoiding = false;
            NewRotation = Quaternion.LookRotation((this.transform.root.position - new Vector3(0,0,0)) * 0.5f);
            transform.root.rotation = Quaternion.RotateTowards(transform.rotation, NewRotation, RotationSpeed * Time.deltaTime);
            NextDirectionTime = Time.time;
            NewRotaiton();

        }

        else if (hit.transform.root.tag != this.transform.root.tag && hit.transform.root.tag != "EnemyLaser" && hit.transform.root.tag != "PlayerLaser")
        {
            if (CurrentlyAvoiding == false || hit.distance <= 200f)
            {
                //Reset Avoidance
                CurrentlyAvoiding = false;
                HitObject = hit.transform.gameObject;
                //Avoid Obstacle
                AvoidObstacle(hit.transform.position);
            }
        }

        //Just incase any of the avoidance criteria isn't met, pick an new direction and continue random movement
        else
        {
            NextDirectionTime = Time.time;
            NewRotaiton();
        }
    }

    private bool CheckForSight(RaycastHit PlayerHit)
    {
        Vector3 VectToPlayer = Player.transform.position - ShipFront.transform.position;
        if (CanSpotPlayer && VectToPlayer.magnitude <= 300f)
        {
            Physics.Raycast(ShipFront.transform.position, VectToPlayer, out PlayerHit, 300);

            if (PlayerHit.transform != null)
            {
                // Is Player Hit?
                if (PlayerHit.transform.root.gameObject.tag == "Player" && PlayerHit.distance <= 300)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }

    private void NewRotaiton()
    {
        //If time to next movement or within distance of target
        if (Time.time >= NextDirectionTime || RangeToTarget())
        {
            NextDirectionTime = Time.time + Random.Range(15, 30);
            RandomPosition.x = Random.Range(-900, 900);
            RandomPosition.y = Random.Range(-900, 900);
            RandomPosition.z = Random.Range(-900, 900);
        }

        Quaternion NewRotation = Quaternion.LookRotation(RandomPosition - this.transform.root.position);
        transform.root.rotation = Quaternion.RotateTowards(transform.rotation, NewRotation, RotationSpeed * Time.deltaTime);
    }

    private bool RangeToTarget()
    {
        if((this.transform.root.position.x > RandomPosition.x - 50f && this.transform.root.position.x < RandomPosition.x + 50f)
            && (this.transform.root.position.y > RandomPosition.y - 50f && this.transform.root.position.y < RandomPosition.y + 50f)
            && (this.transform.root.position.z > RandomPosition.z - 50f && this.transform.root.position.z < RandomPosition.z + 50f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AvoidObstacle(Vector3 ObjectLoc)
    {
        if (CurrentlyAvoiding == false)
        {
            CurrentlyAvoiding = true;
            NewRotation = Quaternion.LookRotation((this.transform.root.position - ObjectLoc) * 0.5f);
        }

        //Avoid Obstacle
        transform.root.rotation = Quaternion.RotateTowards(transform.rotation, NewRotation, RotationSpeed * Time.deltaTime);
        if (this.transform.root.rotation != NewRotation)
        {
            NextDirectionTime = Time.time;
            CurrentlyAvoiding = false;
        }
    }

    private IEnumerator SightCooldown()
    {
        yield return new WaitForSeconds(15f);
        CanSpotPlayer = true;
    }
}
