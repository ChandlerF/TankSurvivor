using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    TextMeshProUGUI _text;
    string _textToWrite;
    int characterIndex;
    float timePerCharacter;
    float timer;

    public void AddWriter(TextMeshProUGUI text, string textToWrite, float timePerCharacter)
    {
        _text = text;
        _textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
    }
    private void Update()
    {
        if(_text != null)
        {
            timer -= Time.deltaTime;
            while(timer <= 0f )
            {
                timer += timePerCharacter;
                characterIndex++;
                _text.text = _textToWrite.Substring(0, characterIndex);

                if(characterIndex >= _textToWrite.Length)
                {
                    _text = null;
                    return;
                }
            }
        }
    }
}
