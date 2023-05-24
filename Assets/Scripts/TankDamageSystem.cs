using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDamageSystem : MonoBehaviour
{
    public static TankDamageSystem Instance;

    [SerializeField] private GameObject _tracksOriginal, _guardsOriginal, _headOriginal, _barrelOriginal;

    [SerializeField] private GameObject _tracksPrefab, _guardsPrefab, _headPrefab, _barrelPrefab;
    public bool Tracks = true, Guards = true, Head = true, Barrel = true;

    [SerializeField] private Material _repairableMat;


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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) { SelectRandomPart(); }
    }

    




    public void SelectRandomPart()
    {
        //There is overlap, think of it as a chance of luck for player
        int x = Random.Range(0, 4);

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
        if (heldPart.CompareTag(brokenPart.tag)) return true;
        else return false;
    }



    private void LosePart(GameObject original, GameObject prefab)
    {
        //Very messy, however when passing a bool through LosePart()  - when trying to change it, it changes a newly created bool, not the one that was passed through, i could be stupid, but i'm too tired at this point
        if(original == _tracksOriginal)
        {
            if (!Tracks) return;
            else Tracks = false;
        }
        else if(original == _guardsOriginal)
        {
            if (!Guards) return;
            else Guards = false;
        }
        else if(original == _barrelOriginal)
        {
            if (!Barrel) return;
            else Barrel = false;
        }
        else if(original == _headOriginal)
        {
            if (!Head) return;
            else Head = false;
        }






        original.GetComponent<MeshRenderer>().material = _repairableMat;
        original.GetComponent<BoxCollider>().enabled = false;

        Vector3 explosionOffset = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(0.1f, 3f), Random.Range(-8.0f, 8.0f));

        Instantiate(prefab, original.transform.position, original.transform.rotation).AddComponent<Rigidbody>().AddExplosionForce(850f, original.transform.position - explosionOffset, 100f, 1f);

    }



    public void RepairPart(GameObject original, GameObject prefab, bool _bool)
    {
        _bool = true;
        original.GetComponent<MeshRenderer>().material = prefab.GetComponent<MeshRenderer>().material;
        original.GetComponent<BoxCollider>().enabled = true;

    }
}
