using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardController : MonoBehaviour
{

    public string stringToEdit = "Hello World";
    private TouchScreenKeyboard keyboard;


    // Opens native keyboard
    void OnGUI()
    {
        stringToEdit = GUI.TextField(new Rect(10, 10, 200, 30), stringToEdit, 30);

        if (GUI.Button(new Rect(10, 50, 200, 100), "Default"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }

    }
}