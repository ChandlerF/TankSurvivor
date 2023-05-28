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
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] GameObject explosionFX;
    [SerializeField] SpriteRenderer[] playerSprites;


    int HPtoBeCompared;
    int numPartsLost = 0;
    const int MAX_PARTS = 4;
    bool isDead, lostPart;

    void Start()
    {
        for (int i = 0; i < playerSprites.Length; i++)
        {
            if (!playerSprites[i].enabled)
            {
                playerSprites[i].enabled = true;
            }
        }
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
            for(int i = 0; i < playerSprites.Length; i++)
            {
                playerSprites[i].enabled = false;
            }
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
        aud.PlayOneShot(explosionAudio, 0.4f);
        explosionFX.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.LoseMenu();
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
