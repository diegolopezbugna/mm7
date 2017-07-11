using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharPortraitStatus {
    None,
    Green,
    Yellow,
    Red
}

public class CharPortrait : MonoBehaviour {

    [SerializeField]
    private RawImage charPortraitImage;

    [SerializeField]
    private RawImage statusGreenImage;

    [SerializeField]
    private RawImage statusYellowImage;

    [SerializeField]
    private RawImage statusRedImage;

    [SerializeField]
    private Slider hitPointsSlider;

    [SerializeField]
    private Slider spellPointsSlider;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    // TODO: distintas.. otro objeto?
    public void SetPortraitImages(string[] images)
    {
        // TODO: mas imagens
        Texture t = Resources.Load("Portraits/" + images[0]) as Texture;
        charPortraitImage.texture = t;
    }

    public void SetMaxHitPoints(float maxHitPoints)
    {
        hitPointsSlider.maxValue = maxHitPoints;
    }

    public void SetMaxSpellPoints(float maxSpellPoints)
    {
        spellPointsSlider.maxValue = maxSpellPoints;
    }

    public void SetHitPoints(float hitPoints) {
        hitPointsSlider.value = hitPoints;
    }

    public void SetSpellPoints(float spellPoints) {
        spellPointsSlider.value = spellPoints;
    }

    public void SetStatus(CharPortraitStatus status)
    {
        if (status == CharPortraitStatus.Green)
            SetStatusGreen();
        else if (status == CharPortraitStatus.Yellow)
            SetStatusYellow();
        else if (status == CharPortraitStatus.Red)
            SetStatusRed();
        else
            SetStatusNone();
    }

    private void SetStatusNone() 
    {
        statusGreenImage.enabled = false;
        statusYellowImage.enabled = false;
        statusRedImage.enabled = false;
    }

    private void SetStatusGreen() 
    {
        statusGreenImage.enabled = true;
        statusYellowImage.enabled = false;
        statusRedImage.enabled = false;
    }

    private void SetStatusYellow() 
    {
        statusGreenImage.enabled = false;
        statusYellowImage.enabled = true;
        statusRedImage.enabled = false;
    }

    private void SetStatusRed() 
    {
        statusGreenImage.enabled = false;
        statusYellowImage.enabled = false;
        statusRedImage.enabled = true;
    }

}
