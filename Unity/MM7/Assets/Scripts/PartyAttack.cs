﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class PartyAttack : MonoBehaviour, PartyAttacksViewInterface {

    private const int CHARS = 4;

    [SerializeField]
    private Rigidbody arrow;

    [SerializeField]
    private GameObject[] weaponContainers;

    private float[] lastAttack;
    private int lastCharAttacker = -1;
    private List<Animator> weaponAnimators = new List<Animator>();

	// Use this for initialization
	void Start () {
        lastAttack = new float[4];

        foreach (var wc in weaponContainers)
        {
            weaponAnimators.Add(wc.GetComponentInChildren<Animator>());
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("q"))
        {
            // TODO: select player
            var attackingChar = -1;
            var newCharAttacker = lastCharAttacker + 1;
            for (int i = newCharAttacker; i < newCharAttacker + CHARS; i++)
            {
                var j = i % 4;
                if (Time.time - lastAttack[j] > Game.Instance.PartyStats.Chars[j].RecoveryTime && 
                    Party.Instance.IsCharActive(j))
                {
                    attackingChar = j;
                    break;
                }
            }
            if (attackingChar >= 0)
            {
                lastCharAttacker = attackingChar;
                DoAttack(attackingChar, Party.Instance.CurrentTargetPoint, Party.Instance.CurrentTarget);
                lastAttack[attackingChar] = Time.time;
            }

        }
	}

    void DoAttack(int charIndex, Vector3? targetPoint, Transform targetTransform) {
        var attackingChar = Game.Instance.PartyStats.Chars[charIndex];
        var partyAttacksUseCase = new PartyAttacksUseCase(this, targetPoint, targetTransform, Party.Instance.transform);

        if (targetTransform != null && targetTransform.tag.StartsWith("Enemy"))
        {
            var enemyAttackBehaviour = targetTransform.GetComponent<EnemyAttack>();
            if (enemyAttackBehaviour != null) // TODO: prevent attacking a dying enemy
            {
                partyAttacksUseCase.TryHit(attackingChar, enemyAttackBehaviour.ArmorClass);
            }
        }
        else
        {
            partyAttacksUseCase.HitNothing(attackingChar);
        }
    }

    public void ThrowArrowToTarget(PlayingCharacter attackingChar, Transform targetTransform,  Vector3 targetPoint, bool didHit, int damage) {
        int charIndex = Game.Instance.PartyStats.Chars.IndexOf(attackingChar);
        var origin = transform.TransformPoint((charIndex - 1.5f) / 2f, 0.5f, 0);
        var a = Instantiate(arrow);

        var enemyAttackBehaviour = targetTransform.GetComponent<EnemyAttack>();
        if (enemyAttackBehaviour != null) // TODO: prevent attacking a dying enemy
        {
            a.GetComponent<ArrowMove>().SetTarget(targetTransform, didHit, () =>
                {
                    var scriptHealth = targetTransform.GetComponent<EnemyHealth>();
                    if (scriptHealth != null && scriptHealth.IsActive())
                    {
                        // TODO: move some of this logic to the useCase?
                        enemyAttackBehaviour.AlertOthers();
                        scriptHealth.TakeHit(damage);
                    }
                });
        }

        a.transform.position = origin;
        a.transform.LookAt(targetPoint);
        // TODO: if enemy is moving, calc rotation to catch it
        a.velocity = a.transform.forward * 40f;
    }

    public void ThrowArrowToNonInteractiveObjects(PlayingCharacter attackingChar, Vector3? targetPoint) {
        int charIndex = Game.Instance.PartyStats.Chars.IndexOf(attackingChar);
        var origin = transform.TransformPoint((charIndex - 1.5f) / 2f, 0.5f, 0);
        var a = Instantiate(arrow);

        a.transform.position = origin;
        if (targetPoint.HasValue)
            a.transform.LookAt(targetPoint.Value);
        else
            a.transform.rotation = transform.rotation;

        a.velocity = a.transform.forward * 40f;
    }

    public void HandToHandAttack(PlayingCharacter attackingChar, Transform targetTransform, bool didHit, int damage) {
        int charIndex = Game.Instance.PartyStats.Chars.IndexOf(attackingChar);
        weaponAnimators[charIndex].SetTrigger("Attack");

        if (targetTransform != null)
        {
            if (didHit)
            {
                var scriptHealth = targetTransform.GetComponent<EnemyHealth>();
                if (scriptHealth != null && scriptHealth.IsActive())
                {
                    // TODO: move some of this logic to the useCase?
                    scriptHealth.TakeHit(damage);
                }
            }
        }
    }

    public void AddMessage(string message) {
        MessagesScroller.Instance.AddMessage(message);
    }
}