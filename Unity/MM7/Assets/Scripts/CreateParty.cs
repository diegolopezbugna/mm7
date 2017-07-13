using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class CreateParty : Singleton<CreateParty>, CreatePartyViewInterface {

    public CreatePartyUseCase CreatePartyUseCase { get; private set; }

    [SerializeField]
    private CreatePartyChar[] createPartyChars;
        
    [SerializeField]
    private GameObject skillsContainer;

    [SerializeField]
    private GameObject professionsContainer;

    [SerializeField]
    private Text bonusPointsText;

    private int _bonusPoints;
    public int BonusPoints
    {
        get { return _bonusPoints; }
        set {
            _bonusPoints = value;
            bonusPointsText.text = value.ToString();
        }
    }

	// Use this for initialization
	void Start () {
        CreateSkills();
        CreateProfessions();
        CreatePartyUseCase = new CreatePartyUseCase(this);
        CreatePartyUseCase.ClearWithDefaultValues();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateSkills() {
        List<Text> skillsTexts = new List<Text>();
        var skill1 = skillsContainer.GetComponentInChildren<Text>();
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
        var skill1 = professionsContainer.GetComponentInChildren<Text>();
        skillsTexts.Add(skill1);

        for (int i = 1; i <= 8; i++)
        {
            var newSkill = Instantiate(skill1, skill1.transform.parent);
            newSkill.text = i.ToString();
            skillsTexts.Add(newSkill);
        }
    }

    public void SetPortraitSelectedForChar(int portrait, int charIndex) {
        createPartyChars[charIndex].PortraitSelected = portrait;
    }

    public void Clear() {
        CreatePartyUseCase.ClearWithDefaultValues();
    }
}
