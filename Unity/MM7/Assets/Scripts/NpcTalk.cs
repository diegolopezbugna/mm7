using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;

public class NpcTalk : MonoBehaviour {

    [SerializeField]
    private string code;

    private Npc npc;

	// Use this for initialization
	void Start () {
        npc = Npc.GetByCode(code);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string GetDescription() {
        return npc.Name;
    }

    public void Talk() {
        NpcDialog.Instance.Show(npc);
    }

}
