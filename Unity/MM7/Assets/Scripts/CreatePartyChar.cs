using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class CreatePartyChar : MonoBehaviour {

    [SerializeField]
    private RawImage portraitImage;

    [SerializeField]
    private int maxPortraits = 20;

    [SerializeField]
    private int portraitSelected = 1;

    [SerializeField]
    private Text raceText;

    [SerializeField]
    private Text mightValue;

    [SerializeField]
    private Text intellectValue;

    [SerializeField]
    private Text personalityValue;

    [SerializeField]
    private Text enduranceValue;

    [SerializeField]
    private Text accuracyValue;

    [SerializeField]
    private Text speedValue;

    private Race raceSelected;

	// Use this for initialization
	void Start () {
        UpdatePortrait();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PrevPortraitButtonClick() {
        if (portraitSelected == 1)
            portraitSelected = 20;
        else
            portraitSelected--;
        UpdatePortrait();
    }

    public void NextPortraitButtonClick() {
        if (portraitSelected == maxPortraits)
            portraitSelected = 1;
        else
            portraitSelected++;
        UpdatePortrait();
    }

    private void UpdatePortrait() {
        portraitImage.texture = Resources.Load(string.Format("Portraits/PC{0:D2}01", portraitSelected)) as Texture;
        UpdateRace();
    }

    private void UpdateRace() {
        if (portraitSelected <= 8)
            raceSelected = Race.Human();
        else if (portraitSelected <= 12)
            raceSelected = Race.Elf();
        else if (portraitSelected <= 16)
            raceSelected = Race.Dwarf();
        else if (portraitSelected <= 20)
            raceSelected = Race.Goblin();

        raceText.text = raceSelected.Name;

        mightValue.text = raceSelected.DefaultMight.ToString();
        intellectValue.text = raceSelected.DefaultIntellect.ToString();
        personalityValue.text = raceSelected.DefaultPersonality.ToString();
        enduranceValue.text = raceSelected.DefaultEndurance.ToString();
        accuracyValue.text = raceSelected.DefaultAccuracy.ToString();
        speedValue.text = raceSelected.DefaultSpeed.ToString();

    }

    public void AddMight() {
        var currentValue = int.Parse(mightValue.text);
        var bonusCost = raceSelected.GetBonusCostForMight(currentValue, true);
        if (CreateParty.Instance.CreatePartyUseCase.CanUseBonusPoints(bonusCost.BonusChange))
        {
            CreateParty.Instance.CreatePartyUseCase.BonusPointsUsed(bonusCost.BonusChange);
            mightValue.text = (currentValue + bonusCost.AttributeChange).ToString();
        }

        // TODO: check next
    }

    public void SubstractMight() {
        var currentValue = int.Parse(mightValue.text);
        var bonusCost = raceSelected.GetBonusCostForMight(currentValue, false);
        CreateParty.Instance.CreatePartyUseCase.BonusPointsUsed(-bonusCost.BonusChange);
        mightValue.text = (currentValue - bonusCost.AttributeChange).ToString();
    }

}
