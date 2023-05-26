using System.Collections;
using UnityEngine;
[RequireComponent(typeof(WaypointMover))]
public class EnemyTankAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform shootPosParent;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip shootAudio;
    [SerializeField] WaypointMover waypoints;

    [Header("Enemy Stats")]
    [SerializeField] float bulletSpeed;
    [SerializeField] float shootRate;

    GameObject player;
    bool isShooting;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if(!waypoints.IsFollowingWaypoints)
        {
            LookAt();
            if(!isShooting)
            {
                StartCoroutine(ShootPlayer());
            }
        }
    }
    void LookAt()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        shootPosParent.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    IEnumerator ShootPlayer()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().velocity = shootPosParent.up * bulletSpeed;
        aud.PlayOneShot(shootAudio, 0.1f);
        yield return new WaitForSeconds(shootRate);
        Projectile bulletInfo = bulletClone.GetComponent<Projectile>();
        bulletInfo.SetShooter(transform);
        isShooting = false;
    }
}
