using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagesScroller : Singleton<MessagesScroller> {

    private ScrollRect messagesScrollView;
    private Text textTemplate;

	// Use this for initialization
	void Start () {
        messagesScrollView = GetComponent<ScrollRect>();
        textTemplate = messagesScrollView.content.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddMessage(string message) {
        Debug.Log(message);
        var newText = Instantiate(textTemplate, messagesScrollView.content.transform);
        newText.text = message;
        Canvas.ForceUpdateCanvases();
        messagesScrollView.verticalNormalizedPosition = 0;
    }
}
