using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;
using Business;

public class VideoBuildingUI : BaseUI<VideoBuildingUI> {

    [SerializeField]
    private RawImage videoImage;

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

    //public Building Building { get; set; }
    //public List<Npc> Npcs { get; set; }

    private Dictionary<string, int> CurrentGreeting = new Dictionary<string, int>();
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private Canvas canvas;

    public override void Awake() {
        base.Awake();
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
    }

	public override void Update () {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            canvas.enabled = false;
        }
	}

    public void Show(Building building, List<Npc> npcs) {
//        Building = building;
//        Npcs = npcs;
        base.Show();
        StartCoroutine(PlayVideo("Assets/Resources/Videos/" + building.VideoFilename + ".mp4"));
        buildingNameText.text = building.Name;
        var npc = npcs[0]; // TODO: more than 1
        dialogText.text = npc.NextGreeting();
        portraitTopicsPortraitImage.texture = Resources.Load(string.Format("NPC Pictures/NPC{0:D3}", npc.PictureCode)) as Texture;
        portraitTopicsPortraitText.text = npc.Name;
        ShowTopics(npc);
    }

    private void OnVideoPrepared() {
        canvas.enabled = true;
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

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)topicsContainer.transform);
    }

    public void OnTopicClicked(Npc npc, NpcTopic npcTopic) {
        dialogText.text = npcTopic.Description;
    }
        
    IEnumerator PlayVideo(string url)
    {
        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        videoPlayer.url = url;
        videoPlayer.isLooping = false;

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        //videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        var waitTime = new WaitForSeconds(0.01f);
        while (!videoPlayer.isPrepared)
            yield return waitTime;
        yield return waitTime;

        //Assign the Texture from Video to RawImage to be displayed
        videoImage.texture = videoPlayer.texture;

        Debug.Log("Done Preparing Video");
        OnVideoPrepared();

        //Play Video & sound
        videoPlayer.Play();
        audioSource.Play();
    }

}
