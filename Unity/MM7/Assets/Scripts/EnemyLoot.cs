using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;

public class EnemyLoot : MonoBehaviour {

    [SerializeField]
    private int distanceSqrToLoot = 16;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && Party.Instance.CurrentTarget == this.transform)
        {
            float distanceToParty = Party.Instance.GetDistanceSqrTo(transform);
            if (distanceToParty < distanceSqrToLoot)
            {
                var enemyInfo = new EnemyInfo() { LootGoldMin = 3, LootGoldMax = 18, };         // TODO: get gold from monsters.txt
                var enemyLootUseCase = new EnemyLootUseCase(Party.Instance);
                enemyLootUseCase.Loot(enemyInfo);
                Destroy(gameObject);
            }
        }
	}
}
