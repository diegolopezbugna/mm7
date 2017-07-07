using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour {

    private NavMeshAgent agent;
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
        agent = GetComponent<NavMeshAgent>();
        wanderMoveBehaviour = GetComponent<RandomWanderMove>();
	}
	
	// Update is called once per frame
	void Update () {

        float distanceToParty = (Party.Instance.transform.position - transform.position).sqrMagnitude;
        isEngagingParty = distanceToParty < this.EngagingDistanceSqr && distanceToParty > 16; // TODO: attack distance
        // TODO: activar a los mounstros cercanos?

        if (isEngagingParty)
        {
            wanderMoveBehaviour.StopMoving();
            // ESTO SE EJECUTA TAMBIEN CUANDO ATACA!  JUNTAR ENEMYmove y ENEMYattack
            agent.SetDestination(Party.Instance.transform.position);
            agent.isStopped = false;
        }
        else if (distanceToParty > this.EngagingDistanceSqr)
        {
            wanderMoveBehaviour.StartMoving();
        }


	}

}
