using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI messageText;
    [SerializeField] TextWriter textWriter;

    private void Start()
    {
        textWriter.AddWriter(messageText, "Welcome to Tank Survivor!", 0.1f);
    }

}
