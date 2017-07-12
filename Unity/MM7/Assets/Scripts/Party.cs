using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : Singleton<Party> {

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

	// Use this for initialization
	void Start () {

        charsPortraits[0].SetPortraitImages(new string[] { "PC17-01" });
        charsPortraits[1].SetPortraitImages(new string[] { "PC04-01" });
        charsPortraits[2].SetPortraitImages(new string[] { "PC15-01" });
        charsPortraits[3].SetPortraitImages(new string[] { "PC11-01" });

        charsPortraits[0].SetMaxHitPoints(50);
        charsPortraits[0].SetHitPoints(50);
        charsPortraits[1].SetMaxHitPoints(40);
        charsPortraits[1].SetHitPoints(40);
        charsPortraits[2].SetMaxHitPoints(30);
        charsPortraits[2].SetHitPoints(30);
        charsPortraits[3].SetMaxHitPoints(20);
        charsPortraits[4].SetHitPoints(20);

        charsPortraits[0].SetMaxSpellPoints(0);
        charsPortraits[0].SetSpellPoints(0);
        charsPortraits[1].SetMaxSpellPoints(0);
        charsPortraits[1].SetSpellPoints(0);
        charsPortraits[2].SetMaxSpellPoints(20);
        charsPortraits[2].SetSpellPoints(20);
        charsPortraits[3].SetMaxSpellPoints(20);
        charsPortraits[3].SetSpellPoints(20);


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {
        bool isUserAttacking = Input.GetKeyDown("q");
        CalculateTarget(isUserAttacking);
    }

    void LateUpdate() {
        CheckEnemiesEngagingStatus();
    }

    void CalculateTarget(bool includeTargetPoint) {

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

    public void SetEnemyEngagingParty(GameObject enemy, float distanceSqr) {
        isEnemyEngagingPartyThisFrame = true;

        if (distanceSqr < handToHandCombatDistanceSqr)
            isEnemyInHandToHandCombatThisFrame = true;
    }

    private void CheckEnemiesEngagingStatus() {
        if (isEnemyInHandToHandCombatThisFrame)
        {
            foreach (var p in charsPortraits)
                p.SetStatus(CharPortraitStatus.Red);
        }
        else if (isEnemyEngagingPartyThisFrame)
        {
            foreach (var p in charsPortraits)
                p.SetStatus(CharPortraitStatus.Yellow);
        }
        else
        {
            foreach (var p in charsPortraits)
                p.SetStatus(CharPortraitStatus.Green);
        }
        isEnemyInHandToHandCombatThisFrame = false;
        isEnemyEngagingPartyThisFrame = false;
    }

}
