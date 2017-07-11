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
    private RawImage[] c1StatusImages;

    [SerializeField]
    private RawImage[] c2StatusImages;

    [SerializeField]
    private RawImage[] c3StatusImages;

    [SerializeField]
    private RawImage[] c4StatusImages;

    [SerializeField]
    private Slider c1HitPoints;

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
        c1HitPoints.minValue = 0;
        c1HitPoints.maxValue = 50; //chat HPs 
        c1HitPoints.value = 10;
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
        // TODO cuerpo a cuerpo?
        isEnemyEngagingPartyThisFrame = true;

        if (distanceSqr < handToHandCombatDistanceSqr)
            isEnemyInHandToHandCombatThisFrame = true;
    }

    private void CheckEnemiesEngagingStatus() {
        if (isEnemyInHandToHandCombatThisFrame)
        {
            SetCharactersEngagingStatus(0, false);
            SetCharactersEngagingStatus(1, false);
            SetCharactersEngagingStatus(2, true);
        }
        else if (isEnemyEngagingPartyThisFrame)
        {
            SetCharactersEngagingStatus(0, false);
            SetCharactersEngagingStatus(1, true);
            SetCharactersEngagingStatus(2, false);
        }
        else
        {
            SetCharactersEngagingStatus(0, true);
            SetCharactersEngagingStatus(1, false);
            SetCharactersEngagingStatus(2, false);
        }
        isEnemyInHandToHandCombatThisFrame = false;
        isEnemyEngagingPartyThisFrame = false;
    }

    private void SetCharactersEngagingStatus(int statusIndex, bool value)
    {
        c1StatusImages[statusIndex].enabled = value;
        c2StatusImages[statusIndex].enabled = value;
        c3StatusImages[statusIndex].enabled = value;
        c4StatusImages[statusIndex].enabled = value;
    }
}
