using System;

namespace Business
{
    public class PartyRestsUseCase
    {
        private RestUseCaseViewInterface View;
        private PlayingCharacterViewInterface PlayingCharacterView;

        public PartyRestsUseCase(RestUseCaseViewInterface view, PlayingCharacterViewInterface playingCharacterView)
        {
            View = view;
            PlayingCharacterView = playingCharacterView;
        }

        public void RestAndHeal()
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

