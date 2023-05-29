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
                    textWriter.AddWriter(messageText, messages[i], 0.08f);
                    yield return new WaitForSeconds(7f);
                    isWriting = false;
                }
            isFinished = true;
        }
    }

}
