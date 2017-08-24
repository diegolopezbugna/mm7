using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

// TODO: remove, use lamda as NpcDialog
public class TopicData : MonoBehaviour {

    public Npc Npc { get; set; }
    public NpcTopic NpcTopic { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // remove, use lamda as NpcDialog
    public void TopicClicked() {
        VideoBuildingUI.Instance.OnTopicClicked(Npc, NpcTopic);
    }

}
