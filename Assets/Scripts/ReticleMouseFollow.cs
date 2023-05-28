using UnityEngine;
using UnityEngine.InputSystem;

public class ReticleMouseFollow : MonoBehaviour
{
    [SerializeField] InputActionReference _reticlePosition;

    Vector3 reticlePos;
    Vector3 reticleWorldPos;
    Camera mainCam;
    private void Awake()
    {
        mainCam = Camera.main;
        Cursor.visible = false;
    }
    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        //Grab mouse/right stick/touch position based on camview
        reticlePos = _reticlePosition.action.ReadValue<Vector2>();
        //backoff the z for 2D
        reticlePos.z = mainCam.nearClipPlane;
        //Get the world spot
        reticleWorldPos = mainCam.ScreenToWorldPoint(reticlePos);
        //move the reticle
        transform.position = reticleWorldPos;

    }
}
