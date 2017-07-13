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

    private bool _isSelected = false;
    public bool IsSelected {
        get { return _isSelected; }
        set {
            _isSelected = value;
            selectedImage.enabled = value;
        }
    }

    public Race RaceSelected { get; set; }

	// Use this for initialization
	void Start () {
        UpdatePortrait();
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
    }

    public void SelectChar() {
        CreateParty.Instance.SelectChar(this);
    }

}
