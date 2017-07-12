using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePartyChar : MonoBehaviour {

    [SerializeField]
    private RawImage portraitImage;

    [SerializeField]
    private int maxPortraits = 20;

    [SerializeField]
    private int portraitSelected = 1;


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
    }
}
