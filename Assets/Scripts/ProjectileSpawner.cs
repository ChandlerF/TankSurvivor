using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("--- Components ---")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] InputActionReference _shoot;

    [Header("--- Projectile Movement ---")]
    [SerializeField] float projectileSpeed;


    void OnEnable()
    {
        _shoot.action.performed += Spawn;
    }

    void OnDisable()
    {
        _shoot.action.performed -= Spawn;
    }

    private void Spawn(InputAction.CallbackContext obj)
    {
        GameObject projectileClone = Instantiate(projectilePrefab.gameObject, shootPos.position, projectilePrefab.gameObject.transform.rotation);
        projectileClone.TryGetComponent<Rigidbody2D>(out Rigidbody2D body);
        if(body != null)
        {
            body.velocity = transform.up * projectileSpeed;
        }
    }
}
