using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameOverUI : BaseUI<GameOverUI> {

    public override void Show()
    {
        base.Show();
        FirstPersonController.Instance.enabled = false;
    }

    public override void Update()
    {
    }

}
