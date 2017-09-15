using UnityEngine;

namespace Business
{
    public class PartyCastsSpellUseCase
    {
        private PartyAttacksViewInterface View { get; set; }
        private Transform PartyTransform { get; set; }

        public PartyCastsSpellUseCase(PartyAttacksViewInterface view, Transform partyTransform)
        {
            View = view;
            PartyTransform = partyTransform;
        }

        public void ThrowSpell(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3? targetPoint)
        {
            View.ThrowSpell(attackingChar, spellInfo, targetPoint, (Transform collided) => {
                if (collided.tag.StartsWith("Enemy")) { // TODO: other collisions
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
            });
        }

        public void CastSpell(PlayingCharacter speller, SpellInfo spellInfo)
        {
            // TODO: spells with no target
        }
    }
}

