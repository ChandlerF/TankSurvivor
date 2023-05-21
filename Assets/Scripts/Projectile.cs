using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1, 10)] int damage;

    Action<Projectile> _repoolAction;
    Transform _shooter;
    public void Init(Action<Projectile> repoolAction)
    {
        _repoolAction = repoolAction;
    }
    private void OnTriggerEnter(Collider other)
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
        //Repool the projectile
        _repoolAction(this);
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
}
