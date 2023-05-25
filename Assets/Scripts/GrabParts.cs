using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GrabParts : MonoBehaviour
{

    private RaycastHit _hitInfo;
    private bool _isHolding = false;
    private GameObject _heldPart;

    private void OnFire()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hitInfo, 5.0f))
        {
            if (_hitInfo.transform.CompareTag("TankBarrel") ||
                _hitInfo.transform.CompareTag("TankTracks") ||
                _hitInfo.transform.CompareTag("TankWheelGuard") ||
                _hitInfo.transform.CompareTag("TankHead"))
            {

                _hitInfo.transform.TryGetComponent<BoxCollider>(out BoxCollider boxCollider);

                if (!_isHolding && _hitInfo.transform.TryGetComponent<Rigidbody>(out _))
                {
                    PickupPart();
                }
                else if(_isHolding && boxCollider.isTrigger)
                {
                    RepairPart();
                }
            }
        }
    }



    private void PickupPart()
    {
        _isHolding = true;

        _heldPart = _hitInfo.transform.gameObject;

        Destroy(_heldPart.GetComponent<Rigidbody>());
        Destroy(_heldPart.GetComponent<BoxCollider>());

        _hitInfo.transform.SetParent(transform);
    }



    private void RepairPart()
    {
        if(!TankDamageSystem.Instance.CanRepair(_heldPart, _hitInfo.transform.gameObject)) 
        {
            return;
        }

        //Nees to make it a progress bar so player has to hold a button for 30 seconds
        //Head to comment out the line below because it was causing a comp error
        //It needs the original and the prefab
        TankDamageSystem.Instance.RepairPart( _hitInfo.transform.gameObject, _heldPart);

        _isHolding = false;

        Destroy(_heldPart);
    }
}
