using System;

namespace Business
{
    public class PartyRestsUseCase
    {
        public const int FOOD_COST_TO_REST = 2;
        
        private PartyRestsViewInterface View;
        private PlayingCharacterViewInterface PlayingCharacterView;

        public PartyRestsUseCase(PartyRestsViewInterface view, PlayingCharacterViewInterface playingCharacterView)
        {
            View = view;
            PlayingCharacterView = playingCharacterView;
        }

        public void RestAndHeal(bool atInn)
        {
            // TODO: can't rest when enemies are nearby
            foreach (var c in Game.Instance.PartyStats.Chars)
            {
                if (c.ConditionStatus != ConditionStatus.Dead && c.ConditionStatus != ConditionStatus.Unconscious)
                {
                    c.ConditionStatus = ConditionStatus.Sleeping;
                    PlayingCharacterView.UpdatePlayingCharacter(c);
                }
            }

            if (!atInn)
            {
                Game.Instance.PartyStats.Food -= FOOD_COST_TO_REST;
                if (Game.Instance.PartyStats.Food < 0)
                {
                    Game.Instance.PartyStats.Food = 0;
                    // TODO: starving
                }
            }

            var playingCharacterHealsUseCase = new PlayingCharacterHealsUseCase(PlayingCharacterView);

            View.WaitTime(8f, () =>
                {
                    foreach (var c in Game.Instance.PartyStats.Chars)
                        playingCharacterHealsUseCase.HealResting(c);
                    View.Hide();
                });
        }

    }
}

