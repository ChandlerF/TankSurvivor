using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleMouseFollow : MonoBehaviour
{
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
        //Grab mouse position based on camview
        Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        //backoff the z since plane will be on z = 0
        mouseWorldPosition.z = 0f;
        //move the reticle
        transform.position = mouseWorldPosition;
    }
}
