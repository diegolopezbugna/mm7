using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;

public class CreatePartyChar : MonoBehaviour {

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
    private RawImage selectedImage;

    [SerializeField]
    private Text professionText;

    [SerializeField]
    private RawImage professionImage;

    [SerializeField]
    private SkillData[] skills;

    private bool _isSelected = false;
    public bool IsSelected {
        get { return _isSelected; }
        set {
            _isSelected = value;
            selectedImage.enabled = value;
        }
    }

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

            SetSkill(0, Skill.Get(value.DefaultSkills[0]));
            SetSkill(1, Skill.Get(value.DefaultSkills[1]));
            SetSkill(2, null);
            SetSkill(3, null);
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

    void Awake() {
        characterNameInputField = this.GetComponentInChildren<InputField>();
    }

	// Use this for initialization
	void Start () {
        //UpdatePortrait();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PrevPortraitButtonClick() {
        SelectChar();
        if (PortraitSelected == 1)
            PortraitSelected = 20;
        else
            PortraitSelected--;
        UpdatePortrait();
    }

    public void NextPortraitButtonClick() {
        SelectChar();
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

        CreateParty.Instance.GiveBackUsedBonusPoints();
    }

    public void SelectChar() {
        CreateParty.Instance.SelectChar(this);
    }

    public void SetSkill(int index, Skill skill) {
        skills[index].Skill = skill;
        var text = skills[index].GetComponent<Text>();
        if (skill != null)
        {
            text.text = skill.Name;
//            text.color = CreateParty.Instance.YellowSelectedColor;
        }
        else
        {
            text.text = Localization.Instance.Get("None");
//            text.color = CreateParty.Instance.YellowSelectedColor;
        }
    }

    public void RemoveSkill(Skill skill) {
        if (skills[2].Skill == skill)
            SetSkill(2, null);
        else if (skills[3].Skill == skill)
            SetSkill(3, null);
    }

    public void AddSkill(Skill skill) {
        if (skills[2].Skill == null)
            SetSkill(2, skill);
        else if (skills[3].Skill == null)
            SetSkill(3, skill);
    }

    public void SetAttributeValues(int[] values) {
        for (var i = 0; i < values.Length; i++)
            attributes[i].SetAttributeValue(values[i]);
    }

    public PlayingCharacter GetPlayingCaracter() {
        PlayingCharacter pc = new PlayingCharacter(CharacterName, RaceSelected, PortraitSelected.ToString("D2"));
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
