using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Business;
using UnityStandardAssets.Characters.FirstPerson;

public class Party : Singleton<Party>, PartyCastsSpellViewInterface {

    [SerializeField]
    private Text focussedText;

    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private float handToHandCombatDistanceSqr = 9;

    [SerializeField]
    private CharPortrait[] charsPortraits;

    private Transform currentTarget;
    public Transform CurrentTarget
    {
        get
        {
            return currentTarget;
        }
        set
        {
            currentTarget = value;
        }
    }

    private Vector3 currentTargetPoint;
    public Vector3 CurrentTargetPoint
    {
        get
        {
            return currentTargetPoint;
        }
        set
        {
            currentTargetPoint = value;
        }
    }

    private bool isEnemyEngagingPartyThisFrame = false;
    private bool isEnemyInHandToHandCombatThisFrame = false;
    private SpellInfo spellChoosingTarget = null;

    private PartyHealth partyHealthBehaviour;
    private PartyAttack partyAttackBehaviour;

	// Use this for initialization
	void Start () {

        var chars = Game.Instance.PartyStats.Chars;
        for (int i = 0; i < chars.Count; i++)
        {
            charsPortraits[i].PlayingCharacter = Game.Instance.PartyStats.Chars[i];
            charsPortraits[i].SetPortraitImageCode(chars[i].PortraitCode);
            UpdatePlayingCharacter(chars[i]);
        }
        partyHealthBehaviour = this.GetComponent<PartyHealth>();
        partyAttackBehaviour = this.GetComponent<PartyAttack>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        // TODO: refactor main loop
        
        if (spellChoosingTarget != null) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (spellChoosingTarget.Needs3dTarget)
                {
                    var targetPoint = CalculateMouseTarget();
                    var partyCastsSpellUseCase = new PartyCastsSpellUseCase(this, transform);
                    Time.timeScale = 1;
                    partyCastsSpellUseCase.ThrowSpell(Game.Instance.PartyStats.Chars[3], spellChoosingTarget, targetPoint); // TODO: selected char
                    spellChoosingTarget = null;
                    FirstPersonController.Instance.SetCursorLock(true);
                }
            }
            else
            {
                CalculateMouseTarget();
            }
            return;
        }

        if (Input.GetKeyDown("i"))
        {
            CharDetailsUI.Instance.ShowInventory();
        }
        else if (Input.GetKeyDown("s"))
        {
            CharDetailsUI.Instance.ShowStats();
        }
        else if (Input.GetKeyDown("k"))
        {
            CharDetailsUI.Instance.ShowSkills();
        }
        else if (Input.GetKeyDown("b"))
        {
            // TODO: check recovery times
            SpellBookUI.Instance.Show(Game.Instance.PartyStats.Chars[3]); // TODO: selected char
        }

        bool isUserAttacking = Input.GetKeyDown("q");
        CalculateTarget(isUserAttacking);
	}

    void LateUpdate() {
        CheckEnemiesEngagingStatus();
    }

    void CalculateTarget(bool includeTargetPoint) {
        // TODO: refactor main loop

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
        {
            if (hit.transform.tag.StartsWith("Enemy"))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.blue, 2, true);
                //Debug.Log("HIT: " + hit.transform.gameObject.tag);

                focussedText.text = hit.transform.gameObject.tag.TagToDescription() + " - " + hit.distance;
            }
            else if (hit.transform.tag.StartsWith("VideoDoor") && hit.distance < 5)
            {
                var videoDoor = hit.transform.GetComponent<VideoDoor>();
                focussedText.text = videoDoor.GetDescription();
                if (Input.GetMouseButton(0) && !VideoBuildingUI.Instance.IsShowing && !CharDetailsUI.Instance.IsShowing) // TODO: better way
                {
                    focussedText.text = videoDoor.TryOpen(); // TODO: show result for X seconds
                }
            }
            else if (hit.transform.tag.StartsWith("Npc") && hit.distance < 2)
            {
                var npcTalk = hit.transform.GetComponent<NpcTalk>();
                if (npcTalk != null)
                {
                    focussedText.text = npcTalk.GetDescription();
                    if (Input.GetMouseButton(0) && !NpcDialog.Instance.IsShowing)
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
            else
            {
                focussedText.text = "";
            }

			CurrentTarget = hit.transform;

            if (includeTargetPoint)
                CurrentTargetPoint = hit.point;
        }
        else
        {
            focussedText.text = "";
            CurrentTarget = null;

            if (includeTargetPoint)
            {
                var screenPoint = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y, Camera.main.farClipPlane);
                CurrentTargetPoint = Camera.main.ScreenToWorldPoint(screenPoint);
            }
        }
    }

    public Vector3? CalculateMouseTarget() {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
        {
            UpdateFocussedText(hit);
            return hit.point;
        }
        return null;
    }

    private void UpdateFocussedText(RaycastHit hit)
    {
        if (hit.transform.tag.StartsWith("Enemy"))
        {
            focussedText.text = hit.transform.gameObject.tag.TagToDescription() + " - " + hit.distance;
        }
        else if (hit.transform.tag.StartsWith("VideoDoor") && hit.distance < 5)
        {
            var videoDoor = hit.transform.GetComponent<VideoDoor>();
            focussedText.text = videoDoor.GetDescription();
        }
        else if (hit.transform.tag.StartsWith("Npc") && hit.distance < 2)
        {
            var npcTalk = hit.transform.GetComponent<NpcTalk>();
            if (npcTalk != null)
                focussedText.text = npcTalk.GetDescription();
            else
            {
                var npcNews = hit.transform.GetComponent<NpcNews>();
                if (npcNews != null)
                    focussedText.text = npcNews.GetDescription();
            }
        }
        else
        {
            focussedText.text = "";
        }
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

    public void EnemyAttacks(EnemyAttack enemy, int charIndex) {
        // TODO: move all this logic to business.EnemyAttacksUseCase!!!
        var ac = Game.Instance.PartyStats.Chars[charIndex].ArmorClass;
        var chanceToHit = (5f + enemy.MonsterLevel * 2f) / (10f + enemy.MonsterLevel * 2f + ac);

        var message = "";
        if (Random.Range(0f, 1f) > chanceToHit)
        {
            var damage = Random.Range(enemy.DamageMin, enemy.DamageMax + 1);  // TODO: review damage formula
            message = string.Format("{0} hits {1} for {2} points", enemy.tag.TagToDescription(), Game.Instance.PartyStats.Chars[charIndex].Name, damage);
            Game.Instance.PartyStats.Chars[charIndex].HitPoints -= damage; // this code smells... this is a UseCase!
            partyHealthBehaviour.TakeHit(charIndex);
            charsPortraits[charIndex].SetHitPoints(Game.Instance.PartyStats.Chars[charIndex].HitPoints);
            charsPortraits[charIndex].ShowHitPortrait();
            if (Game.Instance.PartyStats.Chars[charIndex].HitPoints <= 0)
            {
                charsPortraits[charIndex].ConditionStatus = CharConditionStatus.Unconscious;
                message += " who gets unconscious";
            }
            // TODO: dead
        }
        else
        {
            message = string.Format("{0} misses {1}", enemy.tag.TagToDescription(), Game.Instance.PartyStats.Chars[charIndex].Name);
        }
        MessagesScroller.Instance.AddMessage(message);
    }

    public bool IsCharActive(int charIndex) {
        return charsPortraits[charIndex].IsCharActive();
    }

    public float GetDistanceSqrTo(Transform other) {
        return (transform.position - other.position).sqrMagnitude;
    }

    public void SpellBookCastSpell(SpellInfo spellInfo) 
    {
        if (spellInfo.NeedsPartyTarget || spellInfo.Needs3dTarget)
        {
            Time.timeScale = 0;
            FirstPersonController.Instance.SetCursorLock(false);
            spellChoosingTarget = spellInfo;
            // TODO: change cursor?
        }
        else
        {
            var view = transform.GetComponent<PartyAttack>();
            var partyCastsSpellUseCase = new PartyCastsSpellUseCase(this, transform);
            partyCastsSpellUseCase.CastSpell(Game.Instance.PartyStats.Chars[3], spellChoosingTarget); // TODO: selected char
        }
    }

    public void OnPortraitLeftClick(PlayingCharacter playingCharClicked)
    {
        if (spellChoosingTarget != null && spellChoosingTarget.NeedsPartyTarget)
        {
            var partyCastsSpellUseCase = new PartyCastsSpellUseCase(this, transform);

            Time.timeScale = 1;
            partyCastsSpellUseCase.CastSpell(Game.Instance.PartyStats.Chars[2], spellChoosingTarget, playingCharClicked); // TODO: selected char
            spellChoosingTarget = null;
            FirstPersonController.Instance.SetCursorLock(true);
        }
    }

    public void AddMessage(string message)
    {
        MessagesScroller.Instance.AddMessage(message);
    }

    public void ThrowSpell(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3? targetPoint, System.Action<Transform> onCollision)
    {
        partyAttackBehaviour.ThrowSpell(attackingChar, spellInfo, targetPoint, onCollision);
    }

    public void ShowPortraitSpellAnimation(PlayingCharacter target, SpellInfo spellInfo)
    {
        var i = Game.Instance.PartyStats.Chars.IndexOf(target);
        charsPortraits[i].ShowSpellAnimation(spellInfo);
    }

    public void UpdatePlayingCharacter(PlayingCharacter playingCharacter)
    {
        var i = Game.Instance.PartyStats.Chars.IndexOf(playingCharacter);
        charsPortraits[i].SetMaxHitPoints(playingCharacter.MaxHitPoints);
        charsPortraits[i].SetHitPoints(playingCharacter.HitPoints);
        charsPortraits[i].SetMaxSpellPoints(playingCharacter.MaxSpellPoints);
        charsPortraits[i].SetSpellPoints(playingCharacter.SpellPoints);
        charsPortraits[i].ConditionStatus = CharConditionStatus.Normal; // TODO: condition status
    }
}
