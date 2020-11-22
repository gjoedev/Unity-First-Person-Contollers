using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBasedFPS : MonoBehaviour
{
    public Rigidbody rb;
    public float WalkSpeed;
    float xInput = Input.GetAxis("Horizontal");
    float zInput = Input.GetAxis("Vertical");

    //Movement

    float CamX, CamY, MXR;
    public Transform Camera;
    public float Sensitivity;
    public float LookMin, LookMax;
    public float MaxSpeed;

    //Camera Stuff

    public Transform GroundCheck;
    private bool isGrounded;
    public float GroundCheckRadius;
    public float JumpHieght;
    public LayerMask WhatisGround;
    public float CrouchHight;
    public float RegularHight;
    private bool CanUnCrouch;
    private bool isCrouching;
    public Transform CeelingCheck;

    //Jumping and Crouching

    private float MoveSpeed;
    public float SprintSpeed;

    //Walking and Sprinting

    private float defualtdrag = 0.0f;
    public float CounterDrag;
    private Vector2 movement;
    private Vector2 prevMovement;
    //Counter Movement
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Movement();
        JumpAndCrouch();
        //Check for the ground
        isGrounded = Physics.CheckSphere(GroundCheck.transform.position, GroundCheckRadius, WhatisGround);
        CanUnCrouch = Physics.CheckSphere(CeelingCheck.transform.position, GroundCheckRadius, WhatisGround);
    }
    private void Update()
    {
        GetInput();
        CameraControls();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(isGrounded);
        }
    }
    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        CamX = Input.GetAxis("Mouse X") * Sensitivity;
        CamY = Input.GetAxis("Mouse Y") * Sensitivity;
    }
    void CameraControls()
    {
        MXR -= CamY;
        MXR = Mathf.Clamp(MXR, LookMin, LookMax);

        Camera.transform.localRotation = Quaternion.Euler(MXR, 0f, 0f);
        transform.Rotate(Vector3.up * CamX);
    }
    void JumpAndCrouch()
    {
        if (isGrounded == true && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * JumpHieght * 50);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1, CrouchHight, 1);
            isCrouching = true;
        }
        else
        {
            if(CanUnCrouch == false)
            {
                transform.localScale = new Vector3(1, RegularHight, 1);
                isCrouching = false;
            }
        }
    }
    private void Movement()
    {
        rb.AddForce(transform.forward * zInput * MoveSpeed);
        rb.AddForce(transform.right * xInput * MoveSpeed);
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Countermovement (HUGE thanks to alwayscodeangry on answers.unity.com, the page I found this on is here https://answers.unity.com/questions/1701997/rigidbody-controller-counter-movement.html?sort=votes)
        if ((prevMovement.x != movement.x || prevMovement.y != movement.y)  || rb.velocity.magnitude > MaxSpeed)
        {
            rb.drag = CounterDrag;
        }
        else
        {
            rb.drag = defualtdrag;
        }
        prevMovement = movement;
        //End Countermovement code

        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeed = SprintSpeed;
            Debug.Log("I got Here!");
        } else
        {
            MoveSpeed = WalkSpeed;
        }
    }
   
}
