using System;

namespace Business
{
    public class PlayingCharacterHealsUseCase
    {
        private PlayingCharacterViewInterface View;
        
        public PlayingCharacterHealsUseCase(PlayingCharacterViewInterface view)
        {
            View = view;
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
    }
}

