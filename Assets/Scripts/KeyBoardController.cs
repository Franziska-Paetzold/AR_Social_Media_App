using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardController : MonoBehaviour
{

    public string textPostContent = "";
    private TouchScreenKeyboard manualKeyboard;
    public Text textField;



    // Opens native keyboard
    void OnGUI()
    {
        //GUIStyle kindOfPostButtonStyle = new GUIStyle("kindOfPostButtonStyle");
        //kindOfPostButtonStyle.fontSize = 20;
        //bool textPostButton = GUI.Button(new Rect(10, Screen.height - 200, 200, 200), "A", kindOfPostButtonStyle);
        bool textPostButton = GUI.Button(new Rect(10, Screen.height - 500, 200, 200), "A");


        if (textPostButton)
        {
            manualKeyboard = TouchScreenKeyboard.Open(textPostContent);
            TouchScreenKeyboard.hideInput = true;
            textField.text = textPostContent;
        }

        if (manualKeyboard.active)
        {
            textField.text = textPostContent;
        }

        if (manualKeyboard != null)
        {
            textPostContent = manualKeyboard.text;
        }


    }
}