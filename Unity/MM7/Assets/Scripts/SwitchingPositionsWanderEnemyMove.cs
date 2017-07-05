using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SwitchingPositionsWanderEnemyMove : MonoBehaviour {

    [SerializeField]
    private float speed = 2;

    private Vector3? currentDestination;
    private IList<Vector3> destinations;

	// Use this for initialization
	void Start () {
        SetDestinations();

        StartCoroutine(MoveArround());
	}
	
	// Update is called once per frame
	void Update () {
        if (currentDestination.HasValue)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentDestination.Value, speed * Time.deltaTime);
	}

    void FixedUpdate() {
    }

    void SetDestinations () {
        destinations = new List<Vector3>();
        var flyingEnemies = Object.FindObjectsOfType<SwitchingPositionsWanderEnemyMove>();
        foreach (var e in flyingEnemies)
            destinations.Add(e.transform.localPosition);
    }

    IEnumerator MoveArround() {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        SetNewWanderDestination();
        if (currentDestination.HasValue)
            transform.LookAt(currentDestination.Value);
        StartCoroutine(MoveArround());
    }

    void SetNewWanderDestination() {
        var i = Random.Range(0, destinations.Count);
        currentDestination = destinations[i];
        Debug.LogFormat("destination: {0}", currentDestination);
    }
}
