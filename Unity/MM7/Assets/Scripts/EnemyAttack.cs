using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public enum EnemyAttackState {
    FarAway,
    EngagingParty,
    AttackingParty
}

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

    private TurnBasedController turnBasedController;

    private const int engagingDistanceSqr = 20 * 20;
    public float EngagingDistanceSqr {
        get {
            return engagingDistanceSqr;
        }
    }

    public EnemyAttackState state = EnemyAttackState.FarAway;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        wanderMoveBehaviour = GetComponent<RandomWanderMove>();
        agent = GetComponent<NavMeshAgent>();
        turnBasedController = FindObjectOfType<TurnBasedController>();
	}
	
	// Update is called once per frame
	void Update () {
        float distanceToParty = Party.Instance.GetDistanceSqrTo(transform);

        if (distanceToParty > this.EngagingDistanceSqr && state != EnemyAttackState.EngagingParty)
        {
            state = EnemyAttackState.FarAway;
            wanderMoveBehaviour.StartMoving();
        }
        else
        {
            Party.Instance.SetEnemyEngagingParty(this.gameObject, distanceToParty);
            wanderMoveBehaviour.StopMoving();
            LookAtParty();

            if (distanceToParty > maxAttackDistanceSqr)
                EngageParty();
            else
                StartAttackingParty();
        }
	}

    void EngageParty() {
        state = EnemyAttackState.EngagingParty;
        animator.SetBool("IsRunning", true);
        agent.speed = engagingSpeed;
        agent.SetDestination(FirstPersonController.Instance.transform.position);
        agent.isStopped = false;
    }

    void LookAtParty() {
        var partyPostitionToLookAt = new Vector3(FirstPersonController.Instance.transform.position.x, transform.position.y, FirstPersonController.Instance.transform.position.z);
        transform.LookAt(partyPostitionToLookAt); // TODO: smooth
    }

    void StartAttackingParty() {
        state = EnemyAttackState.AttackingParty;

        agent.isStopped = true;
        LookAtParty();

        var currentTime = Time.time;
        if (currentTime - lastAttack > 4f)  // TODO: enemy recovery time
        {
            lastAttack = currentTime;
            StartCoroutine(AttackParty());
        }
    }

    IEnumerator AttackParty() {
        // TODO: proyectiles?
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackAnimationPartyHitDuration);
        var charAttacked = GetCharAttacked();
        if (charAttacked >= 0)
        {
            var enemyAttacksUseCase = new EnemyAttacksUseCase(Party.Instance, Party.Instance);
            enemyAttacksUseCase.EnemyAttacks(EnemyInfo, Game.Instance.PartyStats.Chars[charAttacked]);
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
        state = EnemyAttackState.EngagingParty;
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
