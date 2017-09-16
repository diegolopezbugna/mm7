using System;
using UnityEngine;

namespace Business
{
    public interface PartyCastsSpellViewInterface
    {
        void AddMessage(string message);
        void ThrowSpell(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3? targetPoint, Action<Transform> onCollision);
        void ShowPortraitSpellAnimation(PlayingCharacter target, SpellInfo spellInfo);
        void UpdatePlayingCharacter(PlayingCharacter target);
    }
}

