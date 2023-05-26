using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamage
{
    [Header("Player Stats")]
    [SerializeField] int HP = 50;
    [SerializeField, Range(0, 0.99f)] float partsLossIncrementAmount;

    [Header("Components")]
    [SerializeField] Slider slider;

    int HPtoBeCompared;
    int numPartsLost = 0;
    const int MAX_PARTS = 4;
    bool isDead, lostPart;

    void Start()
    {
        HPtoBeCompared = HP;
        SetMaxHealth(HP);
    }

    public void TakeDamage(int amount)
    {
        if(HP > 0)
        {
            HP -= amount;
            SetHealth(HP);
            CinemachineShake.Instance.ShakeCamera(1f, 0.5f);
        }
        else if (HP <= 0 && !isDead)
        {
            SetHealth(HP);
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

    IEnumerator OnDead()
    {
        isDead = true;
        //TODO fun blowy up stuff and a lose screen
        yield return new WaitForSeconds(1f);
    }

    void ProcessLosingParts()
    {
        TankDamageSystem.Instance.SelectRandomPart();
    }

    void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    void SetHealth(int health)
    {
        slider.value = health;
    }
}
