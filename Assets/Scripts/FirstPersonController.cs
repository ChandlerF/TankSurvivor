using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    private bool _isJump = false;
    private Vector2 _moveInput, _mouseInput;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    //-------------------------------------------------------------------------------------------------------------So I swapped most of this code for vector3 y   ->   vector3.z due to the player being sideways
    //It still doesnt fullt work and I need to figure out cinemachine to get that working
    //I also need to make the FollowMouse Function on PlayerMovement.cs be new input and hopefully add a reticle for controllers, then I need to keep the mouse from going off screen
    //--------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.up);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (walkingSpeed * _moveInput.y) : 0;
        float curSpeedY = canMove ? (walkingSpeed * _moveInput.x) : 0;
        float _movementDirectionZ = moveDirection.z;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.z = _movementDirectionZ;

        if (_isJump && canMove && characterController.isGrounded)
         {
             moveDirection.z = jumpSpeed;
            _isJump = false;
         }
         else
         {
             moveDirection.z = _movementDirectionZ;
         }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.z += gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -_mouseInput.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, _mouseInput.x * lookSpeed, 0);
        }
    }


    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }


    private void OnJump()
    {
        _isJump = true;
    }

    private void OnLook(InputValue value)
    {
        _mouseInput = value.Get<Vector2>();
    }
}