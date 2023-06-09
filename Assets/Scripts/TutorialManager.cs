using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI messageText;
    [SerializeField] TextWriter textWriter;
    [SerializeField] string[] messages;

    bool isWriting;
    bool isFinished;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    private void Start()
    {
        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        while (!isFinished)
        {
            if(!isWriting)
                for(int i = 0; i < messages.Length; i++)
                {
                    textWriter.AddWriter(messageText, messages[i], 0.04f);
                    yield return new WaitForSeconds(5f);
                    isWriting = false;
                }
            isFinished = true;
        }
    }

}
