using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlerFPS : MonoBehaviour
{
    public Camera Camera;
    public float Sensitivity;
    private float MouseXRotation = 0f;
    public float LookMin, LookMax;
    public CharacterController Character;
    private CollisionFlags collisionFlags;
    public float AirContollModifier;
    public float SprintSpeed, WalkSpeed;
    private float MoveSpeed;
    private float x, z, CamX, CamY;
    private Vector3 Velocity;
    public float GravityModifier = -9.81f;
    public Transform GroundCheck, CeelingCheck;
    private float GroundDistance = 0.4f;
    public LayerMask Ground;
    bool IsGrounded, CanUnCrouch;
    public float JumpHieght;
    private  float CrouchHight, RegularHight;
    private bool isCrouching;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GravityModifier = Mathf.Clamp(GravityModifier, -9999, 0);
        RegularHight = transform.localScale.y;
        CrouchHight = transform.localScale.y / 2;
    }
    private void Update()
    {
        Debug.Log(IsGrounded);
        Spriting();
        Gravity();
        JumpAndCrouch();
        GetInput();
        Movement();
        CameraControlls();
  
    }
    private void GetInput()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        CamX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        CamY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
    }
    private void Movement()
    {
        Vector3 MovementVector = transform.right * x + transform.forward * z;
        Character.Move(MovementVector * MoveSpeed * Time.deltaTime);
        if (IsGrounded == false)
        {
            MoveSpeed = MoveSpeed * AirContollModifier;
        }
    }
    private void CameraControlls()
    {
        MouseXRotation -= CamY;
        MouseXRotation = Mathf.Clamp(MouseXRotation, LookMin, LookMax);

        Camera.transform.localRotation = Quaternion.Euler(MouseXRotation, 0f, 0f);
        transform.Rotate(Vector3.up * CamX);
    }
    private void Gravity()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.transform.position, GroundDistance, Ground);
        if(IsGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }
        Velocity.y += GravityModifier * Time.deltaTime;
        Character.Move(Velocity * Time.deltaTime);
    }
    private void JumpAndCrouch()
    {
        CanUnCrouch = Physics.CheckSphere(CeelingCheck.transform.position, 1.3f, Ground);
        if(Input.GetButtonDown("Jump") && IsGrounded)
        {
            Velocity.y = Mathf.Sqrt(JumpHieght * -2f * GravityModifier);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, CrouchHight, transform.localScale.z);
            isCrouching = true;
        } else
        {
            if (CanUnCrouch == false)
            {
                transform.localScale = new Vector3(transform.localScale.x, RegularHight, transform.localScale.z);
                isCrouching = false;
            }
        }
        if(isCrouching == true)
        {
            MoveSpeed = MoveSpeed / 3;
        } 
    }
    private void Spriting()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeed = SprintSpeed;
        } else
        {
            MoveSpeed = WalkSpeed;
        }
    }
    //This code is from Unitys First Person Contoller included with the 2017 unity standard assets
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(Character.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
