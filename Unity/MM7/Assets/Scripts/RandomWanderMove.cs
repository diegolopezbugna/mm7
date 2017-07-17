using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWanderMove : MonoBehaviour {

    [SerializeField]
    private int speed = 5;

    [SerializeField]
    private int maxdistance = 20;

    private Vector3 initialPosition;

    private IEnumerator runningCorroutine;

	// Use this for initialization
	void Start () {
        initialPosition = transform.localPosition;
        StartMoving();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void StartMoving() {
        if (runningCorroutine == null)
        {
            runningCorroutine = MoveArround();
            StartCoroutine(runningCorroutine);
        }
    }

    public void StopMoving() {
        if (runningCorroutine != null)
            StopCoroutine(runningCorroutine);
        runningCorroutine = null;
    }

    IEnumerator MoveArround() {
        yield return new WaitForSeconds(Random.Range(1f, 5f));

        if (enabled)
        {
            Vector3 newDestination = GetNewDestination();

            var lerpTime = 0f;
            while ((newDestination - transform.localPosition).sqrMagnitude > 1f)
            {
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

        StartCoroutine(MoveArround());
    }

    Vector3 GetNewDestination() {
        var newX = Random.Range(initialPosition.x - maxdistance, initialPosition.x + maxdistance);
        var newZ = Random.Range(initialPosition.z - maxdistance, initialPosition.z + maxdistance);
        return new Vector3(newX, initialPosition.y, newZ);  // TODO: ojo que no cambie la y... raycast con el terreno?
    }

}
