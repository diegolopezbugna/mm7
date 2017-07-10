using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyHealth : MonoBehaviour {

    [SerializeField]
    private Image blood;

    private Color initialBloodColor;

//    [SerializeField]
//    private GUITexture blood2;

	// Use this for initialization
	void Start () {
        initialBloodColor = blood.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {

        if (other.tag == "EnemyWeapon")
        {
            var attack = other.GetComponentsInParent<EnemyAttack>();
            attack[0].GetDamage(Party.Instance);
            // TODO: health

            StartCoroutine(TakeHit());
        }
    }

    IEnumerator TakeHit() {
        Debug.Log("HIT");

        Color bloodColor = initialBloodColor;
        blood.color = bloodColor;

        while (bloodColor.a < 0.8f)
        {
            bloodColor.a += 0.1f;
            blood.color = bloodColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while (bloodColor.a > 0f)
        {
            bloodColor.a -= 0.1f;
            blood.color = bloodColor;
            yield return null;
        }
    }
}
