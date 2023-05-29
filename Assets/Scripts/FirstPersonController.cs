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
    // public Camera playerCamera;
    [SerializeField] private GameObject _cameraPlaceHolder;
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
        Debug.Log("PlayerController activated");
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }


    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (walkingSpeed * _moveInput.y) : 0;
        float curSpeedY = canMove ? (walkingSpeed * _moveInput.x) : 0;
        float _movementDirectionZ = moveDirection.z;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.z = _movementDirectionZ;

        if (_isJump && canMove && IsGrounded())
         {
             moveDirection.z = -jumpSpeed;
            _isJump = false;
         }
         else
         {
             moveDirection.z = _movementDirectionZ;
         }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)

        if (!IsGrounded())
        {
            if (moveDirection.z < 6f)
            {
                moveDirection.z += gravity * Time.deltaTime;
            }
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove && Time.timeScale != 0)
        {
            rotationX += -_mouseInput.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            _cameraPlaceHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, _mouseInput.x * lookSpeed, 0);
            }
    }
    private bool IsGrounded() 
 {
        return Physics.Raycast(transform.position, Vector3.forward, 0.3f);
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
        if(!PlayerMovement.MouseFollowEnabled)
        {
            _mouseInput = value.Get<Vector2>();
        }
    }
}