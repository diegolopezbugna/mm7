using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Business;
using UnityStandardAssets.Characters.FirstPerson;
using System;

public class Party : Singleton<Party>, PartyCastsSpellViewInterface, EnemyAttacksViewInterface, PlayingCharacterViewInterface {

    private const float PAUSED_ATTACK_CONTINUE_TIME_IN_SECONDS = 1f;
    private const float PAUSED_ATTACK_WAIT_TIME_IN_SECONDS = 1f;

    [SerializeField]
    private Text foodValue;

    [SerializeField]
    private Text goldValue;

    [SerializeField]
    private Text focussedText;

    [SerializeField]
    private Text messagesText;

    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private float handToHandCombatDistanceSqr = 9;

    [SerializeField]
    private CharPortrait[] charsPortraits;

    private int _charPortraitSelected = -1;
    public int CharPortraitSelected
    { 
        get { return _charPortraitSelected; }
        set
        {
            _charPortraitSelected = value;
            for (int i = 0; i < charsPortraits.Length; i++)
                charsPortraits[i].IsSelected = false;
            if (value >= 0)
                charsPortraits[value].IsSelected = true;

            if (PlayingCharacterSelectedChanged != null)
                PlayingCharacterSelectedChanged(this, EventArgs.Empty);
        }
    }

    public PlayingCharacter GetPlayingCharacterSelected()
    {
        var i = CharPortraitSelected;
        if (i < 0)
            return null;
        else
            return charsPortraits[i].PlayingCharacter;
    }

    public PlayingCharacter GetPlayingCharacterSelectedOrDefault()
    {
        var selected = GetPlayingCharacterSelected();
        if (selected == null)
            selected = charsPortraits[0].PlayingCharacter;   // TODO: who will be next available?
        return selected;
    }
        
    public void SetPlayingCharacterSelected(PlayingCharacter playingCharacter)
    {
        if (playingCharacter == null)
        {
            CharPortraitSelected = -1;
            return;
        }

        for (int i = 0; i < charsPortraits.Length; i++)
        {
            if (charsPortraits[i].PlayingCharacter == playingCharacter)
            {
                CharPortraitSelected = i;
                break;
            }
        }
    }

    public event EventHandler PlayingCharacterSelectedChanged;

    private bool isEnemyEngagingPartyThisFrame = false;
    private bool isEnemyInHandToHandCombatThisFrame = false;
    private SpellInfo spellChoosingTarget = null;
    private bool isPausedAttack = false;
    private float nextPausedAttack = 0f;

    private PartyBlood partyBloodBehaviour;
    private PartyAttack partyAttackBehaviour;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start () {

        var chars = Game.Instance.PartyStats.Chars;
        for (int i = 0; i < chars.Count; i++)
        {
            charsPortraits[i].PlayingCharacter = Game.Instance.PartyStats.Chars[i];
            charsPortraits[i].SetPortraitImageCode(chars[i].PortraitCode);
            UpdatePlayingCharacter(chars[i]);
        }
        partyBloodBehaviour = this.GetComponent<PartyBlood>();
        partyAttackBehaviour = FirstPersonController.Instance.GetComponent<PartyAttack>();

        RefreshGoldAndFood();
	}
	
	// Update is called once per frame
	void Update () 
    {
        // TODO: refactor main loop

        if (VideoBuildingUI.Instance.IsShowing ||
            CharDetailsUI.Instance.IsShowing ||
            OpenChestUI.Instance.IsShowing ||
            RestUI.Instance.IsShowing ||
            SpellBookUI.Instance.IsShowing ||
            NpcDialog.Instance.IsShowing)
        {
            return;
        }

        if (CharPortraitSelected < 0)
        {
            SelectNextPlayingCharacter();
        }

        for (int i = 0; i < charsPortraits.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
                OnPortraitLeftClick(Game.Instance.PartyStats.Chars[i]);
        }

        if (nextPausedAttack > 0 && nextPausedAttack < Time.time)
        {
            nextPausedAttack = 0f;
            isPausedAttack = true;
        }

        if (CharPortraitSelected >= 0)
        {
            if (Input.GetKeyDown("i"))
            {
                CharDetailsUI.Instance.ShowInventory();
            }
            else if (Input.GetKeyDown("r"))
            {
                OnRestIconClicked();
            }
            else if (Input.GetKeyDown("b"))
            {
                OnSpellBookIconClicked();
            }

            if (Input.GetKeyDown("e"))
            {
                isPausedAttack = true;
            }
        }

        if ((spellChoosingTarget != null && spellChoosingTarget.Needs3dTarget) ||
            isPausedAttack)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                spellChoosingTarget = null;
                ExitPausedAttack();
            }
            else
            {
                // TODO: exiting identifyMonsterUI sets timeScale to 1
                Time.timeScale = 0;
                FirstPersonController.Instance.SetCursorLock(false);

                var targetRaycastHit = CalculateMouseTarget();
                if (targetRaycastHit != null)
                {
                    if (Input.GetKeyDown("t"))
                    {
                        SelectNextPlayingCharacter();
                        nextPausedAttack = Time.time + PAUSED_ATTACK_WAIT_TIME_IN_SECONDS;
                        ContinuePausedAttack();
                    }
                }
            }
            return;
        }

        bool isUserAttacking = Input.GetKeyDown("q");
        CalculateCrosshairTarget(isUserAttacking);
	}

    void LateUpdate() {
        CheckEnemiesEngagingStatus();
    }

    private void CalculateCrosshairTarget(bool isUserAttacking) {
        // TODO: refactor main loop

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
        {
            ProcessMainRaycastHit(hit);
        }
        else
        {
            focussedText.text = "";

            if (isUserAttacking)
            {
                // TODO: get nearest enemy
                Transform nearestAttackableEnemy = null;
                if (nearestAttackableEnemy == null)
                {
                    // TODO: shoot to nothing
                    //var screenPoint = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y, Camera.main.farClipPlane);
                }
            }
        }
    }

    private RaycastHit? CalculateMouseTarget() {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
        {
            ProcessMainRaycastHit(hit);
            return hit;
        }
        return null;
    }

    private void ProcessMainRaycastHit(RaycastHit hit)
    {
        if (spellChoosingTarget != null && spellChoosingTarget.Needs3dTarget) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                var partyCastsSpellUseCase = new PartyCastsSpellUseCase(this, this, FirstPersonController.Instance.transform);
                Time.timeScale = 1;
                FirstPersonController.Instance.SetCursorLock(true);
                partyCastsSpellUseCase.CastSpell(GetPlayingCharacterSelected(), spellChoosingTarget, hit.point, hit.transform);
                spellChoosingTarget = null;
                return;
            }
        }

        if (hit.transform.tag.StartsWith("Enemy"))
        {
            //Debug.LogFormat("HIT: {0} - {1}" + hit.transform.gameObject.tag, hit.distance);
            focussedText.text = hit.transform.gameObject.tag.TagToDescription();

            if (Input.GetMouseButton(1))
            {
                var enemyAttack = hit.transform.GetComponent<EnemyAttack>();
                var enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if (enemyAttack != null && enemyHealth != null)
                {
                    IdentifyMonsterUI.Instance.Show(enemyAttack.EnemyInfo, enemyHealth);
                }
            }
            else if (Input.GetKeyDown("q") ||
                     (isPausedAttack && Input.GetMouseButtonDown(0)))
            {
                var attackingChar = GetPlayingCharacterSelected();
                if (attackingChar != null)
                {
                    if (isPausedAttack)
                        ContinuePausedAttack();
                    partyAttackBehaviour.DoAttack(attackingChar, hit.point, hit.transform);
                }
            }
            else
            {
                var enemyLoot = hit.transform.GetComponent<EnemyLoot>(); // TODO: remove enemyLoot
                if (enemyLoot != null && enemyLoot.isActiveAndEnabled && hit.distance < 4 && Input.GetMouseButtonDown(0))
                    enemyLoot.Loot();
            }
        }
        else if (hit.transform.tag.StartsWith("VideoDoor") && hit.distance < 5)
        {
            var videoDoor = hit.transform.GetComponent<VideoDoor>();
            focussedText.text = videoDoor.GetDescription();
            if (Input.GetMouseButton(0))
            {
                ShowMessage(videoDoor.TryOpen());
            }
        }
        else if (hit.transform.tag.StartsWith("DungeonEntrance") && hit.distance < 5)
        {
            var dungeonEntrance = hit.transform.GetComponent<DungeonEntrance>();
            focussedText.text = dungeonEntrance.GetDescription();
            if (Input.GetMouseButton(0))
            {
                dungeonEntrance.Show();
            }
        }
        else if (hit.transform.tag.StartsWith("Npc") && hit.distance < 2)
        {
            var npcTalk = hit.transform.GetComponent<NpcTalk>();
            if (npcTalk != null)
            {
                focussedText.text = npcTalk.GetDescription();
                if (Input.GetMouseButton(0))
                {
                    npcTalk.Talk();
                }
            }
            var npcNews = hit.transform.GetComponent<NpcNews>();
            if (npcNews != null)
            {
                focussedText.text = npcNews.GetDescription();
                if (Input.GetMouseButton(0))
                    npcNews.Talk();
            }
        }
        else if (hit.transform.tag.StartsWith("Chest") && hit.distance < 4)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // TODO: which chest background?
                var chest = hit.transform.GetComponent<Chest>();
                OpenChestUI.Instance.Show(chest.GeneratedItems);
            }
        }
        else
        {
            focussedText.text = "";
        }
    }

    private void ContinuePausedAttack()
    {
        nextPausedAttack = Time.time + PAUSED_ATTACK_CONTINUE_TIME_IN_SECONDS;
        isPausedAttack = false;
        Time.timeScale = 1;
        FirstPersonController.Instance.SetCursorLock(true);
    }

    private void ExitPausedAttack()
    {
        Time.timeScale = 1;
        FirstPersonController.Instance.SetCursorLock(true);
        isPausedAttack = false;
    }

    public void SetEnemyEngagingParty(GameObject enemy, float distanceSqr) {
        isEnemyEngagingPartyThisFrame = true;

        if (distanceSqr < handToHandCombatDistanceSqr)
            isEnemyInHandToHandCombatThisFrame = true;
    }

    private void CheckEnemiesEngagingStatus() {
        if (isEnemyInHandToHandCombatThisFrame)
        {
            foreach (var p in charsPortraits)
                p.SetStatus(CharEnemyEngagingStatus.Red);
        }
        else if (isEnemyEngagingPartyThisFrame)
        {
            foreach (var p in charsPortraits)
                p.SetStatus(CharEnemyEngagingStatus.Yellow);
        }
        else
        {
            foreach (var p in charsPortraits)
                p.SetStatus(CharEnemyEngagingStatus.Green);
        }
        isEnemyInHandToHandCombatThisFrame = false;
        isEnemyEngagingPartyThisFrame = false;
    }

    public float GetDistanceSqrTo(Transform other) {
        return (FirstPersonController.Instance.transform.position - other.position).sqrMagnitude;
    }

    public void SpellBookCastSpell(SpellInfo spellInfo)
    {
        if (spellInfo.NeedsPartyTarget || spellInfo.Needs3dTarget)
        {
            Time.timeScale = 0;
            FirstPersonController.Instance.SetCursorLock(false);
            spellChoosingTarget = spellInfo;
        }
        else
        {
            var partyCastsSpellUseCase = new PartyCastsSpellUseCase(this, this, FirstPersonController.Instance.transform);
            partyCastsSpellUseCase.CastSpell(GetPlayingCharacterSelected(), spellInfo);
        }
    }

    public void OnPortraitLeftClick(PlayingCharacter playingCharClicked)
    {
        if (spellChoosingTarget != null && spellChoosingTarget.NeedsPartyTarget)
        {
            var partyCastsSpellUseCase = new PartyCastsSpellUseCase(this, this, FirstPersonController.Instance.transform);
            Time.timeScale = 1;
            partyCastsSpellUseCase.CastSpell(GetPlayingCharacterSelected(), spellChoosingTarget, playingCharClicked);
            spellChoosingTarget = null;
            FirstPersonController.Instance.SetCursorLock(true);
        }
        else
        {
            if (VideoBuildingUI.Instance.IsShowing ||
               (playingCharClicked.IsActive && playingCharClicked.LastAttackTimeTo < Time.time))
            {
                SetPlayingCharacterSelected(playingCharClicked);
            }
        }
    }

    public void AddMessage(string message)
    {
        MessagesScroller.Instance.AddMessage(message);
    }

    public void ShowSpellFx(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3? targetPoint, System.Action<Transform> onCollision)
    {
        partyAttackBehaviour.ShowSpellFx(attackingChar, spellInfo, targetPoint, onCollision);
    }

    public void ThrowSpellFx(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3 targetPoint, System.Action<Transform> onCollision)
    {
        partyAttackBehaviour.ThrowSpellFx(attackingChar, spellInfo, targetPoint, onCollision);
    }

    public void ShowPortraitSpellAnimation(PlayingCharacter target, SpellInfo spellInfo)
    {
        var i = Game.Instance.PartyStats.Chars.IndexOf(target);
        charsPortraits[i].ShowSpellAnimation(spellInfo);
    }

    public void UpdatePlayingCharacter(PlayingCharacter playingCharacter)
    {
        var i = Game.Instance.PartyStats.Chars.IndexOf(playingCharacter);
        charsPortraits[i].UpdatePlayingCharacter();
        if (!playingCharacter.IsActive && GetPlayingCharacterSelected() == playingCharacter)
            SelectNextPlayingCharacter();
    }

    public void TakeHit(PlayingCharacter target)
    {
        partyBloodBehaviour.TakeHit();
        charsPortraits[Game.Instance.PartyStats.Chars.IndexOf(target)].ShowHitPortrait();
        if (!target.IsActive && GetPlayingCharacterSelected() == target)
            SelectNextPlayingCharacter();
    }

    public void ShowGameOver()
    {
        GameOverUI.Instance.Show();
    }

    public void SelectNextPlayingCharacter() 
    {
        var current = CharPortraitSelected;
        var i = current;

        for (int pass = 0; pass < charsPortraits.Length; pass++)
        {
            i++;
            if (i >= charsPortraits.Length)
                i -= charsPortraits.Length;

            if (charsPortraits[i].PlayingCharacter.IsActive && 
                charsPortraits[i].PlayingCharacter.LastAttackTimeTo < Time.time)
            {
                CharPortraitSelected = i;
                return;
            }
        }

        CharPortraitSelected = -1;
    }

    public void OnRestIconClicked() 
    {
        RestUI.Instance.Show();
    }

    public void OnSpellBookIconClicked()
    {
        if (isPausedAttack)
            ContinuePausedAttack();
        
        var playingCharacterSelected = GetPlayingCharacterSelected();
        if (playingCharacterSelected != null)
            SpellBookUI.Instance.Show(playingCharacterSelected);
    }

    public void ShowMessage(string message)
    {
        StartCoroutine(ShowMessageFor(message, 3f));
    }

    IEnumerator ShowMessageFor(string message, float seconds)
    {
        messagesText.text = message;
        yield return new WaitForSecondsRealtime(seconds);
        if (messagesText.text == message)
            messagesText.text = "";
    }

    public void RefreshGoldAndFood()
    {
        foodValue.text = Game.Instance.PartyStats.Food.ToString();
        goldValue.text = Game.Instance.PartyStats.Gold.ToString();
    }

    public void PlayGoldSound()
    {
        // TODO: gold sound
    }

}
