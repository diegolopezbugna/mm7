using System;

namespace Business
{
    public interface PartyRestsViewInterface
    {
        void WaitTime(float timeInHours, Action onFinished);
        void Show();
        void Hide();
    }
}

