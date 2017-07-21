using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class PartyRangedAttack : MonoBehaviour {

    private const int CHARS = 4;

    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private Rigidbody arrow;

    [SerializeField]
    private float mediumRangeSqrDistance = 20 * 20; // TODO: check distances

    [SerializeField]
    private float longRangeSqrDistance = 40 * 40; // TODO: check distances

    private float[] lastAttack;
    private int lastCharAttacker = -1;

	// Use this for initialization
	void Start () {
        lastAttack = new float[4];
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
                if (Time.time - lastAttack[j] > Game.Instance.PartyStats.Chars[j].RecoveryTime)
                {
                    attackingChar = j;
                    break;
                }
            }
            if (attackingChar >= 0)
            {
                lastCharAttacker = attackingChar;
                DoThrowArrow(attackingChar, Party.Instance.CurrentTargetPoint, Party.Instance.CurrentTarget);
                lastAttack[attackingChar] = Time.time;
            }
        }
	}

    void DoThrowArrow(int charIndex, Vector3? targetPoint, Transform targetTransform) {
        var origin = transform.TransformPoint((charIndex - 1.5f) / 2f, 0.5f, 0);
        var a = Instantiate(arrow);

        var attackingChar = Game.Instance.PartyStats.Chars[charIndex];
        // TODO: player attacking status

        if (targetTransform != null && targetTransform.tag.StartsWith("Enemy"))
        {
            var enemyAttackBehaviour = targetTransform.GetComponent<EnemyAttack>();
            var monsterArmorClass = enemyAttackBehaviour.ArmorClass;
            var toHitAttackNumber = attackingChar.RangedAttackBonus * 2f + monsterArmorClass + 30f;
            var toHitDefenseNumber = (monsterArmorClass + 15f) * GetAttackDistanceMultiplier(targetTransform); 
            var didHit = Random.Range(1f, toHitAttackNumber) > Random.Range(1f, toHitDefenseNumber);
            if (!didHit)
            {
                MessagesScroller.Instance.AddMessage(string.Format("{0} misses {1}", attackingChar.Name, targetTransform.tag.TagToDescription()));
            }
            a.GetComponent<ArrowMove>().SetTarget(targetTransform, didHit, () =>
                {
                    var scriptHealth = targetTransform.GetComponent<EnemyHealth>();
                    if (scriptHealth != null) {
                        // TODO: physical resistance
                        var damage = Random.Range(attackingChar.RangedDamageMin, attackingChar.RangedDamageMax + 1);
                        MessagesScroller.Instance.AddMessage(string.Format("{0} hits {1} for {2} points", attackingChar.Name, targetTransform.tag.TagToDescription(), damage));
                        enemyAttackBehaviour.AlertOthers();
                        scriptHealth.TakeHit(damage);
                    }
                });
        }

        a.transform.position = origin;
        if (targetPoint.HasValue)
        {
            a.transform.LookAt(targetPoint.Value);
        }
        else
        {
            a.transform.rotation = transform.rotation;
        }

        // TODO: if enemy is moving, calc rotation to catch it
        a.velocity = a.transform.forward * 40f;
    }

    private float GetAttackDistanceMultiplier(Transform target) {
        var distanceToTarget = (target.position - transform.position).sqrMagnitude;
        if (distanceToTarget > longRangeSqrDistance)
            return 2f;
        else if (distanceToTarget > mediumRangeSqrDistance)
            return 1.5f;
        else
            return 1f;
    }
}
