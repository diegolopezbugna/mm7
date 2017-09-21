﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class RestUI : BaseUI<RestUI>, RestUseCaseViewInterface {

    [SerializeField]
    private Text timeValue;

    [SerializeField]
    private Text dayValue;

    [SerializeField]
    private Text monthValue;

    [SerializeField]
    private Text yearValue;

    private float _defaultEnviroDayLengthInMinutes = 30f;
    private float _defaultEnviroNightLengthInMinutes = 30f;

    public override void Start()
    {
        base.Start();
        _defaultEnviroDayLengthInMinutes = EnviroSky.instance.GameTime.DayLengthInMinutes;
        _defaultEnviroNightLengthInMinutes = EnviroSky.instance.GameTime.NightLengthInMinutes;
    }

    public override void Show(bool cursorLock)
    {
        base.Show(cursorLock);
        Time.timeScale = 1;
    }

    public override void Update()
    {
        base.Update();
        // TODO: get date/time from Game.Instance. Sync Game.Instance.GameDateTime with Enviro
        timeValue.text = string.Format("{0:D2}:{1:D2}", EnviroSky.instance.GameTime.Hours, EnviroSky.instance.GameTime.Minutes);
        dayValue.text = (EnviroSky.instance.GameTime.Days % 31 + 1).ToString();
        monthValue.text = (EnviroSky.instance.GameTime.Days / 31 + 1).ToString();
        yearValue.text = EnviroSky.instance.GameTime.Years.ToString();
    }

    IEnumerator WaitUntil(float enviroHour, Action onFinished)
    {
        EnviroSky.instance.GameTime.DayLengthInMinutes = 0.1f;
        EnviroSky.instance.GameTime.NightLengthInMinutes = 0.1f;

        while (EnviroSky.instance.internalHour > enviroHour)
            yield return null;
        
        while (EnviroSky.instance.internalHour < enviroHour)
            yield return null;
        
        EnviroSky.instance.GameTime.DayLengthInMinutes = _defaultEnviroDayLengthInMinutes;
        EnviroSky.instance.GameTime.NightLengthInMinutes = _defaultEnviroDayLengthInMinutes;

        if (onFinished != null)
            onFinished();
    }

    private void Wait(float enviroTimeToWait, Action onFinished) 
    {
        var awakeTime = EnviroSky.instance.internalHour + enviroTimeToWait;
        if (awakeTime > 24f)
            awakeTime -= 24f;

        StartCoroutine(WaitUntil(awakeTime, onFinished));
    }

    public void OnRestHealClick()
    {
        var restUseCase = new PartyRestsUseCase(this, Party.Instance);
        restUseCase.RestAndHeal();
    }

    public void OnWaitUntilDawnClick()
    {
        StartCoroutine(WaitUntil(8f, null));  // TODO: different sunrises depending on season and location
    }

    public void OnWait1HourClick()
    {
        Wait(1f, null);
    }

    public void OnWait15MinutesClick()
    {
        Wait(0.2f, null);
    }

    public void OnExitRestClick()
    {
        Hide();
    }

    #region RestUseCaseViewInterface implementation

    public void WaitTime(float timeInHours, Action onFinished)
    {
        Wait(timeInHours, onFinished);
    }

    public void UpdatePlayingCharacter(PlayingCharacter target)
    {
        Party.Instance.UpdatePlayingCharacter(target);
    }

    #endregion

}