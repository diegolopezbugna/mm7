using System;
using Infrastructure;
using UnityEngine;

namespace Business
{
    public class RentRoomUseCase
    {
        private BuySellItemViewInterface View;
        private PartyRestsViewInterface PartyRestsView;
        private PlayingCharacterViewInterface PlayingCharacterView;

        public RentRoomUseCase(BuySellItemViewInterface view, PartyRestsViewInterface partyRestsView, PlayingCharacterViewInterface playingCharacterView)
        {
            View = view;
            PartyRestsView = partyRestsView;
            PlayingCharacterView = playingCharacterView;
        }

        public void RentRoom(float shopMultiplier, PlayingCharacter buyer)
        {
            var rentCost = Mathf.CeilToInt(shopMultiplier / 2); // TODO: reduced price by merchant bonus
            if (Game.Instance.PartyStats.Gold >= rentCost)
            {
                Game.Instance.PartyStats.Gold -= rentCost;
                PlayingCharacterView.RefreshGoldAndFood();
                View.Hide();
                PartyRestsView.Show();
                var partyRestsUseCase = new PartyRestsUseCase(PartyRestsView, PlayingCharacterView);
                partyRestsUseCase.RestAndHeal(true);
            }
            else
            {
                View.ShowError(Localization.Instance.Get("ImSorryButYouDontHaveEnoughMoney", buyer.Name));
            }
        }

        public void BuyFood(float shopMultiplier, PlayingCharacter buyer)
        {
            var foodCost = Mathf.CeilToInt(shopMultiplier / 3); // TODO: reduced price by merchant bonus
            if (Game.Instance.PartyStats.Gold >= foodCost)
            {
                var foodToBuy = Mathf.CeilToInt(shopMultiplier);
                if (Game.Instance.PartyStats.Food < foodToBuy)
                {
                    Game.Instance.PartyStats.Food += foodToBuy;
                    Game.Instance.PartyStats.Gold -= foodCost; // TODO: reduced price by merchant bonus
                    PlayingCharacterView.RefreshGoldAndFood();
                }
                else
                {
                    View.ShowError(Localization.Instance.Get("YourPacksAreFull"));
                }
            }
            else
            {
                View.ShowError(Localization.Instance.Get("ImSorryButYouDontHaveEnoughMoney", buyer.Name));
            }
        }
    }
}

