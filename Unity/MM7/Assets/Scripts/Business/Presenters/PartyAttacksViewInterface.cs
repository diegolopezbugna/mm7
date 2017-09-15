using System;
using UnityEngine;

namespace Business
{
    public interface PartyAttacksViewInterface
    {
        void AddMessage(string message);
        void ThrowArrowToTarget(PlayingCharacter attackingChar, Transform targetTransform, Vector3 targetPoint, bool didHit, int damage);
        void ThrowArrowToNonInteractiveObjects(PlayingCharacter attackingChar, Vector3? targetPoint);
        void ThrowSpell(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3? targetPoint, Action<Transform> onCollision);
        void HandToHandAttack(PlayingCharacter attackingChar, Transform targetTransform, bool didHit, int damage);
    }
}

