using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] GameObject explosionFX;
    [SerializeField] ParticleSystem[] damageFX;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip explosion;
    [SerializeField] SpriteRenderer bodyRenderer;
    [SerializeField] SpriteRenderer hullRenderer;

    [Header("Stats")]
    [SerializeField, Range(0, 50)] int HP = 15;
    [SerializeField] bool isBoss;

    bool isDead;
    public bool IsDead
    {
        get { return isDead; }
    }
    public void TakeDamage(int amount)
    {
        if(!isDead)
        {
            HP -= amount;
            StartCoroutine(ProcessDamage());
        }
        if(HP <= 0 && !isDead)
        {
            StartCoroutine(OnDead());
        }
    }
    private void Start()
    {
        TankDamageSystem.Instance.SwitchedPerspective += TankDamage_SwitchedPerspective;
    }

    private void TankDamage_SwitchedPerspective()
    {
        if(bodyRenderer != null && hullRenderer != null)
        {
            bodyRenderer.enabled = PlayerMovement.MouseFollowEnabled;

            transform.GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr);
            sr.enabled = PlayerMovement.MouseFollowEnabled;

            transform.GetChild(0).GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr1);
            sr1.enabled = PlayerMovement.MouseFollowEnabled;

            transform.GetChild(0).GetChild(0).GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr2);
            sr2.enabled = PlayerMovement.MouseFollowEnabled;

            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr3);
            sr3.enabled = PlayerMovement.MouseFollowEnabled;
        }
    }

    IEnumerator OnDead()
    {
        TankDamageSystem.Instance.SwitchedPerspective -= TankDamage_SwitchedPerspective;
        isDead = true;
        bodyRenderer.enabled = false;

        transform.GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr);
        sr.enabled = false;

        transform.GetChild(0).GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr1);
        sr1.enabled = false;

        transform.GetChild(0).GetChild(0).GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr2);
        sr2.enabled = false;

        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer sr3);
        sr3.enabled = false;


        //hullRenderer.enabled = false;
        explosionFX.SetActive(true);
        aud.PlayOneShot(explosion);
        yield return new WaitForSeconds(1f);
        if(isBoss)
        {
            GameManager.Instance.BossDied();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ProcessDamage()
    {
        int damageFXToPlay = Random.Range(0, damageFX.Length - 1);
        damageFX[damageFXToPlay].Play();
        damageFX[damageFXToPlay + 1].Play();
        yield return new WaitForSeconds(0.25f);
        damageFX[damageFXToPlay].Stop();
        damageFX[damageFXToPlay + 1].Stop();
    }
}
