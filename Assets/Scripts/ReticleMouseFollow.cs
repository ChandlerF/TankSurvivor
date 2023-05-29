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
        if(TankDamageSystem.Instance.Head && TankDamageSystem.Instance.Barrel)
        {
            FollowMouseOldInput();
        }
        //FollowMouseNewInput();
    }

    private void FollowMouseNewInput()
    {
        //Grab mouse/right stick/touch position based on camview
        reticlePos = _reticlePosition.action.ReadValue<Vector2>();
        
        //Get the world spot
        reticleWorldPos = mainCam.ScreenToWorldPoint(reticlePos);
        //backoff the z for 2D
        reticleWorldPos.z = mainCam.nearClipPlane;
        //move the reticle
        transform.position = reticleWorldPos;
        Debug.Log("Mouse Pos: " + reticlePos);

    }

    void FollowMouseOldInput()
    {
        Vector3 reticlePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        reticlePos.z = 0f;
        transform.position = reticlePos;
    }


    /*
    _renderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("CheckCamMode", 0, 2f);
    private void CheckCamMode()
    {

        if (!mainCam.orthographic)
        {
            _renderer.enabled = false;
            return;
        }
        else
        {
            _renderer.enabled = true;
        }
    }*/
}
