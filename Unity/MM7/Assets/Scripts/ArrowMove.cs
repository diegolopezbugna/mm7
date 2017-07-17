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
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Target == null)
            return;

        var distanceToTarget = (Target.position - transform.position).sqrMagnitude;

        if (distanceToTarget < 1.0f || (distanceToTarget > previousDistanceToTarget && distanceToTarget > destroyDistance))
        {
            Destroy(this.gameObject);
            if (DidHit)
            {
                OnTargetReached();
            }
        }
        else if (DidHit)
        {
            transform.LookAt(Target);
            // TODO: actualizar trayectoria?
        }

        previousDistanceToTarget = distanceToTarget;
	}

    public void SetTarget(Transform target, bool didHit, Action onTargetReached) {
        Target = target;
        DidHit = didHit;
        OnTargetReached = onTargetReached;
    }
}
