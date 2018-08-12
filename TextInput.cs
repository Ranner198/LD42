using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour {

    public InputField input;

    public string text;

    private void Start()
    {
        input = GetComponent<InputField>();
        input.text = "Stack.";
    }

    void Update () {

        KeyCode inserted;

        if (Input.anyKeyDown)
        {
            //inserted = KeyCode.Insert;
            //var temp = inserted;
        }
        if (input.caretPosition < 6) {
            input.caretPosition = 6;
        }

        if (input.text.Length < 5)
            input.text = "Stack.";
        input.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Stack.push() pop()";
    }
}
