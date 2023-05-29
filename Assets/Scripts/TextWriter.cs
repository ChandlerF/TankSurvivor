using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    private TextWriterSingle textWriterSingle;

    public void AddWriter(TextMeshProUGUI text, string textToWrite, float timePerCharacter)
    {
        textWriterSingle = new(text, textToWrite, timePerCharacter);
    }
    private void Update()
    {
        if(textWriterSingle != null)
        {
            textWriterSingle.Update();
        }
    }
    /*Represents a single TextWriter instance
     * */

    public class TextWriterSingle
    {
        TextMeshProUGUI _text;
        string _textToWrite;
        int characterIndex;
        float timePerCharacter;
        float timer;
        public bool isFinished;
        public TextWriterSingle(TextMeshProUGUI text, string textToWrite, float timePerCharacter)
        { 
        _text = text;
        _textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
        }
        public void Update()
        {
            if (_text != null)
            {
                timer -= Time.deltaTime;
                while (timer <= 0f)
                {
                    timer += timePerCharacter;
                    characterIndex++;
                    _text.text = _textToWrite.Substring(0, characterIndex);

                    if (characterIndex >= _textToWrite.Length)
                    {
                        _text = null;
                        return;
                    }
                }
            }
        }
    }
}
