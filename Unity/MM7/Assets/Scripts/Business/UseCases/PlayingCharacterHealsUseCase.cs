﻿using System;
using Infrastructure;

namespace Business
{
    public class PlayingCharacterHealsUseCase
    {
        private PlayingCharacterViewInterface View;
        private BuySellItemViewInterface BuySellItemView;

        public PlayingCharacterHealsUseCase(PlayingCharacterViewInterface view)
        {
            View = view;
        }

        public PlayingCharacterHealsUseCase(PlayingCharacterViewInterface view, BuySellItemViewInterface buySellItemView)
        {
            View = view;
            BuySellItemView = buySellItemView;
        }

        public void Heal(PlayingCharacter playingCharacter, int hitPointsToRecover) 
        {
            if (playingCharacter.ConditionStatus != ConditionStatus.Dead)
            {
                if (playingCharacter.MaxHitPoints - playingCharacter.HitPoints > hitPointsToRecover)
                    playingCharacter.HitPoints += hitPointsToRecover;
                else
                    playingCharacter.HitPoints = playingCharacter.MaxHitPoints;

                if (playingCharacter.ConditionStatus == ConditionStatus.Unconscious && playingCharacter.HitPoints > 0)
                    playingCharacter.ConditionStatus = ConditionStatus.Normal;

                View.UpdatePlayingCharacter(playingCharacter);
            }
        }

        public void HealResting(PlayingCharacter playingCharacter)
        {
            if (playingCharacter.ConditionStatus != ConditionStatus.Dead)
            {
                playingCharacter.ConditionStatus = ConditionStatus.Normal;
                playingCharacter.HitPoints = playingCharacter.MaxHitPoints;
                playingCharacter.SpellPoints = playingCharacter.MaxSpellPoints;
                View.UpdatePlayingCharacter(playingCharacter);
            }
        }

        public void HealAtHealer(int shopMultiplier, PlayingCharacter playingCharacter)
        {
            var cost = shopMultiplier;
            if (playingCharacter.ConditionStatus == ConditionStatus.Dead)
                cost = 20 * shopMultiplier; // TODO: update formula (time multiplier?)
            // TODO: more conditions

            if (Game.Instance.PartyStats.Gold >= cost)
            {
                Game.Instance.PartyStats.Gold -= cost;
                BuySellItemView.RefreshGoldAndFood();
                playingCharacter.ConditionStatus = ConditionStatus.Normal;
                playingCharacter.HitPoints = playingCharacter.MaxHitPoints;
                playingCharacter.SpellPoints = playingCharacter.MaxSpellPoints;
                View.UpdatePlayingCharacter(playingCharacter);
            }
            else
            {
                BuySellItemView.ShowError(Localization.Instance.Get("YouDontHaveEnoughMoney"));
            }

        }
    }
}

