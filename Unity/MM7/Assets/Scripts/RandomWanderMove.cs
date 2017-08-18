using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWanderMove : MonoBehaviour {

    [SerializeField]
    private int speed = 5;

    [SerializeField]
    private int maxdistance = 20;

    private Vector3 initialPosition;

    private Coroutine runningCorroutine;

    private float yOffsetFromTerrain;

    private Animator anim;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        yOffsetFromTerrain = transform.position.y - Terrain.activeTerrain.SampleHeight(transform.position);
        anim = GetComponent<Animator>();
        StartMoving();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim == null)
            anim = GetComponent<Animator>();
	}

    public void StartMoving() {
        if (runningCorroutine == null)
        {
            runningCorroutine = StartCoroutine(MoveArround());
        }
    }

    public void StopMoving() {
        anim.SetBool("IsWalking", false);
        if (runningCorroutine != null)
            StopCoroutine(runningCorroutine);
        runningCorroutine = null;
    }

    IEnumerator MoveArround() {
        if (anim != null)
            anim.SetBool("IsWalking", false);
        yield return new WaitForSeconds(Random.Range(1f, 5f));

        if (enabled)
        {
            Vector3 newDestination = GetNewDestination();

            var lerpTime = 0f;
            while ((newDestination - transform.localPosition).sqrMagnitude > 1f)
            {
                anim.SetBool("IsWalking", true);
                lerpTime += Time.deltaTime * 2;
                if (lerpTime < 1)
                {
                    var newRotation = Quaternion.LookRotation(newDestination - transform.localPosition);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, lerpTime);
                }

                transform.localPosition = Vector3.MoveTowards(transform.position, newDestination, speed * Time.deltaTime);
                yield return null;
            }
        }

        runningCorroutine = StartCoroutine(MoveArround());
    }

    Vector3 GetNewDestination() {
        var newX = Random.Range(initialPosition.x - maxdistance, initialPosition.x + maxdistance);
        var newZ = Random.Range(initialPosition.z - maxdistance, initialPosition.z + maxdistance);
        var newPositionBadY = new Vector3(newX, initialPosition.y, newZ);
        var newY = Terrain.activeTerrain.SampleHeight(newPositionBadY) + yOffsetFromTerrain;
        return new Vector3(newX, newY, newZ);
    }

}
