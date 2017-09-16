using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class BaseUI<T> : Singleton<T> where T : MonoBehaviour
{
    public bool IsShowing { get; private set; }

    // Use this for initialization
    public virtual void Awake() {
    }

    public virtual void Start() {
    }

    public virtual void Update() {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Hide();
        }
    }
    
    public virtual void Show() {
        Show(false);
    }

    public virtual void Show(bool cursorLock) {
        gameObject.SetActive(true);
        IsShowing = true;
        FirstPersonController.Instance.SetCursorLock(cursorLock);
        Time.timeScale = 0;
    }

    public virtual void Hide() {
        Time.timeScale = 1;
        FirstPersonController.Instance.SetCursorLock(true);
        IsShowing = false;
        gameObject.SetActive(false);
    }
}

