using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamage
{
    [Header("Player Stats")]
    [SerializeField] int HP = 50;
    [SerializeField, Range(0, 0.99f)] float partsLossIncrementAmount;

    int HPtoBeCompared;
    bool isDead, lostPart;
    // Start is called before the first frame update
    void Start()
    {
        HPtoBeCompared = HP;
    }

    public void TakeDamage(int amount)
    {
        if(HP > 0)
        {
            HP -= amount;
        }
        else if (HP <= 0 && !isDead)
        {
            StartCoroutine(OnDead());
        }
        if (HP <= (HPtoBeCompared * partsLossIncrementAmount) && !lostPart && !isDead)
        {
            StartCoroutine(LoseAPart());
        }
    }

    IEnumerator LoseAPart()
    {
        lostPart = true;
        ProcessLosingParts();
        HPtoBeCompared = HP;
        yield return new WaitForSeconds(0.5f);
        lostPart = false;
    }

    void ProcessLosingParts()
    {
        TankDamageSystem.Instance.SelectRandomPart();
    }

    IEnumerator OnDead()
    {
        isDead = true;
        //TODO fun blowy up stuff and a lose screen
        yield return new WaitForSeconds(1f);
    }
}
