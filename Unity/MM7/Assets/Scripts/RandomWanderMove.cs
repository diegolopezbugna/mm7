using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWanderMove : MonoBehaviour {

    [SerializeField]
    private int speed = 5;

    [SerializeField]
    private int maxdistance = 20;

    private Vector3 initialPosition;

    private Coroutine runningCorroutine;

    private float yOffsetFromTerrain;

    private Animator anim;

    private NavMeshAgent agent;

	// Use this for initialization
	void Start () 
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (agent != null)
        {
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(transform.position, out closestHit, 500f, NavMesh.AllAreas))
                agent.Warp(closestHit.position);
        }

        initialPosition = transform.position;
        if (Terrain.activeTerrain != null)
            yOffsetFromTerrain = transform.position.y - Terrain.activeTerrain.SampleHeight(transform.position);
        
        StartMoving();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (anim == null) // TODO: is this needed?
            anim = GetComponent<Animator>();
	}

    public void StartMoving() 
    {
        if (runningCorroutine == null)
            runningCorroutine = StartCoroutine(MoveArround());
    }

    public void StopMoving() 
    {
        StopMovingAnimations();

        if (agent != null)
            agent.isStopped = true;

        if (runningCorroutine != null)
            StopCoroutine(runningCorroutine);
        runningCorroutine = null;

    }

    IEnumerator MoveArround() 
    {
        StopMovingAnimations();
        yield return new WaitForSeconds(Random.Range(1f, 5f));

        if (enabled)
        {
            StartWalkingAnimation();
            Vector3 newDestination = GetNewDestination();

            if (agent != null)
            {
                agent.speed = this.speed;
                agent.SetDestination(newDestination);
                //while (agent.remainingDistance > 0.5f)
                while ((newDestination - transform.position).sqrMagnitude > 1f)
                {
                    yield return null;
                }
            }
            else
            {
                var lerpTime = 0f;
                while ((newDestination - transform.position).sqrMagnitude > 1f)
                {
                    lerpTime += Time.deltaTime * 2;
                    if (lerpTime < 1)
                    {
                        var newRotation = Quaternion.LookRotation(newDestination - transform.position);
                        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpTime);
                    }

                    transform.position = Vector3.MoveTowards(transform.position, newDestination, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }

        runningCorroutine = StartCoroutine(MoveArround());
    }

    Vector3 GetNewDestination() 
    {
        Vector3 newDestination;
        var newX = Random.Range(initialPosition.x - maxdistance, initialPosition.x + maxdistance);
        var newZ = Random.Range(initialPosition.z - maxdistance, initialPosition.z + maxdistance);

        if (agent != null)
        {
            newDestination = new Vector3(newX, initialPosition.y, newZ);
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(newDestination, out closestHit, 500f, NavMesh.AllAreas))
                newDestination = closestHit.position;
        }
        else
        {
            var newPositionXZSameY = new Vector3(newX, initialPosition.y, newZ);
            if (Terrain.activeTerrain != null)
            {
                var newY = Terrain.activeTerrain.SampleHeight(newPositionXZSameY) + yOffsetFromTerrain;
                newDestination = new Vector3(newX, newY, newZ);
            }
            else
            {
                newDestination = newPositionXZSameY;
            }
        }
        
        return newDestination;
    }

    void StopMovingAnimations()
    {
        // TODO: Use the NavMeshAgent.velocity as input to the Animator to roughly match the agent’s movement to the animations
        if (anim != null)
        {
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsRunning", false);
        }
    }

    void StartWalkingAnimation()
    {
        // TODO: Use the NavMeshAgent.velocity as input to the Animator to roughly match the agent’s movement to the animations
        if (anim != null)
        {
            anim.SetBool("IsWalking", true);
        }
    }
}
