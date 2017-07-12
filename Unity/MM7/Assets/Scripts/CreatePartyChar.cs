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

    public RaceCode RaceSelected { get; private set; }

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

	// Use this for initialization
	void Start () {
		
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
            RaceSelected = RaceCode.Human;
        else if (portraitSelected <= 12)
            RaceSelected = RaceCode.Elf;
        else if (portraitSelected <= 16)
            RaceSelected = RaceCode.Dwarf;
        else if (portraitSelected <= 20)
            RaceSelected = RaceCode.Goblin;

        raceText.text = RaceSelected.ToString();

        var race = Race.FromCode(RaceSelected);

        mightValue.text = race.DefaultMight.ToString();
        intellectValue.text = race.DefaultIntellect.ToString();
        personalityValue.text = race.DefaultPersonality.ToString();
        enduranceValue.text = race.DefaultEndurance.ToString();
        accuracyValue.text = race.DefaultAccuracy.ToString();
        speedValue.text = race.DefaultSpeed.ToString();

    }
}
