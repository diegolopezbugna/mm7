using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {

    [SerializeField]
    private bool isRanged;

    [SerializeField]
    private float maxAttackDistanceSqr = 20;

    [SerializeField]
    private float minAttackDistanceSqr = 0;

    private Animator animator;
    private NavMeshAgent agent;

    private float lastAttack = 0;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

        float distanceToParty = (Party.Instance.transform.position - transform.position).sqrMagnitude;

        if (distanceToParty > minAttackDistanceSqr &&
            distanceToParty < maxAttackDistanceSqr)
        {
            var currentTime = Time.time;
            if (currentTime - lastAttack > 2)
            {
                lastAttack = currentTime;
                if (agent.isActiveAndEnabled)
                    agent.isStopped = true;
                animator.SetTrigger("Attack");

                if (isRanged)
                {
                    // TODO: proyectiles?
                }
            }
        }

	}
}
