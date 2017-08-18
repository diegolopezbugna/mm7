using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;

public class VideoDoor : MonoBehaviour {

    [SerializeField]
    private string locationCode;

    private Building building;
    private List<Npc> npcs;

	// Use this for initialization
	void Start () {
        npcs = Npc.GetByLocationCode(locationCode);
        building = Building.GetByLocationCode(locationCode);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string GetDescription() {
        return building.Name;
    }

    public string TryOpen() {
        // TODO: for shops check opening & closing hours

        VideoBuilding.Instance.Show(building, npcs);

        return "";
    }
}
