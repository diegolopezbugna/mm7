using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;

public class SpellBookUI : BaseUI<SpellBookUI> {

    [SerializeField]
    private RawImage spellBookBg;

    public override void Start()
    {
        base.Start();
        Show(null, SkillCode.FireMagic);
    }

    public void Show(PlayingCharacter speller, SkillCode skillCode) {
        base.Show();
        // TODO: learnt spells!

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

}
