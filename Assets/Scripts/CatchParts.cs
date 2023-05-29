using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchParts : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("FpsPlayer"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1.2f);
        }
        else
        {
            Rigidbody rb = col.transform.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.freezeRotation = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.6f);
        }

    }
}
