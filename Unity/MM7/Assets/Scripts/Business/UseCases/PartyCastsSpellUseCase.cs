using System;
using UnityEngine;

namespace Business
{
    public class PartyCastsSpellUseCase
    {
        private PartyCastsSpellViewInterface View;
        private PlayingCharacterViewInterface PlayingCharacterView;
        private Transform PartyTransform;

        public PartyCastsSpellUseCase(PartyCastsSpellViewInterface view, PlayingCharacterViewInterface playingCharacterView, Transform partyTransform)
        {
            View = view;
            PlayingCharacterView = playingCharacterView;
            PartyTransform = partyTransform;
        }

        public void CastSpell(PlayingCharacter speller, SpellInfo spellInfo)
        {
            SetRecoveryTime(speller, spellInfo);

            if (!string.IsNullOrEmpty(spellInfo.SpellFxName)) 
            {
                View.ShowSpellFx(speller, spellInfo, null, null); // TODO: onCollision, damage
            }
            else 
            {
                // TODO: each spell
                foreach (var target in Game.Instance.PartyStats.Chars)
                {
                    View.ShowPortraitSpellAnimation(target, spellInfo);
                    PlayingCharacterView.UpdatePlayingCharacter(target);
                }
            }
        }

        public void CastSpell(PlayingCharacter speller, SpellInfo spellInfo, PlayingCharacter target)
        {
            SetRecoveryTime(speller, spellInfo);

            if (spellInfo.Code == (int)SpellCodes.Body_Heal)
            {
                if (!speller.Skills.ContainsKey(SkillCode.BodyMagic))
                {
                    SpellFailed(speller, spellInfo);
                    return;
                }

                var multiplicator = (int)speller.Skills[SkillCode.BodyMagic].SkillLevel + 2;
                var hitPointsHealing = 5 + multiplicator * speller.GetTotalSkillBonus(SkillCode.BodyMagic);
                View.ShowPortraitSpellAnimation(target, spellInfo);

                var playingCharacterHealsUseCase = new PlayingCharacterHealsUseCase(PlayingCharacterView);
                playingCharacterHealsUseCase.Heal(target, hitPointsHealing);
            }
        }

        public void CastSpell(PlayingCharacter speller, SpellInfo spellInfo, Vector3 targetPoint, Transform transform)
        {
            SetRecoveryTime(speller, spellInfo);

            Action<Transform> onCollided = (Transform collided) => {
                if (collided.tag.StartsWith("Enemy")) { // TODO: other collisions, area effect (fireball)
                    var enemyHealth = collided.GetComponent<EnemyHealth>();
                    if (enemyHealth != null && enemyHealth.IsActive()) {
                        var totalSkillBonus = speller.GetTotalSkillBonus(spellInfo.SkillCode);
                        var minDamage = spellInfo.BaseDamage + totalSkillBonus; // * 1
                        var maxDamage = spellInfo.BaseDamage + totalSkillBonus * spellInfo.SkillPointDamageBonus;
                        var damage = UnityEngine.Random.Range(minDamage, maxDamage + 1); // TODO: monster resistances!
                        enemyHealth.TakeHit(damage);
                        View.AddMessage(string.Format("{0}'s {1} hits {2} for {3} points", speller.Name, spellInfo.Name, collided.tag.TagToDescription(), damage));
                        var enemyAttackBehaviour = collided.GetComponent<EnemyAttack>();
                        enemyAttackBehaviour.AlertOthers();
                    }
                }
            };
            
            if (spellInfo.HasNoTrail)
            {
                View.ShowSpellFx(speller, spellInfo, targetPoint, null);
                // TODO: spell have different distance OK limits... check distance, fail if distance > allowed distance for that spell
                if (transform != null)
                    onCollided(transform);
            }
            else
            {
                View.ThrowSpellFx(speller, spellInfo, targetPoint, onCollided);
            }
            
        }

        private void SpellFailed(PlayingCharacter speller, SpellInfo spellInfo)
        {
            // TODO: spell failed, show sad portrait
        }

        private void SetRecoveryTime(PlayingCharacter speller, SpellInfo spellInfo)
        {
            speller.LastAttackTimeFrom = Time.time;
            // TODO: anything else that affects recovery time?
            speller.LastAttackTimeTo = speller.LastAttackTimeFrom + 2f + spellInfo.RecoveryTimes[speller.Skills[spellInfo.SkillCode].SkillLevel] / 100f;
            PlayingCharacterView.SelectNextPlayingCharacter();
        }


    }
}

