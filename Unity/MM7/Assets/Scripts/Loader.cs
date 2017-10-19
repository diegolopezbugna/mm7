using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Loader : MonoBehaviour {

    public static Loader Instance;

    [SerializeField] private GameObject FpsControllerPrefab;
    [SerializeField] private GameObject[] uiPrefabs;
    [SerializeField] private Transform initialPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        var fps = FindObjectOfType<FirstPersonController>();
        if (fps == null)
        {
            fps = Instantiate(FpsControllerPrefab).GetComponent<FirstPersonController>();
            fps.gameObject.name = "FPSController";
            fps.SetPositionAndRotation(initialPosition.transform.position, initialPosition.transform.rotation);
        }

        foreach (var prefab in uiPrefabs)
        {
            if (!GameObject.Find(prefab.name))
            {
                var instance = Instantiate(prefab);
                instance.name = prefab.name;
            }
        }

        // TODO: find a way to remove direct references to these uiPrefabs
        VideoBuildingUI.Instance.gameObject.SetActive(false);
        NpcDialog.Instance.gameObject.SetActive(false);
        CharDetailsUI.Instance.gameObject.SetActive(false);
        SpellBookUI.Instance.gameObject.SetActive(false);
        RestUI.Instance.gameObject.SetActive(false);
        IdentifyMonsterUI.Instance.gameObject.SetActive(false);
        GameOverUI.Instance.gameObject.SetActive(false);
        OpenChestUI.Instance.gameObject.SetActive(false);
    }
}
