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
    private float damage = 10;

    [SerializeField]
    private float engagingSpeed = 10;

    private Animator animator;

    private float lastAttack = 0;

    private RandomWanderMove wanderMoveBehaviour;

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

        float distanceToParty = (Party.Instance.transform.position - transform.position).sqrMagnitude;

        if (distanceToParty > minAttackDistanceSqr &&
            distanceToParty < maxAttackDistanceSqr)
        {
            Party.Instance.SetEnemyEngagingParty(this.gameObject, distanceToParty);
            transform.LookAt(Party.Instance.transform); // TODO: smooth

            var currentTime = Time.time;
            if (currentTime - lastAttack > 2f)
            {
                lastAttack = currentTime;
                // STOP MOVE!
                animator.SetTrigger("Attack");

                if (isRanged)
                {
                    // TODO: proyectiles?
                }
            }
        }
        else if (distanceToParty < this.EngagingDistanceSqr && distanceToParty > maxAttackDistanceSqr) {
            Party.Instance.SetEnemyEngagingParty(this.gameObject, distanceToParty);

            wanderMoveBehaviour.StopMoving();
            transform.LookAt(Party.Instance.transform);
            // TODO: smooth rotation and acceleration
            transform.localPosition = Vector3.MoveTowards(transform.position, Party.Instance.transform.position, engagingSpeed * Time.deltaTime);
        }
        else if (distanceToParty > this.EngagingDistanceSqr)
        {
            wanderMoveBehaviour.StartMoving();
        }

	}

    public float GetDamage(Party party) {
        // TODO: mejorar, verificar personaje al que le pega
        // TODO: caracteristicas
        return damage;
    }
}
