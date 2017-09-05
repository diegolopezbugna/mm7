using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;

public class StatsUI : MonoBehaviour {

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text skillPointsText;

    [SerializeField]
    private GameObject AttributesContainer;

    [SerializeField]
    private GameObject HPSPContainer;

    [SerializeField]
    private GameObject ConditionContainer;

    [SerializeField]
    private GameObject AgeLevelExpContainer;

    [SerializeField]
    private GameObject BonusDamageContainer;

    [SerializeField]
    private GameObject ResistancesContainer;

    public PlayingCharacter PlayingCharacter { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowStats(PlayingCharacter playingCharacter)
    {
        PlayingCharacter = playingCharacter;
        
        nameText.text = PlayingCharacter.Name + " the " + PlayingCharacter.Profession.Name; // TODO: localization
        skillPointsText.text = string.Format("{0}: {1}", Localization.Instance.Get("SkillPoints"), PlayingCharacter.SkillPointsLeft);

        // TODO: right click on attribute
        // TODO: green/red depending if attribute/value is temporary up/down

        // TODO: spells and conditions that modify attributes/bonuses
        AttributesContainer.transform.GetChild(6).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.Might, PlayingCharacter.Might);
        AttributesContainer.transform.GetChild(7).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.Intellect, PlayingCharacter.Intellect);
        AttributesContainer.transform.GetChild(8).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.Personality, PlayingCharacter.Personality);
        AttributesContainer.transform.GetChild(9).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.Endurance, PlayingCharacter.Endurance);
        AttributesContainer.transform.GetChild(10).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.Accuracy, PlayingCharacter.Accuracy);
        AttributesContainer.transform.GetChild(11).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.Speed, PlayingCharacter.Speed);

        HPSPContainer.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.HitPoints, PlayingCharacter.MaxHitPoints);
        HPSPContainer.transform.GetChild(4).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.SpellPoints, PlayingCharacter.MaxSpellPoints);
        HPSPContainer.transform.GetChild(5).GetComponent<Text>().text = string.Format("{0} / {1}", PlayingCharacter.ArmorClass, PlayingCharacter.ArmorClass);

        // TODO: condition/quickSpell

        AgeLevelExpContainer.transform.GetChild(3).GetComponent<Text>().text = PlayingCharacter.Age.ToString();
        AgeLevelExpContainer.transform.GetChild(4).GetComponent<Text>().text = PlayingCharacter.Level.ToString();
        AgeLevelExpContainer.transform.GetChild(5).GetComponent<Text>().text = PlayingCharacter.Experience.ToString();

        BonusDamageContainer.transform.GetChild(4).GetComponent<Text>().text = (PlayingCharacter.AttackBonus >= 0 ? "+" : "") + PlayingCharacter.AttackBonus.ToString();
        BonusDamageContainer.transform.GetChild(5).GetComponent<Text>().text = string.Format("{0} - {1}", PlayingCharacter.DamageMin, PlayingCharacter.DamageMax);
        BonusDamageContainer.transform.GetChild(6).GetComponent<Text>().text = (PlayingCharacter.RangedAttackBonus >= 0 ? "+" : "") + PlayingCharacter.RangedAttackBonus.ToString();
        BonusDamageContainer.transform.GetChild(7).GetComponent<Text>().text = string.Format("{0} - {1}", PlayingCharacter.RangedDamageMin, PlayingCharacter.RangedDamageMax);

        // TODO: resistances
        ResistancesContainer.transform.GetChild(6).GetComponent<Text>().text = "0 / 0";
        ResistancesContainer.transform.GetChild(7).GetComponent<Text>().text = "0 / 0";
        ResistancesContainer.transform.GetChild(8).GetComponent<Text>().text = "0 / 0";
        ResistancesContainer.transform.GetChild(9).GetComponent<Text>().text = "0 / 0";
        ResistancesContainer.transform.GetChild(10).GetComponent<Text>().text = "0 / 0";
        ResistancesContainer.transform.GetChild(11).GetComponent<Text>().text = "0 / 0";
    }

}
