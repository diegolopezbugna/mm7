using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using Business;
using Infrastructure;
using UnityEngine.SceneManagement;

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

    public Building Building { get; set; }
    public List<Npc> Npcs { get; set; }

    private Dictionary<string, int> CurrentGreeting = new Dictionary<string, int>();
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private Canvas canvas;
    private ShopUI shopUI;

    public override void Awake() {
        base.Awake();
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
        shopUI = GetComponent<ShopUI>();
    }

	public override void Update () {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            canvas.enabled = false;
        }
	}

    public void Show(Building building, List<Npc> npcs) {
        Building = building;
        Npcs = npcs;
        base.Show();
        StartCoroutine(PlayVideo("Assets/Resources/Videos/" + building.VideoFilename + ".mp4"));
        buildingNameText.text = building.Name;
        var npc = npcs[0]; // TODO: more than 1
        dialogText.text = npc.NextGreeting().Text;
        portraitTopicsPortraitImage.texture = Resources.Load(string.Format("NPC Pictures/NPC{0:D3}", npc.PictureCode)) as Texture;
        portraitTopicsPortraitText.text = npc.Name;
        ShowTopics(npc);
    }

    public void Show(DungeonEntranceInfo dungeonEntranceInfo, Texture picture, bool isExit) {
        base.Show();
        StartCoroutine(PlayVideo("Assets/Resources/Videos/" + dungeonEntranceInfo.VideoFilename + ".mp4"));
        buildingNameText.text = dungeonEntranceInfo.Name;
        dialogText.text = dungeonEntranceInfo.Description;
        portraitTopicsPortraitImage.texture = picture;
        portraitTopicsPortraitText.text = "";
        ShowTopic(isExit ? dungeonEntranceInfo.LeaveText : dungeonEntranceInfo.EnterText, () => {
            Debug.LogFormat("Loading scene {0} ...", dungeonEntranceInfo.EnterSceneName);
            StartCoroutine(LoadScene(isExit ? dungeonEntranceInfo.LeaveSceneName : dungeonEntranceInfo.EnterSceneName, () => 
                {
                    foreach (var go in GameObject.FindGameObjectsWithTag("DungeonEntrance"))
                    {
                        var dungeonEntrance = go.GetComponent<DungeonEntrance>();
                        if (dungeonEntrance.LocationCode == dungeonEntranceInfo.LocationCode)
                        {
                            dungeonEntrance.SetPartyLocation();
                            break;
                        }
                    }
                    // TODO: hide loading
                    Hide();
                }));
            // TODO: show loading
        });
    }

    IEnumerator LoadScene(string sceneName, Action onLoaded)
    {
        // envirofog, envirolightshafts, enviroskyrendering
        Destroy(FirstPersonController.Instance.GetComponentInChildren<EnviroFog>());
        Destroy(FirstPersonController.Instance.GetComponentInChildren<EnviroLightShafts>());
        Destroy(FirstPersonController.Instance.GetComponentInChildren<EnviroSkyRendering>());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        if (onLoaded != null)
            onLoaded();
    }

    public override void Hide()
    {
        foreach (Transform child in videoImage.transform)
            Destroy(child.gameObject);
        base.Hide();
    }

    private void OnVideoPrepared() {
        canvas.enabled = true;
    }

    public void ShowTopics(Npc npc)
    {
        topicsContainer.SetActive(true);

        // remove all but first TODO: change to prefab and delete all
        var buttons = topicsContainer.GetComponentsInChildren<Button>();
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
            Destroy(buttons[i].gameObject);
        }

        var topicButton = topicsContainer.GetComponentInChildren<Button>();
        for (int i = 0; i < npc.Topics.Count; i++)
        {
            if (i > 0)
                topicButton = Instantiate(topicButton, topicButton.transform.parent);

            topicButton.GetComponent<Text>().text = npc.Topics[i].GetTitleFor(npc.Shop, Party.Instance.GetPlayingCharacterSelectedOrDefault());
            var npcTopic = npc.Topics[i];
            topicButton.onClick.RemoveAllListeners();
            topicButton.onClick.AddListener(() =>
                {
                    if (npc.Shop != null)
                        shopUI.OnTopicClicked(npc, npcTopic);
                    else
                        dialogText.text = npcTopic.Description;
                });
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)topicsContainer.transform);
    }

    private void ShowTopic(string topic, Action onClicked)
    {
        topicsContainer.SetActive(true);

        // remove all but first TODO: change to prefab and delete all
        var buttons = topicsContainer.GetComponentsInChildren<Button>();
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
            Destroy(buttons[i].gameObject);
        }

        var topicButton = topicsContainer.GetComponentInChildren<Button>();
        topicButton.GetComponent<Text>().text = topic;
        topicButton.onClick.RemoveAllListeners();
        topicButton.onClick.AddListener(() => onClicked());

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)topicsContainer.transform);
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
        var waitTime = new WaitForSecondsRealtime(0.01f);
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
