using System;
using UnityEngine;
using Infrastructure;

namespace Business
{
    public class EnemyLootUseCase
    {
        private PlayingCharacterViewInterface View;

        public EnemyLootUseCase(PlayingCharacterViewInterface view)
        {
            View = view;
        }

        public void Loot(EnemyInfo enemyInfo)
        {
            // TODO: loot items
            var gold = UnityEngine.Random.Range(enemyInfo.LootGoldMin, enemyInfo.LootGoldMax + 1);
            Game.Instance.PartyStats.Gold += gold;
            View.RefreshGoldAndFood();
            View.PlayGoldSound();
            View.ShowMessage(Localization.Instance.Get("YouFoundXGold", gold));
        }
    }
}

