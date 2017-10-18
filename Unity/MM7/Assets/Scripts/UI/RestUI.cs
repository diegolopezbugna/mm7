using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class RestUI : BaseUI<RestUI>, PartyRestsViewInterface {

    [SerializeField]
    private Text timeValue;

    [SerializeField]
    private Text dayValue;

    [SerializeField]
    private Text monthValue;

    [SerializeField]
    private Text yearValue;

    [SerializeField]
    private RawImage hourglassImage;

    private float _defaultEnviroDayLengthInMinutes = 30f;
    private float _defaultEnviroNightLengthInMinutes = 30f;

    private int _defaultHourglassSprite = 4;
    private int _currentHourglassSprite;
    private Texture[] _hourglassTextures;
    private Texture[] HourglassTextures {
        get {
            if (_hourglassTextures == null || _hourglassTextures.Length == 0)
                _hourglassTextures = Resources.LoadAll<Texture>("RestHourGlass");
            return _hourglassTextures;
        }
    }

    public override void Awake()
    {
        base.Awake();
        // TODO: inside a cave???
        if (EnviroSky.instance != null)
        {
            _defaultEnviroDayLengthInMinutes = EnviroSky.instance.GameTime.DayLengthInMinutes;
            _defaultEnviroNightLengthInMinutes = EnviroSky.instance.GameTime.NightLengthInMinutes;
        }
    }

    public override void Show(bool cursorLock)
    {
        base.Show(cursorLock);
        Time.timeScale = 1;
        _currentHourglassSprite = _defaultHourglassSprite;
        hourglassImage.texture = HourglassTextures[_currentHourglassSprite];
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
        {
            NextHourGlass();
            yield return null;
        }
        
        while (EnviroSky.instance.internalHour < enviroHour)
        {
            NextHourGlass();
            yield return null;
        }
        
        EnviroSky.instance.GameTime.DayLengthInMinutes = _defaultEnviroDayLengthInMinutes;
        EnviroSky.instance.GameTime.NightLengthInMinutes = _defaultEnviroDayLengthInMinutes;

        if (onFinished != null)
            onFinished();
    }

    private void NextHourGlass()
    {
        _currentHourglassSprite++;
        if (_currentHourglassSprite >= _hourglassTextures.Length)
            _currentHourglassSprite = 0;
        hourglassImage.texture = HourglassTextures[_currentHourglassSprite];
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
        restUseCase.RestAndHeal(false);
    }

    public void OnWaitUntilDawnClick()
    {
        StartCoroutine(WaitUntil(8f, () => Hide()));  // TODO: different sunrises depending on season and location
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
