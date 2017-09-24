using System;

namespace Business
{
    public interface PlayingCharacterViewInterface
    {
        void UpdatePlayingCharacter(PlayingCharacter target);
        void SelectNextPlayingCharacter();
        void ShowGameOver();
    }
}

