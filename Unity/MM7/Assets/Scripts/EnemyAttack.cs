using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [SerializeField]
    private bool isRanged;

    [SerializeField]
    private float maxAttackDistanceSqr = 9;

    [SerializeField]
    private float minAttackDistanceSqr = 0;

    [SerializeField]
    private float engagingSpeed = 10;

    [SerializeField]
    private float alertOthersDistanceSqr = 20 * 20;

    [SerializeField]
    private float attackAnimationPartyHitDuration = 0.5f;

    [SerializeField]
    private int monsterLevel;
    public int MonsterLevel { get { return monsterLevel; } }

    [SerializeField]
    private int damageMin;
    public int DamageMin { get { return damageMin; } }

    [SerializeField]
    private int damageMax;
    public int DamageMax { get { return damageMax; } }

    [SerializeField]
    private int armorClass = 0;
    public int ArmorClass { get { return armorClass; } }

    private Animator animator;
    private RandomWanderMove wanderMoveBehaviour;
    private float lastAttack = 0;
    private bool isEngagingParty = false;

    private const int engagingDistanceSqr = 20 * 20;
    public float EngagingDistanceSqr {
        get {
            return engagingDistanceSqr;
        }
    }

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        wanderMoveBehaviour = GetComponent<RandomWanderMove>();
	}
	
	// Update is called once per frame
	void Update () {

        float distanceToParty = Party.Instance.GetDistanceSqrTo(transform);

        if (distanceToParty > this.EngagingDistanceSqr && !isEngagingParty)
        {
            wanderMoveBehaviour.StartMoving();
        }
        else
        {
            Party.Instance.SetEnemyEngagingParty(this.gameObject, distanceToParty);
            wanderMoveBehaviour.StopMoving();
            transform.LookAt(Party.Instance.transform); // TODO: smooth

            if (distanceToParty > maxAttackDistanceSqr)
            {
                transform.localPosition = Vector3.MoveTowards(transform.position, Party.Instance.transform.position, engagingSpeed * Time.deltaTime);
            }
            else
            {
                var currentTime = Time.time;
                if (currentTime - lastAttack > 2f)
                {
                    lastAttack = currentTime;
                    StartCoroutine(AttackParty());
                }
            }
        }
	}

    IEnumerator AttackParty() {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackAnimationPartyHitDuration);
        var charAttacked = GetCharAttacked();
        if (charAttacked >= 0)
        {
            Party.Instance.EnemyAttacks(this, charAttacked);
            if (isRanged)
            {
                // TODO: proyectiles?
            }
        }
        else
        {
            // TODO: can't attack
        }
    }

    private int GetCharAttacked() {
        var charAttacked = Random.Range(0, 4);
        int i = 0;
        while (!Party.Instance.IsCharActive(charAttacked))
        {
            if (i == 20)
                return -1;
            charAttacked = Random.Range(0, 4);
            i++;
        }
        return charAttacked;
    }

    public void ForceEngageParty() {
        isEngagingParty = true;
    }

    public void AlertOthers() {
        ForceEngageParty();
        var distanceToPartySqr = Party.Instance.GetDistanceSqrTo(transform);
        if (distanceToPartySqr > this.EngagingDistanceSqr)
        {
            var allEnemies = GameObject.FindObjectsOfType<EnemyAttack>();
            foreach (var e in allEnemies)
            {
                if ((transform.position - e.transform.position).sqrMagnitude < alertOthersDistanceSqr)
                    e.ForceEngageParty();
            }
        }
    }

}
