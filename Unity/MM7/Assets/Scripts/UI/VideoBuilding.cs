using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class VideoBuilding : Singleton<VideoBuilding> {

    [SerializeField]
    private Text dialogText;

    [SerializeField]
    private Text buildingNameText;

    [SerializeField]
    private GameObject portraitsContainer;

    [SerializeField]
    private GameObject portraitTopicsContainer;

    [SerializeField]
    private RawImage portraitTopicsPortraitImage;

    [SerializeField]
    private Text portraitTopicsPortraitText;

    [SerializeField]
    private GameObject topicsContainer;

    public Building Building { get; set; }
    public List<Npc> Npcs { get; set; }

	// Use this for initialization
	void Start () {
        buildingNameText.text = Building.Name;
        var npc = Npcs[0]; // TODO: more than 1
        portraitTopicsPortraitImage.texture = Resources.Load(string.Format("NPC Pictures/NPC{0:D3}", npc.PictureCode)) as Texture;
        portraitTopicsPortraitText.text = npc.Name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
