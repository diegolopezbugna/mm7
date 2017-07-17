using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    private int health;

    private Animator animator;
    private ParticleSystem blood;
    private EnemyAttack enemyAttackBehaviour;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        enemyAttackBehaviour = GetComponent<EnemyAttack>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeHit(int damage) {
        health -= damage; //other.getDamageFor(this.gameObject);
        blood.Play();

        // TODO: sacar este if usando doble dispatch
        if (health > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            animator.SetTrigger("Die");
            Destroy(enemyAttackBehaviour);
            foreach (var c in GetComponents<Collider>())
            {
               
                //c.enabled = false;
            }
        }
    }
}
