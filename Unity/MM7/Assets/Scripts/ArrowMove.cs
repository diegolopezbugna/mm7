using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour {

    private Transform Target { get; set; }
    private bool DidHit { get; set; }
    private Action OnTargetReached { get; set; }

    private float previousDistanceToTarget;
    private float destroyDistance = 500f;

	// Use this for initialization
	void Start () {
		previousDistanceToTarget = float.MaxValue;
	}
	
	// Update is called once per frame
	void Update () {
		if (Target == null || !DidHit) {
            var distanceToParty = Party.Instance.GetDistanceSqrTo(transform);
			if (distanceToParty > destroyDistance) {
				Destroy(this);
				Destroy(this.gameObject);
			}
			return;
		}
			
        var distanceToTarget = (Target.position - transform.position).sqrMagnitude;
        if (distanceToTarget < 0.1f || distanceToTarget > previousDistanceToTarget)
        {
            Destroy(this.gameObject);
            OnTargetReached();
        }

        previousDistanceToTarget = distanceToTarget;
	}

    public void SetTarget(Transform target, bool didHit, Action onTargetReached) {
        Target = target;
        DidHit = didHit;
        OnTargetReached = onTargetReached;
    }
}
