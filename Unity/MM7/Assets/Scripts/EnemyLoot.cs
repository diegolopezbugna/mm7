using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;

public class EnemyLoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
    public void Loot()
    {
        var enemyInfo = new EnemyInfo() { LootGoldMin = 3, LootGoldMax = 18, };         // TODO: get gold from monsters.txt
        var enemyLootUseCase = new EnemyLootUseCase(Party.Instance);
        enemyLootUseCase.Loot(enemyInfo);
        Destroy(gameObject);
    }
}
