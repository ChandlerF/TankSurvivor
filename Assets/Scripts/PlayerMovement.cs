using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0.01f, 5f)] float _moveSpeed = 1f;
    [SerializeField] InputActionReference pointPosition;
    [SerializeField] Transform tankBody;
    [SerializeField] Transform tankHull;
    [SerializeField] float bodyRotateSpeed;

    private Vector2 pointPos;
    private Vector2 _moveInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private Rigidbody2D _rbBody;
    private Rigidbody2D _rbHull;


    private void Start()
    {
        _rbBody = tankBody.GetComponent<Rigidbody2D>();
        _rbHull = tankHull.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        SmoothMovement();
        RotateInDirectionOfInput();

        // RotateTowardsMouse();

        FollowMousePosition();
    }

    private void SmoothMovement()
    {
        _smoothMovementInput = Vector2.SmoothDamp(
            _smoothMovementInput,
            _moveInput,
            ref _movementInputSmoothVelocity,
            0.01f);

        //Code was:
        _rbBody.velocity = _smoothMovementInput * _moveSpeed;
        //This code caused the player to move awkwardly, feel free to play around with it

        //_rbBody.velocity = _moveInput * _moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }


    private void FollowMousePosition()
    {
        pointPos = Camera.main.ScreenToWorldPoint(pointPosition.action.ReadValue<Vector2>());
        
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
}
