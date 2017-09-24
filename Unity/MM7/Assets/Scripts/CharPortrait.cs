using System.Collections;
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

public class CharPortraitImages {
    public Texture Normal;
    public List<Texture> Damage;
    public Texture Sleeping;
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

    [SerializeField]
    private RawImage selectedImage;

    private CharPortraitImages portraitImages;

    private ConditionStatus _conditionStatus;
    public ConditionStatus ConditionStatus
    {
        get 
        { 
            return _conditionStatus; 
        }
        set 
        {
            _conditionStatus = value;
            if (value == ConditionStatus.Unconscious)
            {
                charPortraitImage.texture = portraitImages.Unconscious;
                SetStatus(CharEnemyEngagingStatus.None);    // TODO: this should be inside the use case
            }
            else if (value == ConditionStatus.Sleeping)
            {
                charPortraitImage.texture = portraitImages.Sleeping;
            }
            else if (value == ConditionStatus.Dead)
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

    public bool IsSelected 
    {
        get { return selectedImage.IsActive(); }
        set { selectedImage.gameObject.SetActive(value); }
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
            GetPortraitImagePath(portraitCode, "04"),
            GetPortraitImagePath(portraitCode, "11"),
            "Portraits/PCover03a");
    }

    private string GetPortraitImagePath(string portraitCode, string portraitSubcode) {
        return string.Format("Portraits/PC{0}{1}", portraitCode, portraitSubcode);
    }

    public void SetPortraitImages(string normal, string[] damage, string sleeping, string unconscious, string dead)
    {
        portraitImages = new CharPortraitImages();
        portraitImages.Normal = Resources.Load<Texture>(normal);
        portraitImages.Damage = new List<Texture>(damage.Length - 1);
        foreach (var d in damage)
            portraitImages.Damage.Add(Resources.Load<Texture>(d));
        portraitImages.Sleeping = Resources.Load<Texture>(sleeping);
        portraitImages.Unconscious = Resources.Load<Texture>(unconscious);
        portraitImages.Dead = Resources.Load<Texture>(dead);
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
        if (!PlayingCharacter.IsActive && status != CharEnemyEngagingStatus.None)
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
        spellAnimationImage.CrossFadeAlpha(1, 0.3f, true);
        yield return new WaitForSecondsRealtime(1);
        spellAnimationImage.CrossFadeAlpha(0, 0.3f, true);
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
