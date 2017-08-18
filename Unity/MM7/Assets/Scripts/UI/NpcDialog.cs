using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Business;

// TODO: code repeated on this class and VideoBuilding!!!
public class NpcDialog : Singleton<NpcDialog> {

    [SerializeField]
    private GameObject npcTextContainer;

    [SerializeField]
    private Text npcText;

    [SerializeField]
    private GameObject topicsContainer;

    private FirstPersonController fpc;

    public bool IsShowing { get; private set; }

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
            Hide();
        }
	}

    public void Show(Npc npc) {
        gameObject.SetActive(true);
        IsShowing = true;
        fpc.SetCursorLock(false);
        npcText.text = npc.NextGreeting();
        ShowTopics(npc);
        RepositionNpcText();
    }

    public void ShowNews(string news) {
        gameObject.SetActive(true);
        IsShowing = true;
        npcText.text = news;
        topicsContainer.SetActive(false);
        RepositionNpcText();
        StartCoroutine(DismissNpcText(2));
    }

    private IEnumerator DismissNpcText(float seconds) {
        yield return new WaitForSeconds(seconds);
        Hide();
    }

    private void Hide() {
        fpc.SetCursorLock(true);
        IsShowing = false;
        gameObject.SetActive(false);
    }

    private void ShowTopics(Npc npc)
    {
        topicsContainer.SetActive(true);

        // remove all but first
        var texts = topicsContainer.GetComponentsInChildren<Button>();
        for (int i = 1; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
            Destroy(texts[i].gameObject);
        }

        var topicText = topicsContainer.GetComponentInChildren<Button>();
        for (int i = 0; i < npc.Topics.Count; i++)
        {
            if (i > 0)
                topicText = Instantiate(topicText, topicText.transform.parent);

            topicText.GetComponentInChildren<Text>().text = npc.Topics[i].Title;
            var npcTopic = npc.Topics[i];
            topicText.onClick.RemoveAllListeners();
            topicText.onClick.AddListener(() =>
                {
                    OnTopicClicked(npc, npcTopic);
                });
        }

    }

    private void OnTopicClicked(Npc npc, NpcTopic npcTopic) {
        npcText.text = npcTopic.Description;
        RepositionNpcText();
    }

    private void RepositionNpcText() {
        var npcTextRect = (RectTransform)npcTextContainer.transform;
        var topicsRect = (RectTransform)topicsContainer.transform;
        LayoutRebuilder.ForceRebuildLayoutImmediate(npcTextRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(topicsRect);
        npcTextRect.anchoredPosition = new Vector2(npcTextRect.anchoredPosition.x, topicsRect.anchoredPosition.y + topicsRect.rect.height);
    }

}
