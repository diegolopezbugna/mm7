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
    public DungeonEntranceInfo DungeonEntranceInfo
    {
        get
        {
            if (_dungeonEntranceInfo == null)
                _dungeonEntranceInfo = DungeonEntranceInfo.GetByLocationCode(LocationCode);
            return _dungeonEntranceInfo;
        }
    }

    public string GetDescription() {
        if (!string.IsNullOrEmpty(DungeonEntranceInfo.VideoFilename))
            return DungeonEntranceInfo.Name;
        else if (isExit)
            return DungeonEntranceInfo.LeaveText;
        else
            return DungeonEntranceInfo.EnterText;
    }

    public void Show()
    {
                VideoBuildingUI.Instance.Show(DungeonEntranceInfo, picture, isExit);
    }

    public void SetPartyLocation()
    {
        if (exitPoint != null)
            FirstPersonController.Instance.SetPositionAndRotation(exitPoint.position, exitPoint.rotation);
    }
}
