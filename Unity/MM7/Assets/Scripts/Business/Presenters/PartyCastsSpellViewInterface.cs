using System;
using UnityEngine;

namespace Business
{
    public interface PartyCastsSpellViewInterface
    {
        void AddMessage(string message);
        void ShowSpellFx(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3? targetPoint, Action<Transform> onCollision);
        void ThrowSpellFx(PlayingCharacter attackingChar, SpellInfo spellInfo, Vector3 targetPoint, Action<Transform> onCollision);
        void ShowPortraitSpellAnimation(PlayingCharacter target, SpellInfo spellInfo);
        void UpdatePlayingCharacter(PlayingCharacter target);
    }
}

