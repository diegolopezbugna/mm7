using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class NpcMove : MonoBehaviour {

    [SerializeField]
    private float distanceSqrToLookAtParty = 4;

    private RandomWanderMove randomWanderMove;
    private bool isLookingAtPlayer = false;
    private Animator anim;

	// Use this for initialization
	void Start () {
        randomWanderMove = GetComponent<RandomWanderMove>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim == null)
            anim = GetComponent<Animator>();
		
        if (Party.Instance.GetDistanceSqrTo(transform) < distanceSqrToLookAtParty)
        {
            randomWanderMove.StopMoving();
            isLookingAtPlayer = true;

            //transform.LookAt(FirstPersonController.Instance.transform);
            anim.SetBool("IsIdleLooking", true);
            LookAt(FirstPersonController.Instance.transform);
        }
        else
        {
            if (isLookingAtPlayer)
            {
                isLookingAtPlayer = false;
                anim.SetBool("IsIdleLooking", false);
                randomWanderMove.StartMoving();
            }
        }

	}

    private void LookAt(Transform transfromTo) {
        // TODO: entender bien por qué no funciona esto
//        var rotFrom = transform.rotation;
//        var rotTo = Quaternion.Euler(-transfromTo.rotation.eulerAngles);
//        //transform.rotation = Quaternion.Lerp(rotFrom, rotTo, Time.deltaTime * 10);
//        transform.rotation = rotTo;

        Vector3 targetVector = transfromTo.position - transform.position;
        targetVector.y = 0;
        float angle = Vector3.Angle(Vector3.forward, targetVector);
        Vector3 cross = Vector3.Cross(Vector3.forward, targetVector);
        if (cross.y < 0)
            angle *= -1;
        Vector3 finalVector = transform.rotation.eulerAngles;
        finalVector.y = angle;
        Quaternion rotTo = Quaternion.Euler(finalVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotTo, Time.deltaTime * 10);

    }
}
