using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;
using System.Linq;

public class CreatePartyChar : MonoBehaviour {

    private string[] FemalePortraits = new string[] { "05", "06", "07", "08", "11", "12", "15", "16", "19", "20", "22", "25" };
    public Color YellowSelectedColor = new Color(255f / 255f, 240f / 255f, 41f / 255f);

    [SerializeField]
    private RawImage portraitImage;

    [SerializeField]
    private int maxPortraits = 20;

    private int _portraitSelected;
    public int PortraitSelected { 
        get { return _portraitSelected; }
        set {
            _portraitSelected = value;
            UpdatePortrait();
        }
    }

    [SerializeField]
    private Text raceText;

    [SerializeField]
    private CreatePartyCharAttribute[] attributes;

    [SerializeField]
    private Text professionText;

    [SerializeField]
    private RawImage professionImage;

    [SerializeField]
    private Text skillsTitleText;

    [SerializeField]
    private GameObject skillsContainer;

    [SerializeField]
    private GameObject professionsContainer;

    public Race RaceSelected { get; set; }

    private Profession _profession;
    public Profession Profession
    { 
        get { return _profession; } 
        set
        {
            _profession = value;
            professionText.text = value.Name;
            professionImage.texture = Resources.Load("Professions/IC_" + value.ProfessionCode.ToString()) as Texture;

            foreach (var txt in professionsContainer.GetComponentsInChildren<Text>())
            {
                var professionData = txt.GetComponent<ProfessionData>();
                txt.color = (professionData.Profession == value ? YellowSelectedColor : Color.white);
            }

            CreateSkills();
            SetDefaultSkill(Skill.Get(value.DefaultSkills[0]));
            SetDefaultSkill(Skill.Get(value.DefaultSkills[1]));
        }
    }

    public string CharacterName
    {
        get {
            return characterNameInputField.text;
        }
        set {
            characterNameInputField.text = value;
        }
    }
    private InputField characterNameInputField;

    private List<Skill> SkillsDefault { get; set; }
    private List<Skill> SkillsSelected { get; set; }

    void Awake() {
        characterNameInputField = this.GetComponentInChildren<InputField>();
        CreateProfessions();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PrevPortraitButtonClick() {
        if (PortraitSelected == 1)
            PortraitSelected = 20;
        else
            PortraitSelected--;
        UpdatePortrait();
    }

    public void NextPortraitButtonClick() {
        if (PortraitSelected == maxPortraits)
            PortraitSelected = 1;
        else
            PortraitSelected++;
        UpdatePortrait();
    }

    private void UpdatePortrait() {
        portraitImage.texture = Resources.Load(string.Format("Portraits/PC{0:D2}01", PortraitSelected)) as Texture;
        UpdateRace();
    }

    // TODO: move to business
    private void UpdateRace() {
        if (PortraitSelected <= 8)
            RaceSelected = Race.Human();
        else if (PortraitSelected <= 12)
            RaceSelected = Race.Elf();
        else if (PortraitSelected <= 16)
            RaceSelected = Race.Dwarf();
        else if (PortraitSelected <= 20)
            RaceSelected = Race.Goblin();

        raceText.text = RaceSelected.Name;

        foreach (var a in attributes)
            a.SetDefaultAttributeValue(RaceSelected);

        CreateParty.Instance.GiveBackUsedBonusPoints(this);
    }

    private void CreateSkills()
    {
        SkillsDefault = new List<Skill>();
        SkillsSelected = new List<Skill>();

        var skills = Profession.DefaultSkills.Select(s => Skill.Get(s)).Union(
            Profession.AvailableSkills.Select(s => Skill.Get(s))).ToList();

        // remove skills but first
        var texts = skillsContainer.GetComponentsInChildren<Text>();
        for (int i = 1; i < texts.Length; i++)
        {
            texts[i].GetComponent<SkillData>().Skill = null;   // destroy is not inmediate
            Destroy(texts[i].gameObject);
        }

        var skill1 = texts[0];
        skill1.text = skills[0].Name;
        skill1.color = Color.white;
        skill1.GetComponent<SkillData>().Skill = skills[0];

        for (int i = 1; i <= skills.Count - 1; i++)
        {
            var newSkill = Instantiate(skill1, skill1.transform.parent);
            newSkill.text = skills[i].Name;
            newSkill.GetComponent<SkillData>().Skill = skills[i];
        }
    }

    public void SetDefaultSkill(Skill skill) {
        SkillsDefault.Add(skill);
        var txt = GetSkillObject(skill).GetComponent<Text>();
        txt.color = YellowSelectedColor;  // TODO: otro color para la default?
        SetSkillsTitle();
    }

    public void ToggleSkill(Skill skill) {
        if (SkillsDefault.Contains(skill))
            return;

        if (SkillsSelected.Contains(skill))
        {
            SkillsSelected.Remove(skill);
            var txt = GetSkillObject(skill).GetComponent<Text>();
            txt.color = Color.white;
        }
        else if (SkillsSelected.Count < 2)
        {
            SkillsSelected.Add(skill);
            var txt = GetSkillObject(skill).GetComponent<Text>();
            txt.color = Color.green;
        }

        SetSkillsTitle();
    }

    private void SetSkillsTitle() {
        skillsTitleText.text = string.Format("SKILLS {0}/4", SkillsDefault.Count + SkillsSelected.Count);
    }

    public void SkillOnClick(SkillData skillData)
    {
        ToggleSkill(skillData.Skill);
    }

    private GameObject GetSkillObject(Skill skill) {
        foreach (var skillData in skillsContainer.GetComponentsInChildren<SkillData>())
            if (skillData.Skill == skill)
                return skillData.gameObject;
        return null;
    }

    public void SetAttributeValues(int[] values) {
        for (var i = 0; i < values.Length; i++)
            attributes[i].SetAttributeValue(values[i]);
    }

    void CreateProfessions()
    {
        var professions = Profession.All();

        var prof1 = professionsContainer.GetComponentInChildren<Text>();
        prof1.text = professions[0].Name;
        prof1.GetComponent<ProfessionData>().Profession = professions[0];

        for (int i = 1; i < professions.Count; i++)
        {
            var newProf = Instantiate(prof1, prof1.transform.parent);
            newProf.text = professions[i].Name;
            newProf.GetComponent<ProfessionData>().Profession = professions[i];
        }
    }

    public PlayingCharacter GetPlayingCaracter() {
        var portraitCode = PortraitSelected.ToString("D2");
        var gender = FemalePortraits.Contains(portraitCode) ? Gender.Female : Gender.Male;
        PlayingCharacter pc = new PlayingCharacter(CharacterName, RaceSelected, gender, portraitCode);
        pc.Profession = Profession;

        pc.Might = attributes[0].GetAttributeValue();
        pc.Intellect = attributes[1].GetAttributeValue();
        pc.Personality = attributes[2].GetAttributeValue();
        pc.Endurance = attributes[3].GetAttributeValue();
        pc.Accuracy = attributes[4].GetAttributeValue();
        pc.Speed = attributes[5].GetAttributeValue();

        pc.Level = 1;
        pc.Experience = 0;

        pc.HitPoints = pc.MaxHitPoints;
        pc.SpellPoints = pc.MaxSpellPoints;

        return pc;
    }
}
