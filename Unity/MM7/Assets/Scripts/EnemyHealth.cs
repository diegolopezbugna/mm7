using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    public int hitPoints;

    [SerializeField]
    private int experience;

    [SerializeField]
    private float dieAnimationDuration = 0.5f;

    [SerializeField]
    private float dieMoveToFloorSpeed = 3;

    private Animator animator;
    private ParticleSystem blood;
    private EnemyAttack enemyAttackBehaviour;
    private RandomWanderMove enemyRandomWanderMoveBehaviour;
    private EnemyLoot enemyLootBehaviour;

    public int MaxHitPoints { get; private set; }  // TODO: move to Business.Enemy?

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        enemyAttackBehaviour = GetComponent<EnemyAttack>();
        enemyRandomWanderMoveBehaviour = GetComponent<RandomWanderMove>();
        enemyLootBehaviour = GetComponent<EnemyLoot>();
        if (enemyLootBehaviour != null)
            enemyLootBehaviour.enabled = false;
        MaxHitPoints = hitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeHit(int damage) {
        if (damage <= 0)
            return;

        hitPoints -= damage; //other.getDamageFor(this.gameObject);
        blood.Play();

        // TODO: sacar este if usando doble dispatch
        if (hitPoints > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            animator.SetTrigger("Die");
            var colliders = GetComponents<Collider>();
            if (colliders != null && colliders.Length > 0)
            {
                Destroy(colliders[0]);
                if (colliders.Length > 1)
                    colliders[1].enabled = true;
            }
            Destroy(enemyAttackBehaviour);
            Destroy(enemyRandomWanderMoveBehaviour);
            if (enemyLootBehaviour != null)
                enemyLootBehaviour.enabled = true;
            MoveToFloor();
        }
    }

    void MoveToFloor() {
        var rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        RaycastHit hitInfo;
        LayerMask floorLayer = 1 << 9;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 500f, floorLayer))
        {
            Debug.Log("MoveToFloor");
            StartCoroutine(DoMoveToFloor(hitInfo.point));
        }
    }

    IEnumerator DoMoveToFloor(Vector3 floorPoint) {
        yield return new WaitForSeconds(dieAnimationDuration);
        while (transform.position.y > floorPoint.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, floorPoint, Time.deltaTime * dieMoveToFloorSpeed);
            yield return null;
        }
        Destroy(this);
    }

    public bool IsActive() {
        return hitPoints > 0;
    }
}
