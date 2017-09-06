using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;

public class SkillsUI : MonoBehaviour {

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text skillPointsText;

    [SerializeField]
    private GameObject LeftContainer;

    [SerializeField]
    private GameObject RightContainer;

    [SerializeField]
    private GameObject SkillGroupPrefab;

    [SerializeField]
    private GameObject SkillItemTemplatePrefab;

    public PlayingCharacter PlayingCharacter { get; set; }

    public void ShowSkills(PlayingCharacter playingCharacter)
    {
        PlayingCharacter = playingCharacter;

        nameText.text = Localization.Instance.Get("Skills for: {0}", PlayingCharacter.Name); // TODO: localization
        skillPointsText.text = string.Format("{0}: {1}", Localization.Instance.Get("SkillPoints"), PlayingCharacter.SkillPointsLeft);

        foreach (Transform child in LeftContainer.transform)
            Destroy(child.gameObject);

        AddSkillGroup(LeftContainer.transform, "Weapons");
        AddSkills(LeftContainer.transform, SkillGroup.Weapons);

        AddSkillGroup(LeftContainer.transform, "Magic");
        AddSkills(LeftContainer.transform, SkillGroup.Magic);

        foreach (Transform child in RightContainer.transform)
            Destroy(child.gameObject);
        
        AddSkillGroup(RightContainer.transform, "Armor");
        AddSkills(RightContainer.transform, SkillGroup.Armor);

        AddSkillGroup(RightContainer.transform, "Misc");
        AddSkills(RightContainer.transform, SkillGroup.Misc);

        // TODO: right click on skill
        // TODO: green/red depending if skill can be levelled
    }

    private void AddSkillGroup(Transform parentTransform, string localizationKey)
    {
        var skillGroup = Instantiate(SkillGroupPrefab, parentTransform);
        skillGroup.GetComponent<Text>().text = Localization.Instance.Get(localizationKey);
        skillGroup.transform.GetChild(0).GetComponent<Text>().text = Localization.Instance.Get("Level");
    }

    private void AddSkills(Transform parentTransform, SkillGroup skillGroup)
    {
        var skillsFromThisGroup = PlayingCharacter.Skills.Values.Where(ss => ss.Skill.SkillGroup == skillGroup);
        foreach (var s in skillsFromThisGroup)
            AddSkill(parentTransform, s);
    }

    private void AddSkill(Transform parentTransform, SkillStatus skillStatus)
    {
        var skillItem = Instantiate(SkillItemTemplatePrefab, parentTransform);
        var skillName = skillStatus.Skill.Name;
        if (skillStatus.SkillLevel != SkillLevel.Normal)
            skillName += " " + Localization.Instance.Get(skillStatus.SkillLevel.ToString());
        skillItem.GetComponent<Text>().text = skillName;
        skillItem.transform.GetChild(0).GetComponent<Text>().text = skillStatus.Points.ToString();
    }
}
