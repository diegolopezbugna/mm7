﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public enum CharEnemyEngagingStatus {
    None,
    Green,
    Yellow,
    Red
}

public enum CharConditionStatus {
    Normal,
    Unconscious,
    Dead,
}

public class CharPortraitImages {
    public Texture Normal;
    public List<Texture> Damage;
    public Texture Unconscious;
    public Texture Dead;
}

public class CharPortrait : MonoBehaviour {

    [SerializeField]
    private RawImage charPortraitImage;

    [SerializeField]
    private RawImage spellAnimationImage;

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

    private CharPortraitImages portraitImages;

    private CharConditionStatus _conditionStatus;
    public CharConditionStatus ConditionStatus
    {
        get { return _conditionStatus; }
        set {
            _conditionStatus = value;
            if (value == CharConditionStatus.Unconscious)
            {
                charPortraitImage.texture = portraitImages.Unconscious;
                SetStatus(CharEnemyEngagingStatus.None);
            }
            else if (value == CharConditionStatus.Dead)
            {
                charPortraitImage.texture = portraitImages.Dead;
                SetStatus(CharEnemyEngagingStatus.None);
            }
            else
            {
                charPortraitImage.texture = portraitImages.Normal;
            }
        }
    }

    public PlayingCharacter PlayingCharacter { get; set; }

	// Use this for initialization
	void Start () {
        var button = charPortraitImage.gameObject.AddComponent<CharPortraitButton>();
        button.OnCharPortraitButtonLeftUp = OnCharPortraitLeftClick;
        if (spellAnimationImage != null)
            spellAnimationImage.canvasRenderer.SetAlpha(0f);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SetPortraitImageCode(string portraitCode) {
        SetPortraitImages(
            GetPortraitImagePath(portraitCode, "01"), 
            new string[] { 
                GetPortraitImagePath(portraitCode, "37"),
                GetPortraitImagePath(portraitCode, "38"),
                GetPortraitImagePath(portraitCode, "39")},
            GetPortraitImagePath(portraitCode, "11"),
            "Portraits/PCover03a");
    }

    private string GetPortraitImagePath(string portraitCode, string portraitSubcode) {
        return string.Format("Portraits/PC{0}{1}", portraitCode, portraitSubcode);
    }

    public void SetPortraitImages(string normal, string[] damage, string unconscious, string dead)
    {
        portraitImages = new CharPortraitImages();
        portraitImages.Normal = Resources.Load(normal) as Texture;
        portraitImages.Damage = new List<Texture>(damage.Length - 1);
        foreach (var d in damage)
            portraitImages.Damage.Add(Resources.Load(d) as Texture);
        portraitImages.Unconscious = Resources.Load(unconscious) as Texture;
        portraitImages.Dead = Resources.Load(dead) as Texture;
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

    public void SetStatus(CharEnemyEngagingStatus status)
    {
        if (!IsCharActive() && status != CharEnemyEngagingStatus.None)
            return;

        if (status == CharEnemyEngagingStatus.Green)
            SetStatusGreen();
        else if (status == CharEnemyEngagingStatus.Yellow)
            SetStatusYellow();
        else if (status == CharEnemyEngagingStatus.Red)
            SetStatusRed();
        else
            SetStatusNone();
    }

    public void ShowHitPortrait() {
        StartCoroutine(DoShowHitPortrait());
    }
        
    private IEnumerator DoShowHitPortrait() {
        var ratio = hitPointsSlider.value / hitPointsSlider.maxValue;
        charPortraitImage.texture = portraitImages.Damage[ratio > 0.66 ? 0 : (ratio > 0.33 ? 1 : 2)];
        yield return new WaitForSeconds(0.5f);
        ConditionStatus = ConditionStatus;
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

    public bool IsCharActive() 
    {
        return ConditionStatus != CharConditionStatus.Unconscious && 
            ConditionStatus != CharConditionStatus.Dead;
    }

    public void ShowSpellAnimation(SpellInfo spell)
    {
        if (spell.PortraitAnimationTextures == null || spell.PortraitAnimationTextures.Count == 0)
            return;
        
        spellAnimationImage.canvasRenderer.SetAlpha(0f);
        if (spell.PortraitAnimationTextures.Count == 1)
            StartCoroutine(AnimateSpell(spell.PortraitAnimationTextures[0]));
        else
            StartCoroutine(AnimateSpell(spell.PortraitAnimationTextures));
    }

    private IEnumerator AnimateSpell(Texture texture)
    {
        spellAnimationImage.texture = texture;
        spellAnimationImage.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSecondsRealtime(3);
        spellAnimationImage.CrossFadeAlpha(0, 1, true);
    }

    private IEnumerator AnimateSpell(List<Texture> textures)
    {
        spellAnimationImage.CrossFadeAlpha(1f, 0.1f, true);
        for (var i = 0; i < textures.Count; i++)
        {
            spellAnimationImage.texture = textures[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        spellAnimationImage.CrossFadeAlpha(0f, 0.1f, true);
    }

    public void OnCharPortraitLeftClick()
    {
        Party.Instance.OnPortraitLeftClick(PlayingCharacter);
    }
}
