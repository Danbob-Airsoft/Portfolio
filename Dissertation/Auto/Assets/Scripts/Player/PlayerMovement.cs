using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    public int RollSpeed;
    public float TurnSpeed;
    private float Sensitivity;

    public float MoveSpeed;
    public float MinSpeed;
    public float MaxSpeed;
    private float Acceleration;

    private bool Boosting;

    private float Slowdown;
    private bool Dampeners;

    public Camera ActiveCamera;

    public Rigidbody ThisBody;

    public GameObject AimMarker;
    public GameObject TrailObject;

    private TrailRenderer LeftEngine;
    private TrailRenderer RightEngine;

    private PlayerHealth HealthScript;

    public GameObject BoostMarker;
    private Image BoostHUD;

    public GameObject DampenersMarker;
    private Image DampenersHUD;

    public Image TurnCircle;

    public void Start()
    {
        Time.timeScale = 1;
        ThisBody = this.GetComponent<Rigidbody>();
        Dampeners = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Sensitivity = PlayerPrefs.GetFloat("Sensitivity");

        HealthScript = this.GetComponent<PlayerHealth>();

        LeftEngine = TrailObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
        RightEngine = TrailObject.transform.GetChild(1).gameObject.GetComponent<TrailRenderer>();

        LeftEngine.emitting = false;
        RightEngine.emitting = false;

        MoveSpeed = MinSpeed;
        Slowdown = 0.2f;
        Acceleration = 0.2f;
        Boosting = false;

        BoostHUD = BoostMarker.GetComponent<Image>();
        DampenersHUD = DampenersMarker.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        // If not Puased
        if(Time.timeScale != 0)
        {
            //Take damage if boosting and check for shield remaining
            if (Boosting)
            {
                HealthScript.TakeDamage(0.3f);
                if(HealthScript.Shields <= 0)
                {
                    Boosting = false;
                    MaxSpeed -= 10f;
                    Acceleration /= 2;
                    BoostHUD.enabled = false;
                }
            }

            //-----------------------------------------Move Forwards------------------
            if (KeyBindingManager.GetKey(KeyAction.Accelerate))
            {
                if (MoveSpeed <= MaxSpeed - Acceleration)
                {
                    MoveSpeed += Acceleration;
                }
                else
                {
                    MoveSpeed = MaxSpeed;
                }
            }

            //Return to Minimum speed if not accelerating
            else
            {
                //Disable Boosting
                if (Boosting)
                {
                    Boosting = false;
                    MaxSpeed -= 10f;
                    Acceleration /= 2;
                    BoostHUD.enabled = false;
                }

                //Decrease Speed to normal when not holding
                if (MoveSpeed > MinSpeed)
                {
                    if(MoveSpeed > MinSpeed + Slowdown)
                    {
                        MoveSpeed -= Slowdown;
                    }
                    else
                    {
                        MoveSpeed = MinSpeed;
                    }
                }
                //Increase speed if bellow minimum (caused by collision)
                else if (MoveSpeed < MinSpeed)
                {
                    if(MoveSpeed < MinSpeed - Acceleration)
                    {
                        MoveSpeed += Acceleration;
                    }
                    else
                    {
                        MoveSpeed = MinSpeed;
                    }
                }
            }

            //Move
            ThisBody.AddForce(this.transform.forward * MoveSpeed, ForceMode.Impulse);
            AimMarker.transform.Rotate(0, 0, MoveSpeed / 2.5f);

            //---------------------------------------Ascend and Descend----------------------------
            int UpDirection = 0;
            if (KeyBindingManager.GetKey(KeyAction.Descend))
            {
                UpDirection = -1;
                ThisBody.AddForce(this.transform.up * MoveSpeed * UpDirection, ForceMode.Impulse);
            }
            else if (KeyBindingManager.GetKey(KeyAction.Ascend))
            {
                UpDirection = 1;
                ThisBody.AddForce(this.transform.up * MoveSpeed * UpDirection, ForceMode.Impulse);
            }
        }
    }

    private void Update()
    {
        //-------------------------------------Enable/Disable Dampeners----------------------
        if (KeyBindingManager.GetKeyDown(KeyAction.Dampeners))
        {
            if (Dampeners)
            {
                Dampeners = false;
                //Disable Drag
                ThisBody.drag = 2f;
                ThisBody.angularDrag = 2f;
                MaxSpeed += 2.5f;
                Slowdown /= 2;

                DampenersHUD.enabled = false;
            }
            else
            {
                Dampeners = true;
                //Enable Drag
                ThisBody.drag = 4;
                ThisBody.angularDrag = 4;
                MaxSpeed -= 2.5f;
                Slowdown *= 2;

                DampenersHUD.enabled = true;
            }
        }

        //Check for Boosting
        if (KeyBindingManager.GetKeyDown(KeyAction.ShieldForSpeed))
        {
            if (Boosting)
            {
                Boosting = false;
                MaxSpeed -= 15f;
                Acceleration /= 2;
                BoostHUD.enabled = false;
            }

            else if (HealthScript.Shields > 0 && !Boosting)
            {
                Boosting = true;
                MaxSpeed += 15f;
                Acceleration *= 2;
                BoostHUD.enabled = true;
            }
        }

        //-----------------------------------------Rotate With Mouse----------------------------
        //Mouse Rotates Player
        float Yaw = TurnSpeed * (Input.GetAxis("Mouse X") * Sensitivity);
        float Pitch = TurnSpeed * (Input.GetAxis("Mouse Y") * Sensitivity);

        float XVal = Input.GetAxis("Mouse X");
        float YVal = Input.GetAxis("Mouse Y");

        TurnCircle.transform.localPosition = new Vector3(Mathf.Clamp(TurnCircle.transform.localPosition.x, -200, 200),
        Mathf.Clamp(TurnCircle.transform.localPosition.y, -200, 200), 0);
        TurnCircle.transform.localPosition = new Vector3(Mathf.RoundToInt(TurnCircle.transform.localPosition.x), 
            Mathf.RoundToInt(TurnCircle.transform.localPosition.y), 0);
        TurnCircle.transform.localPosition += new Vector3(Yaw, Pitch, 0);

        if (KeyBindingManager.GetKeyDown(KeyAction.AutoLevel))
        {
            TurnCircle.transform.localPosition = new Vector3(0, 0, 0);
        }

        //-----------------------------------------Rolling--------------------------------------
        int RollDirection = 0;
        if (KeyBindingManager.GetKey(KeyAction.RollLeft))
        {
            RollDirection = -1;
        }
        else if (KeyBindingManager.GetKey(KeyAction.RollRight))
        {
            RollDirection = 1;
        }
        else
        {
            RollDirection = 0;
        }

        //"A" And "D" Rolls Left and Right
        float Roll = RollSpeed * Time.deltaTime * RollDirection * 2;

        //--------------------------------- Apply Rotations-------------------------------------------
        gameObject.transform.Rotate(-TurnCircle.transform.localPosition.y * Time.deltaTime, 
            TurnCircle.transform.localPosition.x * Time.deltaTime, -Roll);

        //--------------------------------- Slowly reset Turn Circle ----------------------------------
        /*if (TurnCircle.transform.localPosition != new Vector3(0, 0, 0) && (XVal == 0 && YVal == 0))
        {
            Vector3 VectToCentre = new Vector3(0, 0, 0) - TurnCircle.transform.localPosition;
            TurnCircle.transform.localPosition += (VectToCentre / VectToCentre.magnitude) * 0.75f;
        }*/
    }

    //Speed Boost Powerup
    public void SpeedBoost()
    {
        MoveSpeed += 5f;
        MaxSpeed += 5f;
        StartCoroutine(SpeedBoostDelay());
    }

    private IEnumerator SpeedBoostDelay()
    {
        yield return new WaitForSeconds(30f);
        MoveSpeed -= 5f;
        MaxSpeed -= 5f;
    }

    public void ChangeSensitivity(float NewSens)
    {
        Sensitivity = NewSens;
    }

    //------------------------------------------------------- Saved for Later Use----------------------------------
    //Trail collider trials
    /*if(LeftEngine.positionCount != 0)
    {
        LeftEngine.BakeMesh(LeftMesh.mesh, ActiveCamera, true);
        RightEngine.BakeMesh(RightMesh.mesh, ActiveCamera, true);

        LeftCollider.sharedMesh = LeftMesh.mesh;
        RightCollider.sharedMesh = RightMesh.mesh;
    }
    else
    {
        LeftCollider.enabled = false;
        RightCollider.enabled = false;

        LeftMesh.mesh = null;
        RightMesh.mesh = null;
    }*/
    /*
      LeftCollider.enabled = true;
      RightCollider.enabled = true;
    */

    /*LeftMesh = TrailObject.transform.GetChild(0).gameObject.GetComponent<MeshFilter>();
      RightMesh = TrailObject.transform.GetChild(1).gameObject.GetComponent<MeshFilter>();

      LeftCollider = TrailObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>();
      RightCollider = TrailObject.transform.GetChild(1).gameObject.GetComponent<MeshCollider>();*/

    /*private MeshCollider LeftCollider;
      private MeshCollider RightCollider;

      private MeshFilter LeftMesh;
      private MeshFilter RightMesh;*/
}
