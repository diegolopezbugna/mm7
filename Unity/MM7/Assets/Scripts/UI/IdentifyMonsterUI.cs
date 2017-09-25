using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class IdentifyMonsterUI : BaseUI<IdentifyMonsterUI> {

    [SerializeField]
    private Text nameValue;

    [SerializeField]
    private Slider hitPointsSlider;

    [SerializeField]
    private Text hitPointsValue;

    [SerializeField]
    private Text armorClassValue;

    [SerializeField]
    private Text attackValue;

    [SerializeField]
    private Text damageValue;

    [SerializeField]
    private int initialViewWidth = 283;

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonUp(1))
        {
            Hide();
        }
    }

    public void Show(EnemyInfo enemy, EnemyHealth enemyHealth)
    {
        base.Show(true);
        nameValue.text = enemy.Name;
        RedimensionSliderAndView(enemyHealth.MaxHitPoints);
        hitPointsSlider.maxValue = enemyHealth.MaxHitPoints;
        hitPointsSlider.value = enemyHealth.hitPoints;
        hitPointsValue.text = enemyHealth.hitPoints.ToString();
        // TODO: ID monster skill
    }

    private void RedimensionSliderAndView(int enemyMaxHPs)
    {
        var currentSize = ((RectTransform)hitPointsSlider.transform).sizeDelta;
        ((RectTransform)hitPointsSlider.transform).sizeDelta = new Vector2(enemyMaxHPs, currentSize.y);
        var viewWidth = enemyMaxHPs + 20 > initialViewWidth ? 20 + enemyMaxHPs : initialViewWidth;
        ((RectTransform)hitPointsSlider.transform.parent).sizeDelta = new Vector2(viewWidth, ((RectTransform)hitPointsSlider.transform.parent).sizeDelta.y);
    }
}
