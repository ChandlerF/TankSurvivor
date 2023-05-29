using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] popUps;

    int popUpIndex;
    bool isIntro;
    private void Update()
    {
        for(int i = 0; i < popUps.Length; i++)
        {
            if( i == popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }

        if(popUpIndex == 0)
        {
            if(isIntro)
            {
                StartCoroutine(Intro());
            }
        }
    }

    IEnumerator Intro()
    {
        isIntro = true;
        yield return new WaitForSeconds(1);
    }
}
