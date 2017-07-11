using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {

    [SerializeField]
    private bool isRanged;

    [SerializeField]
    private float maxAttackDistanceSqr = 9;

    [SerializeField]
    private float minAttackDistanceSqr = 0;

    [SerializeField]
    private float damage = 10;

    private Animator animator;
    private NavMeshAgent agent;
    private BoxCollider[] weaponsColliders;

    private float lastAttack = 0;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        weaponsColliders = GetComponentsInChildren<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {

        float distanceToParty = (Party.Instance.transform.position - transform.position).sqrMagnitude;

        if (distanceToParty > minAttackDistanceSqr &&
            distanceToParty < maxAttackDistanceSqr)
        {
            Party.Instance.SetEnemyEngagingParty(this.gameObject, distanceToParty);
//            transform.LookAt(Party.Instance.transform);

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

    public float GetDamage(Party party) {
        // TODO: mejorar, verificar personaje al que le pega
        // TODO: caracteristicas
        return damage;
    }

    public void WeaponActivate() {
        foreach (var w in weaponsColliders)
            w.enabled = true;
    }

    public void WeaponDeactivate() {
        foreach (var w in weaponsColliders)
            w.enabled = false;
    }

}
