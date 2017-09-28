using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;

public class DungeonEntrance : MonoBehaviour {

    [SerializeField]
    private string locationCode;

    [SerializeField]
    private Texture picture;

    private DungeonEntranceInfo _dungeonEntranceInfo;
    public DungeonEntranceInfo DungeonInstanceInfo
    {
        get
        {
            if (_dungeonEntranceInfo == null)
                _dungeonEntranceInfo = DungeonEntranceInfo.GetByLocationCode(locationCode);
            return _dungeonEntranceInfo;
        }
    }

    public string GetDescription() {
        return DungeonInstanceInfo.Name;
    }

    public void Show()
    {
        VideoBuildingUI.Instance.Show(DungeonInstanceInfo, picture);
    }

}
