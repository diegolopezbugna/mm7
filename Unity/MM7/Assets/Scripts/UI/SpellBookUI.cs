using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;

public class SpellBookUI : BaseUI<SpellBookUI> {

    [SerializeField]
    private RawImage spellBookBg;

    [SerializeField]
    private RectTransform tabsContainer;

    private Dictionary<SkillCode, Texture> texturesTabsOff = new Dictionary<SkillCode, Texture>();
    private Dictionary<SkillCode, Texture> texturesTabsOn = new Dictionary<SkillCode, Texture>();

    public override void Awake()
    {
        base.Awake();
        texturesTabsOff.Add(SkillCode.FireMagic, Resources.Load<Texture>("Spells/TAB1A"));
        texturesTabsOn.Add(SkillCode.FireMagic, Resources.Load<Texture>("Spells/TAB1B"));
        texturesTabsOff.Add(SkillCode.AirMagic, Resources.Load<Texture>("Spells/TAB2A"));
        texturesTabsOn.Add(SkillCode.AirMagic, Resources.Load<Texture>("Spells/TAB2B"));
        texturesTabsOff.Add(SkillCode.WaterMagic, Resources.Load<Texture>("Spells/TAB3A"));
        texturesTabsOn.Add(SkillCode.WaterMagic, Resources.Load<Texture>("Spells/TAB3B"));
        texturesTabsOff.Add(SkillCode.EarthMagic, Resources.Load<Texture>("Spells/TAB4A"));
        texturesTabsOn.Add(SkillCode.EarthMagic, Resources.Load<Texture>("Spells/TAB4B"));
        texturesTabsOff.Add(SkillCode.SpiritMagic, Resources.Load<Texture>("Spells/TAB5A"));
        texturesTabsOn.Add(SkillCode.SpiritMagic, Resources.Load<Texture>("Spells/TAB5B"));
        texturesTabsOff.Add(SkillCode.MindMagic, Resources.Load<Texture>("Spells/TAB6A"));
        texturesTabsOn.Add(SkillCode.MindMagic, Resources.Load<Texture>("Spells/TAB6B"));
        texturesTabsOff.Add(SkillCode.BodyMagic, Resources.Load<Texture>("Spells/TAB7A"));
        texturesTabsOn.Add(SkillCode.BodyMagic, Resources.Load<Texture>("Spells/TAB7B"));
        texturesTabsOff.Add(SkillCode.LightMagic, Resources.Load<Texture>("Spells/TAB8A"));
        texturesTabsOn.Add(SkillCode.LightMagic, Resources.Load<Texture>("Spells/TAB8B"));
        texturesTabsOff.Add(SkillCode.DarkMagic, Resources.Load<Texture>("Spells/TAB9A"));
        texturesTabsOn.Add(SkillCode.DarkMagic, Resources.Load<Texture>("Spells/TAB9B"));
    }

    public override void Start()
    {
        base.Start();
        Show(null, SkillCode.FireMagic);
    }

    public void Show(PlayingCharacter speller, SkillCode skillCode) {
        base.Show();
        // TODO: learnt spells!

        Clean();
        DrawTabs(speller, skillCode);

        var prefix = skillCode.ToString().Replace("Magic", "");
        var kk = string.Format("Spells/{0}/{1}Bg", skillCode, prefix);
        spellBookBg.texture = Resources.Load<Texture>(string.Format("Spells/{0}/{1}Bg", skillCode, prefix));
        var spells = SpellInfo.GetAllBySkill(skillCode);

        foreach (var s in spells)
            DrawSpell(s);
    }

    private void DrawSpell(SpellInfo spell)
    {
        var go = new GameObject(spell.Name);
        go.transform.SetParent(spellBookBg.transform, false);
        var image = go.AddComponent<Image>();
        image.sprite = spell.TextureOff;
        image.SetNativeSize();
        image.rectTransform.pivot = Vector2.up;
        image.rectTransform.anchorMin = Vector2.up;
        image.rectTransform.anchorMax = Vector2.up;
        image.rectTransform.anchoredPosition = new Vector2(spell.SpellBookPosX, -spell.SpellBookPosY);
        var button = go.AddComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.SpriteSwap;
        var spriteState = new SpriteState();
        spriteState.highlightedSprite = spell.TextureOn;
        button.spriteState = spriteState;
    }

    private void Clean()
    {
        foreach (Transform child in spellBookBg.transform)
            Destroy(child.gameObject);  // TODO: reuse?
        foreach (Transform child in tabsContainer.transform)
            Destroy(child.gameObject);  // TODO: reuse?
    }

    private void DrawTabs(PlayingCharacter speller, SkillCode selectedTab)
    {
        // TODO: learnt skills!
        DrawTab(speller, SkillCode.FireMagic, selectedTab == SkillCode.FireMagic);
        DrawTab(speller, SkillCode.AirMagic, selectedTab == SkillCode.AirMagic);
        DrawTab(speller, SkillCode.WaterMagic, selectedTab == SkillCode.WaterMagic);
        DrawTab(speller, SkillCode.EarthMagic, selectedTab == SkillCode.EarthMagic);
        DrawTab(speller, SkillCode.SpiritMagic, selectedTab == SkillCode.SpiritMagic);
        DrawTab(speller, SkillCode.MindMagic, selectedTab == SkillCode.MindMagic);
        DrawTab(speller, SkillCode.BodyMagic, selectedTab == SkillCode.BodyMagic);
        DrawTab(speller, SkillCode.LightMagic, selectedTab == SkillCode.LightMagic);
        DrawTab(speller, SkillCode.DarkMagic, selectedTab == SkillCode.DarkMagic);
    }

    private void DrawTab(PlayingCharacter speller, SkillCode skillCode, bool isSelected)
    {
        var go = new GameObject(skillCode.ToString());
        go.transform.SetParent(tabsContainer.transform, false);
        var image = go.AddComponent<RawImage>();
        image.texture = isSelected ? texturesTabsOn[skillCode] : texturesTabsOff[skillCode];
        image.SetNativeSize();
        image.rectTransform.pivot = Vector2.up;
        image.rectTransform.anchorMin = Vector2.up;
        image.rectTransform.anchorMax = Vector2.up;
        image.rectTransform.anchoredPosition = isSelected ? GetTabOnPositionFor(skillCode) : GetTabOffPositionFor(skillCode);
        if (!isSelected)
        {
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(() => Show(speller, skillCode));
        }
    }

    private Vector2 GetTabOffPositionFor(SkillCode skillCode)
    {
        // hardcoded?
        switch (skillCode)
        {
            case SkillCode.FireMagic:
                return new Vector2(15f, -10f);
            case SkillCode.AirMagic:
                return new Vector2(15f, -46f);
            case SkillCode.WaterMagic:
                return new Vector2(15f, -83f);
            case SkillCode.EarthMagic:
                return new Vector2(15f, -121f);
            case SkillCode.SpiritMagic:
                return new Vector2(15f, -158f);
            case SkillCode.MindMagic:
                return new Vector2(16f, -196f);
            case SkillCode.BodyMagic:
                return new Vector2(16f, -234f);
            case SkillCode.LightMagic:
                return new Vector2(16f, -271f);
            case SkillCode.DarkMagic:
                return new Vector2(16f, -307f);
        }
        return Vector2.zero;
    }

    private Vector2 GetTabOnPositionFor(SkillCode skillCode)
    {
        // hardcoded?
        switch (skillCode)
        {
            case SkillCode.FireMagic:
                return new Vector2(6f, -9f);
            case SkillCode.AirMagic:
                return new Vector2(6f, -46f);
            case SkillCode.WaterMagic:
                return new Vector2(6f, -84f);
            case SkillCode.EarthMagic:
                return new Vector2(6f, -121f);
            case SkillCode.SpiritMagic:
                return new Vector2(7f, -158f);
            case SkillCode.MindMagic:
                return new Vector2(5f, -196f);
            case SkillCode.BodyMagic:
                return new Vector2(5f, -234f);
            case SkillCode.LightMagic:
                return new Vector2(5f, -272f);
            case SkillCode.DarkMagic:
                return new Vector2(5f, -309f);
        }
        return Vector2.zero;
    }


}
