using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class CreatePartyCharAttribute : MonoBehaviour {

    [SerializeField]
    private AttributeCode attributeCode;

    [SerializeField]
    private Text attributeValueText;

    [SerializeField]
    private Button substractButton;

    [SerializeField]
    private Button addButton;

    [SerializeField]
    private CreatePartyChar createPartyChar;

	// Use this for initialization
	void Start () {

        substractButton.onClick.AddListener(() => {
            var currentValue = int.Parse(attributeValueText.text);
            var bonusCost = createPartyChar.RaceSelected.GetBonusCost(attributeCode, currentValue, false);
            CreateParty.Instance.CreatePartyUseCase.BonusPointsUsed(-bonusCost.BonusChange, CreateParty.Instance.GetCharIndex(createPartyChar));
            attributeValueText.text = (currentValue - bonusCost.AttributeChange).ToString();
        });

        addButton.onClick.AddListener(() => {
            var currentValue = int.Parse(attributeValueText.text);
            var bonusCost = createPartyChar.RaceSelected.GetBonusCost(attributeCode, currentValue, true);
            if (CreateParty.Instance.CreatePartyUseCase.CanUseBonusPoints(bonusCost.BonusChange))
            {
                CreateParty.Instance.CreatePartyUseCase.BonusPointsUsed(bonusCost.BonusChange, CreateParty.Instance.GetCharIndex(createPartyChar));
                attributeValueText.text = (currentValue + bonusCost.AttributeChange).ToString();
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDefaultAttributeValue(Race race) {
        attributeValueText.text = race.DefaultAttributeValues[attributeCode].ToString();
    }

    public void SetAttributeValue(int value) {
        attributeValueText.text = value.ToString();
    }
}
