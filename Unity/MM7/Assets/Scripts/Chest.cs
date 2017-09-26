using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;

public class Chest : MonoBehaviour {

    [SerializeField]
    private int distanceSqrToLoot = 16;

    [SerializeField]
    private int[] treasureLevels;

    [SerializeField]
    private int[] fixedItemIds;

    private List<Item> generatedItems;

	void Start()
    {
        var openChestUseCase = new OpenChestUseCase();
        generatedItems = openChestUseCase.GetItems(treasureLevels, fixedItemIds);
	}
	
	void Update()
    {
        if (Input.GetMouseButtonDown(0) && Party.Instance.CurrentTarget == this.transform && !OpenChestUI.Instance.IsShowing)
        {
            float distanceToParty = Party.Instance.GetDistanceSqrTo(transform);
            if (distanceToParty < distanceSqrToLoot)
            {
                // TODO: which chest background?
                OpenChestUI.Instance.Show(generatedItems);
            }
        }
	}
}
