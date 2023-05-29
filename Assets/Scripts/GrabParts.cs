using UnityEngine;

public class GrabParts : MonoBehaviour
{

    private RaycastHit _hitInfo;
    private bool _isHolding = false;
    private GameObject _heldPart;
    [SerializeField] private GameObject _holdPos;   

    private void OnFire()
    {
        if (Physics.SphereCast(Camera.main.transform.position, 0.7f, Camera.main.transform.forward, out _hitInfo, 6.0f))
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

    private void OnSecondaryFire()
    {
        if (Physics.SphereCast(Camera.main.transform.position, 0.7f, Camera.main.transform.forward, out _hitInfo, 6.0f))
        {
            {

                TankDamageSystem.Instance.TankToPlayer(TankDamageSystem.Instance.TankMovement,  false);

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
        if(!TankDamageSystem.Instance.CanRepair(_heldPart, _hitInfo.transform.gameObject)) 
        {
            return;
        }

        //Progress Bar?
        TankDamageSystem.Instance.RepairPart( _hitInfo.transform.gameObject, _heldPart);

        _isHolding = false;
    }
}
