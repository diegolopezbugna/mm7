using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyRangedAttack : MonoBehaviour {

    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private Rigidbody arrow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("q"))
        {
            DoThrowArrow(Party.Instance.CurrentTargetPoint, Party.Instance.CurrentTarget);
        }
	}

    void DoThrowArrow(Vector3? targetPoint, Transform targetTransform) {
        // TODO: que salga de una distinta posición según el personaje
        var origin = transform.TransformPoint(0, 0.5f, 0);
        var a = Instantiate(arrow);
		var didHit = targetTransform != null && targetTransform.tag.StartsWith("Enemy");
		a.GetComponent<ArrowMove>().SetTarget(targetTransform, didHit, () =>
            {
				targetTransform.GetComponent<EnemyHealth>().TakeHit(10);
            });
        a.transform.position = origin;
        if (targetPoint.HasValue)
        {
            a.transform.LookAt(targetPoint.Value);
        }
        else
        {
            a.transform.rotation = transform.rotation;
        }

        a.velocity = a.transform.forward * 20f;
    }
}
