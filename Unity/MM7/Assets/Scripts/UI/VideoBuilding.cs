using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
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

    private Dictionary<string, int> CurrentGreeting = new Dictionary<string, int>();
    private FirstPersonController fpc;

    void Awake() {
        fpc = FindObjectOfType<FirstPersonController>();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            fpc.SetCursorLock(true);
            gameObject.SetActive(false);
        }
	}

    public void Show() {
        gameObject.SetActive(true);
        fpc.SetCursorLock(false);
        buildingNameText.text = Building.Name;
        var npc = Npcs[0]; // TODO: more than 1
        dialogText.text = npc.NextGreeting();
        portraitTopicsPortraitImage.texture = Resources.Load(string.Format("NPC Pictures/NPC{0:D3}", npc.PictureCode)) as Texture;
        portraitTopicsPortraitText.text = npc.Name;
        ShowTopics(npc);
    }

    private void ShowTopics(Npc npc)
    {
        // remove all but first
        var texts = topicsContainer.GetComponentsInChildren<Text>();
        for (int i = 1; i < texts.Length; i++)
        {
            texts[i].GetComponent<TopicData>().Npc = null;   // destroy is not inmediate
            texts[i].GetComponent<TopicData>().NpcTopic = null;   // destroy is not inmediate
            Destroy(texts[i].gameObject);
        }

        var topicText = topicsContainer.GetComponentInChildren<Text>();
        for (int i = 0; i < npc.Topics.Count; i++)
        {
            if (i > 0)
                topicText = Instantiate(topicText, topicText.transform.parent);

            topicText.text = npc.Topics[i].Title;
            topicText.GetComponent<TopicData>().Npc = npc;
            topicText.GetComponent<TopicData>().NpcTopic = npc.Topics[i];
        }

    }

    public void OnTopicClicked(Npc npc, NpcTopic npcTopic) {
        dialogText.text = npcTopic.Description;
    }
        
}
