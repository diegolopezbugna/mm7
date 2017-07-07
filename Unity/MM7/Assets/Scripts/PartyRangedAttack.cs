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
            DoThrowArrow(Party.Instance.CurrentTargetPoint);
        }
	}

    void DoThrowArrow(Vector3? target) {
        // TODO: que salga de una distinta posición según el personaje
        var origin = transform.TransformPoint(0, 0.5f, 0);
        var a = Instantiate(arrow);
        a.transform.position = origin;
        if (target.HasValue)
        {
            a.transform.LookAt(target.Value);
        }
        else
        {
            a.transform.rotation = transform.rotation;
        }

        a.velocity = a.transform.forward * 20f;
    }
}
