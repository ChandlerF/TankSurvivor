using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject _tracksOriginal, _guardsOriginal, _headOriginal, _barrelOriginal;

    [SerializeField] private GameObject _tracksPrefab, _guardsPrefab, _headPrefab, _barrelPrefab;
    public bool Tracks = true, Guards = true, Head = true, Barrel = true;

    [SerializeField] private Material _repairableMat;
    
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) { SelectRandomPart(); }
    }*/

    public void SelectRandomPart()
    {

        //There is overlap
        int x = Random.Range(0, 4);

        if(x == 0)
        {
            LosePart(_tracksOriginal, _tracksPrefab, Tracks);
        }
        else if(x == 1)
        {
            LosePart(_guardsOriginal, _guardsPrefab, Guards);
        } 
        else if(x == 2)
        {
            LosePart(_headOriginal, _headPrefab, Head);
        } 
        else if(x== 3) 
        {
            LosePart(_barrelOriginal, _barrelPrefab, Barrel);
        }
    }







    private void LosePart(GameObject original, GameObject prefab, bool _bool)
    {
        _bool = false;
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
