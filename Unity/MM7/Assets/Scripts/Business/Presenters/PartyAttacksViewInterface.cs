using System;
using UnityEngine;

namespace Business
{
    public interface PartyAttacksViewInterface
    {
        void AddMessage(string message);
        void ThrowArrowToTarget(PlayingCharacter attackingChar, Transform targetTransform, Vector3 targetPoint, bool didHit, int damage);
        void ThrowArrowToNonInteractiveObjects(PlayingCharacter attackingChar, Vector3? targetPoint);
        void HandToHandAttack(PlayingCharacter attackingChar, Transform targetTransform, bool didHit, int damage);
    }
}

