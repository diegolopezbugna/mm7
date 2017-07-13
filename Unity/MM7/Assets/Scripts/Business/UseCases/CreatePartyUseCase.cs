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

    }
}

