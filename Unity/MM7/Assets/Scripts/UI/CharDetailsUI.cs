﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Business;
using UnityStandardAssets.Characters.FirstPerson;

public class CharDetailsUI : BaseUI<CharDetailsUI> {

    [SerializeField]
    private GameObject stats;

    [SerializeField]
    private GameObject skills;

    [SerializeField]
    private InventoryUI inventory;

    public void ShowStats(PlayingCharacter character) {
        if (!IsShowing)
            Show();
        stats.SetActive(true);
        skills.SetActive(false);
        inventory.gameObject.SetActive(false);
    }

    public void ShowSkills(PlayingCharacter character) {
        if (!IsShowing)
            Show();
        stats.SetActive(false);
        skills.SetActive(true);
        inventory.gameObject.SetActive(false);
    }

    public void ShowInventory(PlayingCharacter character) {
        if (!IsShowing)
            Show();
        stats.SetActive(false);
        skills.SetActive(false);
        inventory.gameObject.SetActive(true);
        inventory.DrawInventory(character.Inventory);
    }

}