using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    private int health;

    private NavMeshAgent agent;
    private Animator animator;
    private ParticleSystem blood;
    private EnemyMove enemyMoveBehaviour;
    private EnemyAttack enemyAttackBehaviour;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        enemyMoveBehaviour = GetComponent<EnemyMove>();
        enemyAttackBehaviour = GetComponent<EnemyAttack>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter");

        if (other.tag == "Arrow")
        {
            //agent.isStopped = true;
            Destroy(other.gameObject);

            health -= 10; //other.getDamageFor(this.gameObject);
            blood.Play();

            // TODO: sacar este if usando doble dispatch
            if (health > 0)
            {
                animator.SetTrigger("Hurt");
            }
            else
            {
                animator.SetTrigger("Die");
                Destroy(enemyMoveBehaviour);
                Destroy(enemyAttackBehaviour);
                agent.enabled = false;
                foreach (var c in GetComponents<Collider>())
                {
                    c.enabled = false;
                }
            }

        }

    }
}
