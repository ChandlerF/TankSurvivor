using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1, 10)] int damage;
    Transform _shooter;

    float objectLifeSpan = 3f;

    private void Start()
    {
        Destroy(gameObject, objectLifeSpan);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //If the collision involves the IDamage interface
        IDamage damageable = other.gameObject.GetComponent<IDamage>();
        //Deal the damage
        damageable?.TakeDamage(damage);
        //If the collision also involves the player
        if(other.gameObject.CompareTag("Player"))
        {
            //Register the shooter's location for UI indicator
            Register();
        }
        gameObject.SetActive(false);
    }

    public void SetShooter(Transform shooter)
    {
        //Used for enemy AI to set their position
        //when shooting
        _shooter = shooter;
    }

    void Register()
    {
        //TODO Create DamageIndicatorSystem that tells user
        //damage direction by registering the shooter of the
        //projectile
    }

    public void SetDamage(int damageAdded)
    {
        damage += damageAdded;
    }
}
