using UnityEngine;

namespace Business
{
    public class PartyAttacksUseCase
    {
        private const float handToHandRangeSqrDistance = 4 * 4; // TODO: check distances
        private const float mediumRangeSqrDistance = 20 * 20; // TODO: check distances
        private const float longRangeSqrDistance = 40 * 40; // TODO: check distances

        private PartyAttacksViewInterface View { get; set; }
        private Vector3? TargetPoint { get; set; }
        private Transform TargetTransform { get; set; }
        private Transform PartyTransform { get; set; }

        public PartyAttacksUseCase(PartyAttacksViewInterface view, Vector3? targetPoint, Transform targetTransform, Transform partyTransform)
        {
            View = view;
            TargetPoint = targetPoint;
            TargetTransform = targetTransform;
            PartyTransform = partyTransform;
        }

        // TODO: instead of monsterArmorClass have a monster instance?
        public void TryHit(PlayingCharacter attackingChar, float monsterArmorClass) {
            var distanceToTargetSqr = (TargetTransform.position - PartyTransform.position).sqrMagnitude;
            bool handToHand = distanceToTargetSqr < handToHandRangeSqrDistance;
            var attackBonus = handToHand ? attackingChar.AttackBonus : attackingChar.RangedAttackBonus;
            var toHitAttackNumber = attackBonus * 2f + monsterArmorClass + 30f;
            var toHitDefenseNumber = (monsterArmorClass + 15f) * GetAttackDistanceMultiplier(distanceToTargetSqr); 
            bool didHit = Random.Range(1f, toHitAttackNumber) > Random.Range(1f, toHitDefenseNumber);

            if (handToHand)
            {
                // TODO: physical resistance
                var damage = Random.Range(attackingChar.DamageMin, attackingChar.DamageMax + 1);
                View.HandToHandAttack(attackingChar, didHit, damage);
            }
            else
            {
                if (didHit)
                {
                    // TODO: physical resistance
                    var damage = Random.Range(attackingChar.RangedDamageMin, attackingChar.RangedDamageMax + 1);
                    View.ThrowArrowToTarget(attackingChar, TargetTransform, TargetPoint.Value, didHit, damage);
                }
                else
                {
                    View.ThrowArrowToNonInteractiveObjects(attackingChar, TargetPoint);
                }
            }
        }

        public void HitNothing(PlayingCharacter attackingChar) {
            if (TargetPoint.HasValue)
            {
                var distanceToTargetSqr = (TargetPoint.Value - PartyTransform.position).sqrMagnitude;
                if (distanceToTargetSqr < handToHandRangeSqrDistance)
                    View.HandToHandAttack(attackingChar, null, false, 0);
                else
                    View.ThrowArrowToNonInteractiveObjects(attackingChar, TargetPoint);
            }
            else
                View.ThrowArrowToNonInteractiveObjects(attackingChar, TargetPoint);
        }

        private float GetAttackDistanceMultiplier(float distanceToTargetSqr) {
            if (distanceToTargetSqr > longRangeSqrDistance)
                return 2f;
            else if (distanceToTargetSqr > mediumRangeSqrDistance)
                return 1.5f;
            else
                return 1f;
        }

    }
}

