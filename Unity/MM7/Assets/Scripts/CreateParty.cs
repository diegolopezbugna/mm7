using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using System.Linq;

public class CreateParty : Singleton<CreateParty>, CreatePartyViewInterface
{

    public CreatePartyUseCase CreatePartyUseCase { get; private set; }

    [SerializeField]
    private List<CreatePartyChar> createPartyChars;
        
    [SerializeField]
    private GameObject skillsContainer;

    [SerializeField]
    private GameObject professionsContainer;

    [SerializeField]
    private Text bonusPointsText;

    public Color YellowSelectedColor = new Color(255f / 255f, 240f / 255f, 41f / 255f);

    private int _bonusPoints;

    public int BonusPoints
    {
        get { return _bonusPoints; }
        set
        {
            _bonusPoints = value;
            bonusPointsText.text = value.ToString();
        }
    }

    private int CreatePartyCharSelectedIndex { get; set; }

    // Use this for initialization
    void Start()
    {
        CreateProfessions();
        CreatePartyUseCase = new CreatePartyUseCase(this);
        CreatePartyUseCase.ClearWithDefaultValues();
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

    void CreateProfessions()
    {
        var professions = Profession.All();

        var prof1 = professionsContainer.GetComponentInChildren<Text>();
        prof1.text = professions[0].Name;
        prof1.GetComponent<ProfessionData>().Profession = professions[0];

        for (int i = 1; i <= 8; i++)
        {
            var newProf = Instantiate(prof1, prof1.transform.parent);
            newProf.text = professions[i].Name;
            newProf.GetComponent<ProfessionData>().Profession = professions[i];
        }
    }

    public void SetPortraitSelectedForChar(int portrait, int charIndex)
    {
        createPartyChars[charIndex].PortraitSelected = portrait;
    }

    public void Clear()
    {
        CreatePartyUseCase.ClearWithDefaultValues();
    }

    public void SetProfessionForChar(Profession profession, int charIndex)
    {
        createPartyChars[charIndex].Profession = profession;
    }

    public void SelectProfession(Profession profession)
    {
        SetProfessionForChar(profession, CreatePartyCharSelectedIndex);
        SetProfessionTextSelected(profession);
    }

    private void SetProfessionTextSelected(Profession profession)
    {
        foreach (var txt in professionsContainer.GetComponentsInChildren<Text>())
        {
            var professionData = txt.GetComponent<ProfessionData>();
            txt.color = (professionData.Profession == profession ? YellowSelectedColor : Color.white);
        }
        ShowAvailableSkills(profession);
    }

    public void SelectChar(CreatePartyChar toSelect)
    {
        CreatePartyCharSelectedIndex = createPartyChars.IndexOf(toSelect);
        foreach (var cpc in createPartyChars)
            cpc.IsSelected = cpc == toSelect;
        SetProfessionTextSelected(toSelect.Profession);
    }

    public void SetCharSelected(int charIndex)
    {
        SelectChar(createPartyChars[charIndex]);
    }

    public void AddSkill(Skill skill)
    {
        createPartyChars[CreatePartyCharSelectedIndex].AddSkill(skill);
    }

    public void ShowAvailableSkills(Profession profession)
    {
        var skills = profession.AvailableSkills.Select(s => Skill.Get(s)).ToList();

        // remove skills but first
        var texts = skillsContainer.GetComponentsInChildren<Text>();
        for (int i = 1; i < texts.Length; i++)
            Destroy(texts[i].gameObject);

        var skill1 = texts[0];
        skill1.text = skills[0].Name;
        skill1.GetComponent<SkillData>().Skill = skills[0];

        for (int i = 1; i <= skills.Count - 1; i++)
        {
            var newSkill = Instantiate(skill1, skill1.transform.parent);
            newSkill.text = skills[i].Name;
            newSkill.GetComponent<SkillData>().Skill = skills[i];
        }
    }

    public void GiveBackUsedBonusPoints() {
        CreatePartyUseCase.GiveBackUsedBonusPoints(CreatePartyCharSelectedIndex);
    }

    public int GetCharIndex(CreatePartyChar cpc) {
        return createPartyChars.IndexOf(cpc);
    }
}
