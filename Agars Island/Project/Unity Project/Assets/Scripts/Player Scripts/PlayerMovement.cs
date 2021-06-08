using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// Controls Player Movement
///
/// Created by: Daniel Bailey
/// Edited by: Lewis Arnold
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private Camera Camera;
    [HideInInspector] public Rigidbody PlayerBody;

    //Rotation
    [Header("Rotation")]
    public float RotationSpeed;
    public float CamMinY;
    public float CamMaxY;
    public float RotSmoothSpeed;

    //Movement
    [Header("Movement")]
    public float MoveSpeed;
    public LayerMask stopLayers;
    [SerializeField] private float stopDistance;

    //Sprinting
    [Header("Sprinting")]
    public float Stamina;
    public float MaxStamina;
    private bool IsSprinting;
    
    //Stamina Drain
    [Header("Stamina drain")]
    public float SprintDrainRate;
    
    //Stamina Recovery
    [Header("Stamina recovery")]
    public int SprintRecoverDelay;
    private float NextRecTime;
    
    //Other Movement Related
    [Header("Other")]
    float Gravity = -13;
    float VelocityY = 0.0f;

    [SerializeField] private AudioSource footStep;
    [SerializeField] private float stepTime;
    private float stepTimer;
    
    //Rotations
    private float BodyRoationX;
    private float CamRotationY;
    
    // Feedback - LA 10/03
    [SerializeField] private AudioSource outOfBreath;

    //private CharacterController Controller;

    [SerializeField] private Slider staminaBar;

    [Header("Hiding")]
    public bool IsHiding;

    // Start is called before the first frame update
    void Start()
    {
        Camera = this.GetComponentInChildren<Camera>();
        PlayerBody = this.GetComponent<Rigidbody>();

        //Lock Cursor to centre
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Controller = this.GetComponent<CharacterController>();
        Stamina = MaxStamina;
        IsHiding = false;

        stepTimer = stepTime;
    }

    private void Update()
    {
        if (!IsHiding)
        {
            //Check for sprint toggle
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SprintToggle();
            }

            //Drain Sprint after delay
            if (IsSprinting)
            {
                DrainSprint();
            }

            //As long as not sprinting, recover stamina
            else if (!IsSprinting && Time.time >= NextRecTime)
            {
                RecoverSprint();
            }

            staminaBar.value = (Stamina / (float) MaxStamina) * 100.0f;

            // checking for bounce-back off hinge joint doors
            if (PlayerBody.velocity.x > 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                PlayerBody.velocity = Vector3.zero;
            }
        }

        stepTimer -= Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsHiding)
        {
            LookRotation();
            Movement();
        }
    }

    void LookRotation()
    {
        //Add any new mouse movement
        BodyRoationX += Input.GetAxis("Mouse X") * RotationSpeed;
        CamRotationY += Input.GetAxis("Mouse Y") * RotationSpeed;

        //Clamp Camera Roation Values
        CamRotationY = Mathf.Clamp(CamRotationY, CamMinY, CamMaxY);

        //Create Rotation Targets
        Quaternion CamTargetRotation = Quaternion.Euler(-CamRotationY, 0, 0);
        Quaternion BodyRotationTarget = Quaternion.Euler(0, BodyRoationX, 0);

        //Handle Rotations
        transform.rotation = Quaternion.Lerp(transform.rotation, BodyRotationTarget, Time.deltaTime * RotSmoothSpeed);
        Camera.transform.localRotation = Quaternion.Lerp(Camera.transform.localRotation, CamTargetRotation, Time.deltaTime * RotSmoothSpeed);
    }
    void Movement()
    {
        //Get Movement inputs
        float MoveH = Input.GetAxisRaw("Horizontal");
        float MoveV = Input.GetAxisRaw("Vertical");

        //Turn into Vector and Multiply by move speed
        Vector3 MoveDir = new Vector3(MoveH, 0, MoveV) * MoveSpeed;

        //Get position to move to
        Vector3 NewPos = PlayerBody.position + PlayerBody.transform.TransformDirection(MoveDir);

        //Move player
        PlayerBody.MovePosition(NewPos);

        //If player is still, ensure sprinting is disables
        if (MoveDir == Vector3.zero && IsSprinting)
        {
            SprintToggle();
        }

        //Play Sound
        PlayFootStep(MoveDir, IsSprinting);
    }
    void SprintToggle()
    {
        //Check if toggling sprinting on or off
        if (IsSprinting)
        {
            //Set to not sprinting
            IsSprinting = false;
            //Reduce move speed
            MoveSpeed /= 2;
            //Set sprint recovery time
            NextRecTime = Time.time + SprintRecoverDelay;
        }
        //Check if player has stamina
        else if (Stamina > 0)
        {
            //Enable sprinting
            IsSprinting = true;
            //Increase move speed
            MoveSpeed *= 2;
        }
    }
    void DrainSprint()
    {
        Stamina -= SprintDrainRate * Time.deltaTime;
        //Check if stamina is depleted
        if (Stamina <= 0)
        {
            IsSprinting = false;
            MoveSpeed /= 2;
            NextRecTime = Time.time + SprintRecoverDelay;
            outOfBreath.Play();
        }
    }
    void RecoverSprint()
    {
        if (Stamina < MaxStamina)
        {
            Stamina += 5;
            NextRecTime = Time.time + SprintRecoverDelay;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (transform.forward * stopDistance), 0.3f);
    }

    private void PlayFootStep(Vector3 movement, bool sprint)
    {
        if (movement.magnitude > 0 && (sprint || !footStep.isPlaying) && stepTimer <= 0)
        {
            footStep.pitch = UnityEngine.Random.Range(0.7f, 1.0f);
            if(sprint)
                footStep.volume = UnityEngine.Random.Range(0.5f, 1.0f);
            else
                footStep.volume = UnityEngine.Random.Range(0.3f, 0.7f);
                
            footStep.Play();

            stepTimer = stepTime / (sprint ? 2f : 1f);
        }
    }
}
