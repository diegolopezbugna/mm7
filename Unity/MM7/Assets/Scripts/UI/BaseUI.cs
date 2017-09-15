﻿using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class BaseUI<T> : Singleton<T> where T : MonoBehaviour
{
    public bool IsShowing { get; private set; }

    private FirstPersonController fpc;

    // Use this for initialization
    public virtual void Awake() {
        fpc = FindObjectOfType<FirstPersonController>();
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
        fpc.SetCursorLock(cursorLock);
        Time.timeScale = 0;
    }

    public virtual void Hide() {
        Time.timeScale = 1;
        fpc.SetCursorLock(true);
        IsShowing = false;
        gameObject.SetActive(false);
    }
}

