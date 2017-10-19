using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using System.Linq;
using UnityEngine.SceneManagement;

public class CreateParty : Singleton<CreateParty>, CreatePartyViewInterface
{

    public CreatePartyUseCase CreatePartyUseCase { get; private set; }

    [SerializeField]
    private List<CreatePartyChar> createPartyChars;
        
    [SerializeField]
    private Text bonusPointsText;


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

    // Use this for initialization
    void Start()
    {
        CreatePartyUseCase = new CreatePartyUseCase(this);
        CreatePartyUseCase.ClearWithDefaultValues();
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

    public void SetPortraitSelectedForChar(int portrait, int charIndex)
    {
        createPartyChars[charIndex].PortraitSelected = portrait;
    }

    public void Clear()
    {
        CreatePartyUseCase.Clear();
    }

    public void SetProfessionForChar(Profession profession, int charIndex)
    {
        createPartyChars[charIndex].Profession = profession;
    }

    public void SetSkillForChar(Skill skill, int charIndex) {
        createPartyChars[charIndex].ToggleSkill(skill);
    }

    public void SetNameForChar(string name, int charIndex) {
        createPartyChars[charIndex].CharacterName = name;
    }

    public void GiveBackUsedBonusPoints(CreatePartyChar cpc) {
        CreatePartyUseCase.GiveBackUsedBonusPoints(GetCharIndex(cpc));
    }

    public int GetCharIndex(CreatePartyChar cpc) {
        return createPartyChars.IndexOf(cpc);
    }

    public void SetAttributeValuesForChar(int[] values, int charIndex) {
        createPartyChars[charIndex].SetAttributeValues(values);
    }

    public void Ok() {
        var partyStats = new PartyStats();
        partyStats.Chars = new List<PlayingCharacter>();
        foreach (var cpc in createPartyChars)
        {
            var pc = cpc.GetPlayingCaracter();
            CreatePartyUseCase.AddStartingInventoryItems(pc);
            partyStats.Chars.Add(pc);
        }
        Game.Instance.PartyStats = partyStats;

        SceneManager.LoadScene("EmeraldIsland");
    }
}
