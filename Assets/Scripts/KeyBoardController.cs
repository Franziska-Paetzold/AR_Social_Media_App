using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardController : MonoBehaviour
{

    private string textPostContent = "";
    private TouchScreenKeyboard keyboard;
  

    // Opens native keyboard
    void OnGUI()
    {
        //GUIStyle kindOfPostButtonStyle = new GUIStyle("kindOfPostButtonStyle");
        //kindOfPostButtonStyle.fontSize = 20;
        //bool textPostButton = GUI.Button(new Rect(10, Screen.height - 200, 200, 200), "A", kindOfPostButtonStyle);
        bool textPostButton = GUI.Button(new Rect(10, Screen.height - 500, 200, 200), "A");

                                 
        if (textPostButton)
        {
            keyboard = TouchScreenKeyboard.Open(textPostContent);
            TouchScreenKeyboard.hideInput = true;
            textPostContent = GUI.TextField(new Rect(Screen.width/2, Screen.height/2, 200, 200), textPostContent);
        }

        if (keyboard != null)
        {
            textPostContent = keyboard.text;
        }


    }
}
