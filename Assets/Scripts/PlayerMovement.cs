using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0.01f, 5f)] float _moveSpeed = 1f;
    [SerializeField] InputActionReference pointPosition;
    [SerializeField] InputActionReference dash;
    [SerializeField] Transform tankBody;
    [SerializeField] Transform tankHull;
    [SerializeField] float bodyRotateSpeed;
    [SerializeField, Range(0.5f, 4f)] private float startBoostRefreshDelay = 0.5f;
    [SerializeField, Range(0, 1)] float boostDuration;
    //a[SerializeField] bool boostEnabled = true;

    private Vector2 pointPos;
    private Vector2 _moveInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private Rigidbody2D _rbBody;
    private Rigidbody2D _rbHull;

    [SerializeField] private TankDamageSystem _tankDamageSystem;

    float originalMoveSpeed;
    float timer;
    bool isBoosted;
    
    private float boostRefreshDelay;

    public static bool MouseFollowEnabled = true;

/*    private void OnEnable()
    {
        if (boostEnabled)
        {
            dash.action.performed += Dash;
        }
    }

    private void OnDisable()
    {
        if(boostEnabled)
        {
            dash.action.performed -= Dash;
        }
    }*/
    private void Start()
    {
        boostRefreshDelay = startBoostRefreshDelay;
        timer = 0;
        _rbBody = tankBody.GetComponent<Rigidbody2D>();
        _rbHull = tankHull.GetComponent<Rigidbody2D>();
        originalMoveSpeed = _moveSpeed;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Dash();
        }
    }
    private void FixedUpdate()
    {
        if(isBoosted)
        {
            //Timer goes up while boosting
            if(timer <= boostDuration)
            {
                timer += Time.deltaTime;
            }
            //Boosting ends and speed resets
            else
            {
                isBoosted = false;
                _moveSpeed = originalMoveSpeed;
            }
        }
        //boost refresh timer starts
        if (!isBoosted && boostRefreshDelay >= Mathf.Epsilon)
        {
            boostRefreshDelay -= Time.deltaTime;
            timer = 0;
        }

        SmoothMovement();
        RotateInDirectionOfInput();
        // RotateTowardsMouse();
        if(MouseFollowEnabled)
        {
            FollowMousePositionOldInput();
            //FollowMousePositionNewInput();
        }
    }

    private void SmoothMovement()
    {
        _smoothMovementInput = Vector2.SmoothDamp(
            _smoothMovementInput,
            _moveInput,
            ref _movementInputSmoothVelocity,
            0.01f);

        //Code was:
        //_rbBody.velocity = _smoothMovementInput * _moveSpeed;
        //This code caused the player to move awkwardly, feel free to play around with it

        _rbBody.velocity = _moveInput * _moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }


    private void FollowMousePositionNewInput()
    {
        pointPos = Camera.main.ScreenToWorldPoint(pointPosition.action.ReadValue<Vector2>());
        
        Vector2 facingDirection = pointPos - _rbHull.position;

        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;

        _rbHull.MoveRotation(angle - 90f);
    }

    private void FollowMousePositionOldInput()
    {
        pointPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 facingDirection = pointPos - _rbHull.position;

        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;

        _rbHull.MoveRotation(angle - 90f);
    }
    private void RotateTowardsMouse()
    {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler (new(0f, 0f, angle + 90f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void RotateInDirectionOfInput()
    {
        if(_moveInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, bodyRotateSpeed * Time.deltaTime);

            _rbBody.MoveRotation(rotation);
        }
    }


    private void OnSecondaryFire()
    {
        _tankDamageSystem.TankToPlayer(_tankDamageSystem.gameObject, true);
    }

    private void Dash()
    {
        if(boostRefreshDelay <= Mathf.Epsilon)
        {
            isBoosted = true;
            _moveSpeed *= 3.5f;
            boostRefreshDelay = startBoostRefreshDelay;
        }
    }
}
