using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankDamageSystem : MonoBehaviour
{
    public static TankDamageSystem Instance;
    public event Action SwitchedPerspective;
    [SerializeField] private GameObject _tracksOriginal, _guardsOriginal, _headOriginal, _barrelOriginal;
    [SerializeField] private SpriteRenderer[] _tank2D;
    [SerializeField] private MachineGunFire[] _gunFire;
    [SerializeField] private GameObject _tracksPrefab, _guardsPrefab, _headPrefab, _barrelPrefab;


    public bool Tracks = true, Guards = true, Head = true, Barrel = true;

    [SerializeField] private Material _repairableMat;
    [SerializeField] private PlayerInput _tankMovement, _playerMovement;
    [SerializeField] private GameObject _tankModel, _desertMap3D, _playerFPS, _reticle, _trailRenderer, _tankGraveyard;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private PlayerMovement _tankMovementOrtho;
    public GameObject TankMovement;



   private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        TankMovement = _tankMovement.gameObject;
    }




    public void SelectRandomPart()
    {
        //There is overlap, think of it as a chance of luck for player
        int x = UnityEngine.Random.Range(0, 4);

        if(x == 0)
        {
            LosePart(_tracksOriginal, _tracksPrefab);
        }
        else if(x == 1)
        {
            LosePart(_guardsOriginal, _guardsPrefab);
        } 
        else if(x == 2)
        {
            LosePart(_headOriginal, _headPrefab);
        } 
        else if(x== 3) 
        {
            LosePart(_barrelOriginal, _barrelPrefab);
        }
    }

   
    public bool CanRepair(GameObject heldPart, GameObject brokenPart)
    {
        if (heldPart.CompareTag(brokenPart.tag) || heldPart.CompareTag("TankHull")) return true;
        else return false;
    }



    private void LosePart(GameObject original, GameObject prefab)
    {
        if (original == _tracksOriginal)
        {
            if (!Tracks) return;
        }
        else if (original == _guardsOriginal)
        {
            if (!Guards) return;
        }
        else if (original == _barrelOriginal)
        {
            if (!Barrel) return;
        }
        else if (original == _headOriginal)
        {
            if (!Head) return;
        }

        SetBool(prefab, false);


        original.GetComponent<MeshRenderer>().material = _repairableMat;
        original.GetComponent<BoxCollider>().isTrigger = true;

        GameObject spawnedPart = Instantiate(prefab, original.transform.position, original.transform.rotation);
        Rigidbody spawnedRB = spawnedPart.AddComponent<Rigidbody>();

        float bounds = spawnedPart.GetComponent<BoxCollider>().size.y;
        Vector3 explosionOffset = new Vector3(UnityEngine.Random.Range(-8.0f, 8.0f), 
            UnityEngine.Random.Range(bounds, bounds*1.5f), UnityEngine.Random.Range(-8.0f, 8.0f));

        spawnedRB.AddExplosionForce(400f, original.transform.position - explosionOffset, 100f, 1f);
        spawnedRB.useGravity = false;
        spawnedPart.AddComponent<TankPartsGravity>();

    }



    public void RepairPart(GameObject original, GameObject prefab)
    {
        SetBool(prefab, true);
        original.GetComponent<MeshRenderer>().material = prefab.GetComponent<MeshRenderer>().material;
        original.GetComponent<BoxCollider>().isTrigger = false;
        //ReplacedPart?.Invoke()
        Destroy(prefab);
    }


    private void SetBool(GameObject obj, bool setBool)
    {
        if(obj.CompareTag(_tracksPrefab.tag))
        {
            Tracks = setBool;
        }
        else if(obj.CompareTag(_guardsPrefab.tag))
        {
            Guards = setBool;
        }
        else if(obj.CompareTag(_barrelPrefab.tag))
        {
            Barrel = setBool;
        }
        else if(obj.CompareTag(_headPrefab.tag))
        {
            Head = setBool;
        }
    }


    public void TankToPlayer(GameObject cameraFollow, bool _bool)
    {
        //Player Input Scripts
        _tankMovement.enabled = !_bool;
        _playerMovement.enabled = _bool;
        PlayerMovement.MouseFollowEnabled = !_bool;
        SwitchedPerspective?.Invoke();
        //Tank Shooting Script
        _projectileSpawner.enabled = !_bool;

        //3D Models, Reticle, and Buildings
        _tankModel.SetActive(_bool);
        _desertMap3D.SetActive(_bool);
        _reticle.SetActive(!_bool);
        _trailRenderer.SetActive(!_bool);
        _tankGraveyard.SetActive(!_bool);


        for (int i = 0; i < _tank2D.Length; i++)
        {
            _tank2D[i].enabled = !_bool;
        }
        for(int i = 0; i < _gunFire.Length; i++)
        {
            _gunFire[i].enabled = !_bool;
        }


        //Change perspective of camera to go into first person
        Camera.main.orthographic = !_bool;

        //Cinemachine, this is un-needed as the priority on the FPS virtual camera is higher
        if(_bool) _virtualCamera.Priority += 2;
        else _virtualCamera.Priority -= 2;

        //Player FPS postion and active
        _playerFPS.SetActive(_bool);
        _playerFPS.transform.localPosition = new Vector3(0, 0, 1);
    }
}
