using System;
using UnityEngine;

namespace Business
{
    public class PartyCastsSpellUseCase
    {
        private PartyCastsSpellViewInterface View { get; set; }
        private Transform PartyTransform { get; set; }

        public PartyCastsSpellUseCase(PartyCastsSpellViewInterface view, Transform partyTransform)
        {
            View = view;
            PartyTransform = partyTransform;
        }

        public void CastSpell(PlayingCharacter speller, SpellInfo spellInfo)
        {
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
                    View.UpdatePlayingCharacter(target);
                }
            }
        }

        public void CastSpell(PlayingCharacter speller, SpellInfo spellInfo, PlayingCharacter target)
        {
            if (spellInfo.Code == (int)SpellCodes.Body_Heal)
            {
                if (!speller.Skills.ContainsKey(SkillCode.BodyMagic))
                {
                    SpellFailed(speller, spellInfo);
                    return;
                }

                var multiplicator = (int)speller.Skills[SkillCode.BodyMagic].SkillLevel + 2;
                var hitPointsHealing = 5 + multiplicator * speller.GetTotalSkillBonus(SkillCode.BodyMagic);
                target.Heal(hitPointsHealing);

                View.ShowPortraitSpellAnimation(target, spellInfo);
                View.UpdatePlayingCharacter(target);
            }
        }

        public void CastSpell(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3 targetPoint, Transform transform)
        {
            Action<Transform> onCollided = (Transform collided) => {
                if (collided.tag.StartsWith("Enemy")) { // TODO: other collisions, area effect (fireball)
                    var enemyHealth = collided.GetComponent<EnemyHealth>();
                    if (enemyHealth != null && enemyHealth.IsActive()) {
                        var totalSkillBonus = attackingChar.GetTotalSkillBonus(spellInfo.SkillCode);
                        var minDamage = spellInfo.BaseDamage + totalSkillBonus; // * 1
                        var maxDamage = spellInfo.BaseDamage + totalSkillBonus * spellInfo.SkillPointDamageBonus;
                        var damage = UnityEngine.Random.Range(minDamage, maxDamage + 1); // TODO: monster resistances!
                        enemyHealth.TakeHit(damage);
                        View.AddMessage(string.Format("{0}'s {1} hits {2} for {3} points", attackingChar.Name, spellInfo.Name, collided.tag.TagToDescription(), damage));
                        var enemyAttackBehaviour = collided.GetComponent<EnemyAttack>();
                        enemyAttackBehaviour.AlertOthers();
                    }
                }
            };
            
            if (spellInfo.HasNoTrail)
            {
                View.ShowSpellFx(attackingChar, spellInfo, targetPoint, null);
                // TODO: spell have different distance OK limits... check distance, fail if distance > allowed distance for that spell
                if (transform != null)
                    onCollided(transform);
            }
            else
            {
                View.ThrowSpellFx(attackingChar, spellInfo, targetPoint, onCollided);
            }
            
        }

        private void SpellFailed(PlayingCharacter speller, SpellInfo spellInfo)
        {
            // TODO: spell failed, show sad portrait
        }

    }
}

