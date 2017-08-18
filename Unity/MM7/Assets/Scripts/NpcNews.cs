using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infrastructure;

public class NpcNews : MonoBehaviour {

    [SerializeField]
    private string npcTypeLocKey;

    [SerializeField]
    private string newsLocKey;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string GetDescription() {
        return Localization.Instance.Get(npcTypeLocKey);
    }

    public void Talk() {
        NpcDialog.Instance.ShowNews(Localization.Instance.Get(newsLocKey));
    }

}
