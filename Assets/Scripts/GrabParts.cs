using UnityEngine;
using System;
using System.Linq;

public class GrabParts : MonoBehaviour
{

    private RaycastHit _hitInfo;
    private bool _isHolding = false;
    private GameObject _heldPart;
    [SerializeField] private GameObject _holdPos;
    [SerializeField] private LayerMask _layerMask;

    private void OnFire()
    {
        RaycastHit[] hit = Physics.SphereCastAll(Camera.main.transform.position, 0.7f, Camera.main.transform.forward, 8.0f, _layerMask, QueryTriggerInteraction.Collide);

        if (hit.Length > 0)
        {
            foreach (RaycastHit i in hit)
            {
                if (i.transform.CompareTag("TankBarrel") ||
                i.transform.CompareTag("TankTracks") ||
                i.transform.CompareTag("TankWheelGuard") ||
                i.transform.CompareTag("TankHead"))
                {
                    _hitInfo = i;
                    i.transform.TryGetComponent<BoxCollider>(out BoxCollider boxCollider);

                    if (!_isHolding && i.transform.TryGetComponent<Rigidbody>(out _))
                    {
                        PickupPart();
                    }
                    else if (_isHolding && boxCollider.isTrigger)
                    {
                        RepairPart();
                    }
                }
            }
        }
    }

    private void OnSecondaryFire()
    {
        RaycastHit[] hit = Physics.SphereCastAll(Camera.main.transform.position, 0.7f, Camera.main.transform.forward, 8.0f, _layerMask, QueryTriggerInteraction.Collide);

        if (hit.Length > 0)
        {
            foreach (RaycastHit i in hit)
            {
                if (i.transform.CompareTag("TankBarrel") ||
                i.transform.CompareTag("TankTracks") ||
                i.transform.CompareTag("TankWheelGuard") ||
                i.transform.CompareTag("TankHead") ||
                i.transform.CompareTag("TankRepair"))

                {

                    TankDamageSystem.Instance.TankToPlayer(TankDamageSystem.Instance.TankMovement, false);
                    return;
                }
            }
        }
    }



        private void PickupPart()
        {
        _isHolding = true;

        _heldPart = _hitInfo.transform.gameObject;

        Destroy(_heldPart.GetComponent<TankPartsGravity>());
        Destroy(_heldPart.GetComponent<Rigidbody>());
        Destroy(_heldPart.GetComponent<BoxCollider>());

        _hitInfo.transform.SetParent(transform);
        _hitInfo.transform.position = _holdPos.transform.position;
        }



    private void RepairPart()
    {
        Debug.Log(_heldPart.tag + _hitInfo.transform.gameObject.tag);
        if(!TankDamageSystem.Instance.CanRepair(_heldPart, _hitInfo.transform.gameObject)) 
        {
            return;
        }

       /* if (!_hitInfo.transform.gameObject.CompareTag("TankRepair"))
        {
            return;
        }*/

        //Progress Bar?
        TankDamageSystem.Instance.RepairPart( _hitInfo.transform.gameObject, _heldPart);
        _isHolding = false;
    }
}
