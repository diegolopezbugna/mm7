using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;

public class Chest : MonoBehaviour {

    [SerializeField]
    private int[] treasureLevels;

    [SerializeField]
    private int[] fixedItemIds;

    public List<Item> GeneratedItems = new List<Item>();

	void Start()
    {
        var openChestUseCase = new OpenChestUseCase();
        GeneratedItems = openChestUseCase.GetItems(treasureLevels, fixedItemIds);
	}
}
