using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class ProfessionData : MonoBehaviour {

    public Profession Profession { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreatePartyCharOnClick() {
        this.transform.parent.parent.GetComponent<CreatePartyChar>().Profession = Profession;
    }
}
