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
    [SerializeField] MachineGunFire[] machineGuns;

    [SerializeField] bool isMachineGunActive = true;
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
        ActivateMachineGuns(isMachineGunActive);
        HPtoBeCompared = HP;
        SetMaxHealth(HP);

        TankDamageSystem.Instance.ReplacedPart += Instance_ReplacedPart;
    }

    private void Update()
    {
        if(!TankDamageSystem.Instance.Tracks)
        {
            playerSprites[1].enabled = false;
        }
        if(!TankDamageSystem.Instance.Head)
        {
            playerSprites[2].enabled = false;
        }
        if(!TankDamageSystem.Instance.Barrel)
        {
            playerSprites[3].enabled = false;
        }
    }
    public void TakeDamage(int amount)
    {
        if(HP > 0)
        {
            if(!TankDamageSystem.Instance.Guards)
            {
                HP -= (amount + 1);
            }
            else
            {
                HP -= amount;
            }
            SetHealth(HP);
            CinemachineShake.Instance.ShakeCamera(1f, 0.5f);
        }
        else if (HP <= 0 && !isDead)
        {
            SetHealth(HP);
            CinemachineShake.Instance.ShakeCamera(1f, 0.5f);
            for (int i = 0; i < playerSprites.Length; i++)
            {
                playerSprites[i].enabled = false;
            }
            ActivateMachineGuns(false);
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

    private void ActivateMachineGuns(bool isActive)
    {
        for (int i = 0; i < machineGuns.Length; i++)
        {
            machineGuns[i].enabled = isActive;
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
        TankDamageSystem.Instance.ReplacedPart -= Instance_ReplacedPart;
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

    void Instance_ReplacedPart()
    {
        
         HP += HPtoBeCompared / 5;
         if(HP >= HPtoBeCompared)
         {
            HP = HPtoBeCompared;
         }
         SetHealth(HP);
    }
}
