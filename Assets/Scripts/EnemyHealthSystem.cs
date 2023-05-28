using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] GameObject explosionFX;
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
        }
        if(HP <= 0 && !isDead)
        {
            StartCoroutine(OnDead());
        }
    }

    IEnumerator OnDead()
    {
        isDead = true;
        bodyRenderer.enabled = false;
        hullRenderer.enabled = false;
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
}
