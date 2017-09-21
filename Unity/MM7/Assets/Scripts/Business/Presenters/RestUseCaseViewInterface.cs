using System;

namespace Business
{
    public interface RestUseCaseViewInterface
    {
        void WaitTime(float timeInHours, Action onFinished);
        void Hide();
    }
}

