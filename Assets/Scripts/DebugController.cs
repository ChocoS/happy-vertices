using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour {

    private List<string> texts = new List<string>();

    void LateUpdate() {
        GetComponent<Text>().text = "";
        foreach (string text in texts) {
            GetComponent<Text>().text += text + "\n";
        }
        texts.Clear();
    }

    public void AddText(string text) {
        texts.Add(text);
    }
}