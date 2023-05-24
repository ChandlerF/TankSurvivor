using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour, IDamage
{
    [Header("Turret Stats")]
    [SerializeField, Range(1f, 50f)] float attackDistance;
    [SerializeField, Range(10f, 60f)] float bulletSpeed;
    [SerializeField, Range(0f, 1f)] float shootRate;
    [SerializeField, Range(0.01f, 1f)] float turnSpeed;
    [SerializeField] int HP;

    [Header("Components")]
    [SerializeField] Transform shootPosParent;
    [SerializeField] Animator animator;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject[] particles;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip shootAudio;


    GameObject player;
    Transform playerTransform;

    bool isShooting;
    bool isExploding;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }
    private void LateUpdate()
    {
        StartRotating();
        if (Vector3.Distance(playerTransform.position, transform.position) <= attackDistance)
        {
            if (!isShooting)
            {
                StartCoroutine(ShootPlayer());
            }
        }
    }
    void StartRotating()
    {
        LookAt();
    }

    void LookAt()
    {
        var dir = player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    IEnumerator ShootPlayer()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");
        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().velocity = shootPosParent.forward * bulletSpeed;
        aud.PlayOneShot(shootAudio, 0.1f);
        yield return new WaitForSeconds(shootRate);
        Projectile bulletInfo = bulletClone.GetComponent<Projectile>();
        bulletInfo.SetShooter(transform);
        isShooting = false;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            StartCoroutine(ProcessExplosion());
        }
    }

    IEnumerator ProcessExplosion()
    {
        isExploding = true;
        if (isExploding)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(true);
            }
            if (!aud.isPlaying)
            {
                aud.PlayOneShot(explosion, 0.2f);
            }
            yield return new WaitForSeconds(0.5f);
            isExploding = false;
        }
        this.gameObject.SetActive(false);
    }
}
