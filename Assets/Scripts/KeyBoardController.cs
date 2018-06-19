using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardController : MonoBehaviour
{

    public string textPostContent = "Hello World";
    private TouchScreenKeyboard keyboard;


    // Opens native keyboard
    void OnGUI()
    {
        textPostContent = GUI.TextField(new Rect(10, 10, 200, 30), textPostContent);

        if (GUI.Button(new Rect(10, 50, 200, 100), "Text post"))
        {
            keyboard = TouchScreenKeyboard.Open(textPostContent);
        }

        if (keyboard != null)
        {
            textPostContent = keyboard.text;
        }


    }
}
