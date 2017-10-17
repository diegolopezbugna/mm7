using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;
using UnityEngine.AI;

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

    private EnemyInfo enemyInfo;
    public EnemyInfo EnemyInfo {
        get {
            if (enemyInfo == null)
            {
                // TODO: read this from monsters.txt
                enemyInfo = new EnemyInfo()
                    { 
                        Armor = ArmorClass, 
                        DamageMin = this.DamageMin, 
                        DamageMax = this.DamageMax,
                        MonsterLevel = this.MonsterLevel,
                        Name = this.tag.TagToDescription(),
                        LootGoldMin = 3,
                        LootGoldMax = 18,
                    };
            }
            return enemyInfo;
        }
    }

    private Animator animator;
    private RandomWanderMove wanderMoveBehaviour;
    private NavMeshAgent agent;
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
        agent = GetComponent<NavMeshAgent>();
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

            if (agent == null)
            {
                var partyPostitionToLookAt = new Vector3(Party.Instance.transform.position.x, transform.position.y, Party.Instance.transform.position.z);
                transform.LookAt(partyPostitionToLookAt); // TODO: smooth
            }

            if (distanceToParty > maxAttackDistanceSqr)
            {
                animator.SetBool("IsRunning", true);
                if (agent != null)
                {
                    agent.speed = engagingSpeed;
                    agent.SetDestination(Party.Instance.transform.position);
                    agent.isStopped = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, Party.Instance.transform.position, engagingSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (agent != null)
                {
                    agent.isStopped = true;
                    var partyPostitionToLookAt = new Vector3(Party.Instance.transform.position.x, transform.position.y, Party.Instance.transform.position.z);
                    transform.LookAt(partyPostitionToLookAt); // TODO: smooth
                }

                var currentTime = Time.time;
                if (currentTime - lastAttack > 2f)  // TODO: enemy recovery time
                {
                    lastAttack = currentTime;
                    StartCoroutine(AttackParty());
                }
            }
        }
	}

    IEnumerator AttackParty() {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackAnimationPartyHitDuration);
        var charAttacked = GetCharAttacked();
        if (charAttacked >= 0)
        {
            var enemyAttacksUseCase = new EnemyAttacksUseCase(Party.Instance, Party.Instance);
            enemyAttacksUseCase.EnemyAttacks(EnemyInfo, Game.Instance.PartyStats.Chars[charAttacked]);

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
        while (!Game.Instance.PartyStats.Chars[charAttacked].IsActive)
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
