using System;

namespace Business
{
    public class CreatePartyUseCase
    {
        private CreatePartyViewInterface view;
        
        public int BonusPoints { get; set; }
        
        public CreatePartyUseCase(CreatePartyViewInterface view)
        {
            this.view = view;
            BonusPoints = 50;
            view.BonusPoints = 50;
        }

        public bool CanUseBonusPoints(int bonusPoints) {
            return BonusPoints > bonusPoints;
        }

        public void BonusPointsUsed(int bonusPoints) {
            BonusPoints -= bonusPoints;
            view.BonusPoints = BonusPoints;
        }

        public void ClearWithDefaultValues() {
            view.BonusPoints = 50;
            view.SetPortraitSelectedForChar(18, 0);
            view.SetPortraitSelectedForChar(4, 1);
            view.SetPortraitSelectedForChar(15, 2);
            view.SetPortraitSelectedForChar(11, 3);
        }

    }
}

