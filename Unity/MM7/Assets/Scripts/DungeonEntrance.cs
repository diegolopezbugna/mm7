using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;
using UnityStandardAssets.Characters.FirstPerson;

public class DungeonEntrance : MonoBehaviour {

    [SerializeField]
    private string locationCode;

    public string LocationCode { get { return locationCode; } }

    [SerializeField]
    private Texture picture;

    [SerializeField]
    private bool isExit = false;

    [SerializeField]
    private Transform exitPoint;

    private DungeonEntranceInfo _dungeonEntranceInfo;
    public DungeonEntranceInfo DungeonInstanceInfo
    {
        get
        {
            if (_dungeonEntranceInfo == null)
                _dungeonEntranceInfo = DungeonEntranceInfo.GetByLocationCode(LocationCode);
            return _dungeonEntranceInfo;
        }
    }

    public string GetDescription() {
        return DungeonInstanceInfo.Name;
    }

    public void Show()
    {
        VideoBuildingUI.Instance.Show(DungeonInstanceInfo, picture, isExit);
    }

    public void SetPartyLocation()
    {
        if (exitPoint != null)
            FirstPersonController.Instance.transform.SetPositionAndRotation(exitPoint.position, exitPoint.rotation);
    }
}
