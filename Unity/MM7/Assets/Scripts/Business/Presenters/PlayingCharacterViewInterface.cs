using System;

namespace Business
{
    public interface PlayingCharacterViewInterface
    {
        void UpdatePlayingCharacter(PlayingCharacter target);
        void SelectNextPlayingCharacter();
        void ShowGameOver();
        void ShowMessage(string message);
        void RefreshGoldAndFood();
        void PlayGoldSound();
    }
}

