using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MachineGunFire : MonoBehaviour
{
    [Header("--- Components ---")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform shootPos;
    [SerializeField] InputActionReference _shoot;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip shootAud;
    [SerializeField] AudioClip burnoutSound;
    [SerializeField] GameObject muzzleFlash;

    [Header("--- Projectile Movement ---")]
    [SerializeField] float projectileSpeed;

    [Header("Projectile Stats")]
    [SerializeField, Range(0, 1)] float shootRate;
    [SerializeField] int damage;
    [SerializeField] float burnoutTime = 5f;

    [Header("UI")]
    [SerializeField] Slider slider;
    [SerializeField] Image burnoutFill;
    [SerializeField] bool isTimerGun;

    bool isShooting;
    bool held;
    bool isBurnt;
    float timer = 0;

    Color burnoutFillDefaultColor;
    private void Start()
    {
        slider.maxValue = burnoutTime;
        UpdateUI(isTimerGun);
        burnoutFillDefaultColor = burnoutFill.color;

    }
/*    void OnEnable()
    {
        _shoot.action.performed += Spawn;
    }

    void OnDisable()
    {
        _shoot.action.performed -= Spawn;
    }*/
    private void Update()
    {
        if(Input.GetMouseButton(1) && !isBurnt)
        {
            if (!isShooting)
            {
                StartCoroutine(Shoot());
            }
            BurnoutCountdown();
            UpdateUI(isTimerGun);
        }
        if(isBurnt || (!isBurnt && !Input.GetMouseButton(1)) && timer >= Mathf.Epsilon)
        {
            if(!isShooting)
            {
                StartCoroutine(Shoot());
            }
            BurnoutReset();
            UpdateUI(isTimerGun);
        }

/*        if(held && !isBurnt)
        {
            BurnoutCountdown();
            UpdateUI(isTimerGun);
        }
        if ((isBurnt || (!isBurnt && !held)) && timer >= Mathf.Epsilon)
        {
            BurnoutReset();
            UpdateUI(isTimerGun);
        }*/
    }

    private void BurnoutCountdown()
    {
        timer += Time.deltaTime;
        if (timer >= burnoutTime - Mathf.Epsilon)
        {
            burnoutFill.color = Color.yellow;
            isBurnt = true;
            if (isTimerGun)
            {
                aud.PlayOneShot(burnoutSound);
            }
        }
    }

    private void BurnoutReset()
    {
        timer -= Time.deltaTime;
        if (timer <= Mathf.Epsilon)
        {
            burnoutFill.color = burnoutFillDefaultColor;
            isBurnt = false;
            if(held)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private void Spawn(InputAction.CallbackContext obj)
    {
        held = !held;
        if (!isShooting && held)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        while (Input.GetMouseButton(1) && !isBurnt)
        {
            isShooting = true;
            muzzleFlash.SetActive(true);
            aud.PlayOneShot(shootAud, 0.3f);
            GameObject projectileClone = Instantiate(projectilePrefab.gameObject, shootPos.position, shootPos.rotation);
            Projectile projectile = projectileClone.GetComponent<Projectile>();

            projectileClone.TryGetComponent<Rigidbody2D>(out Rigidbody2D body);
            projectile.SetDamage(damage);

            if (body != null)
            {
                body.velocity = shootPos.transform.up * projectileSpeed;
            }
            yield return new WaitForSeconds(0.1f);
            muzzleFlash.SetActive(false);
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    void UpdateUI(bool _isTimerGun)
    {
        if(_isTimerGun)
        {
            slider.value = timer;
        }
        else
        {
            return;
        }
    }
}
