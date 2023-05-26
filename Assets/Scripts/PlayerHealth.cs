using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamage
{
    [Header("Player Stats")]
    [SerializeField] int HP = 50;
    [SerializeField, Range(0, 0.99f)] float partsLossIncrementAmount;

    int HPtoBeCompared;
    int numPartsLost = 0;
    const int MAX_PARTS = 4;
    bool isDead, lostPart;
    void Start()
    {
        HPtoBeCompared = HP;
    }

    public void TakeDamage(int amount)
    {
        if(HP > 0)
        {
            HP -= amount;
            CinemachineShake.Instance.ShakeCamera(1f, 0.5f);
        }
        else if (HP <= 0 && !isDead)
        {
            CinemachineShake.Instance.ShakeCamera(1f, 0.5f);
            StartCoroutine(OnDead());
        }
        //Here if the incremental damage set in the inspector has been satisfied
        //The player will lose a part
        if (HP <= HPtoBeCompared - (HPtoBeCompared * partsLossIncrementAmount) && 
            !lostPart && !isDead && numPartsLost < MAX_PARTS)
        {
            StartCoroutine(LoseAPart());
        }
    }

    IEnumerator LoseAPart()
    {
        lostPart = true;
        numPartsLost++;
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
