using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class CreateParty : Singleton<CreateParty>, CreatePartyViewInterface {

    public CreatePartyUseCase CreatePartyUseCase { get; private set; }

    [SerializeField]
    private List<CreatePartyChar> createPartyChars;
        
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

    private int CreatePartyCharSelectedIndex { get; set; }

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
        var professions = Profession.BasicProfessions();

        List<Text> profTexts = new List<Text>();
        var prof1 = professionsContainer.GetComponentInChildren<Text>();
        prof1.text = professions[0].Name;
        prof1.GetComponent<ProfessionData>().Profession = professions[0];
        profTexts.Add(prof1);

        for (int i = 1; i <= 8; i++)
        {
            var newProf = Instantiate(prof1, prof1.transform.parent);
            newProf.text = professions[i].Name;
            newProf.GetComponent<ProfessionData>().Profession = professions[i];
            profTexts.Add(newProf);
        }
    }

    public void SetPortraitSelectedForChar(int portrait, int charIndex) {
        createPartyChars[charIndex].PortraitSelected = portrait;
    }

    public void Clear() {
        CreatePartyUseCase.ClearWithDefaultValues();
    }

    public void SelectProfession(Profession profession, Text professionText) {
        
    }

    public void SelectChar(CreatePartyChar toSelect) {
        CreatePartyCharSelectedIndex = createPartyChars.IndexOf(toSelect);
        foreach (var cpc in createPartyChars)
            cpc.IsSelected = cpc == toSelect;
    }
}
