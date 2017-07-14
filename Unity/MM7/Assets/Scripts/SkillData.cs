using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class SkillData : MonoBehaviour {

    public Skill Skill { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreatePartyOnClick() {
        if (Skill != null)
            CreateParty.Instance.AddSkill(Skill);
    }
        
    public void CreatePartyCharOnClick() {
        if (Skill != null)
        {
            this.transform.parent.parent.GetComponent<CreatePartyChar>().RemoveSkill(Skill);
        }
    }
}
