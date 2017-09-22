using UnityEngine;
using System.Linq;

namespace Business
{
    public class EnemyAttacksUseCase
    {
        private EnemyAttacksViewInterface View;
        private PlayingCharacterViewInterface PlayingCharacterView;

        public EnemyAttacksUseCase(EnemyAttacksViewInterface view, PlayingCharacterViewInterface playingCharacterView)
        {
            View = view;
            PlayingCharacterView = playingCharacterView;
        }

        public void EnemyAttacks(Enemy enemy, PlayingCharacter targetCharacter) 
        {
            var ac = targetCharacter.ArmorClass;
            var chanceToHit = (5f + enemy.MonsterLevel * 2f) / (10f + enemy.MonsterLevel * 2f + ac);

            if (Random.Range(0f, 1f) > chanceToHit)
            {
                var damage = Random.Range(enemy.DamageMin, enemy.DamageMax + 1);  // TODO: review damage formula
                targetCharacter.HitPoints -= damage;
                MessagesScroller.Instance.AddMessage(string.Format("{0} hits {1} for {2} points", enemy.Name, targetCharacter.Name, damage));

                View.TakeHit(targetCharacter);

                if (targetCharacter.HitPoints <= -targetCharacter.Endurance) // TODO: PRESERVATION SPELL, endurance bonuses from items
                {
                    targetCharacter.ConditionStatus = ConditionStatus.Dead;
                    MessagesScroller.Instance.AddMessage(string.Format("{0} dies.", targetCharacter.Name));
                }
                else if (targetCharacter.HitPoints <= 0)
                {
                    targetCharacter.ConditionStatus = ConditionStatus.Unconscious;
                    MessagesScroller.Instance.AddMessage(string.Format("{0} gets unconscious.", targetCharacter.Name));
                }

                PlayingCharacterView.UpdatePlayingCharacter(targetCharacter);

                if (Game.Instance.PartyStats.Chars.All(c => c.HitPoints <= 0)) {
                    PlayingCharacterView.ShowGameOver();
                }
            }
            else
            {
                MessagesScroller.Instance.AddMessage(string.Format("{0} misses {1}", enemy.Name, targetCharacter.Name));
            }
        }
    }
}

