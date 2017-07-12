using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateParty : MonoBehaviour {

    [SerializeField]
    private CreatePartyChar[] CreatePartyChars;
        
    [SerializeField]
    private GameObject SkillsContainer;

    [SerializeField]
    private GameObject ProfessionsContainer;

	// Use this for initialization
	void Start () {
        CreateSkills();
        CreateProfessions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateSkills() {
        List<Text> skillsTexts = new List<Text>();
        var skill1 = SkillsContainer.GetComponentInChildren<Text>();
        skillsTexts.Add(skill1);

        for (int i = 1; i <= 8; i++)
        {
            var newSkill = Instantiate(skill1, skill1.transform.parent);
            newSkill.text = i.ToString();
            skillsTexts.Add(newSkill);
        }
    }

    void CreateProfessions() {
        List<Text> skillsTexts = new List<Text>();
        var skill1 = ProfessionsContainer.GetComponentInChildren<Text>();
        skillsTexts.Add(skill1);

        for (int i = 1; i <= 8; i++)
        {
            var newSkill = Instantiate(skill1, skill1.transform.parent);
            newSkill.text = i.ToString();
            skillsTexts.Add(newSkill);
        }
    }
        
}
