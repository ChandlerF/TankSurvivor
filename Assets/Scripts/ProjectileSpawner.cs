using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("--- Components ---")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform shootPosParent;
    [SerializeField] InputActionReference _shoot;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip shootAud;
    [SerializeField] GameObject muzzleFlash;

    [Header("--- Projectile Movement ---")]
    [SerializeField] float projectileSpeed;

    [Header("Projectile Stats")]
    [SerializeField] int shootRate;
    [SerializeField] int damage;

    bool isShooting;

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
        if(!isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        CinemachineShake.Instance.ShakeCamera(1.5f, 0.25f);
        muzzleFlash.SetActive(true);
        aud.PlayOneShot(shootAud, 0.3f);
        GameObject projectileClone = Instantiate(projectilePrefab.gameObject, shootPos.position, shootPos.rotation);
        Projectile projectile = projectileClone.GetComponent<Projectile>();

        projectileClone.TryGetComponent<Rigidbody2D>(out Rigidbody2D body);
        projectile.SetDamage(damage);

        if (body != null)
        {
            body.velocity = shootPosParent.transform.up * projectileSpeed;
        }
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
