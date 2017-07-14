using System;

namespace Business
{
    public class CreatePartyUseCase
    {
        private CreatePartyViewInterface view;
        private int[] bonusPointsUsedByChar = new int[4];
        
        public int BonusPoints { get; set; }
        
        public CreatePartyUseCase(CreatePartyViewInterface view)
        {
            this.view = view;
            BonusPoints = 50;
            view.BonusPoints = 50;
        }

        public bool CanUseBonusPoints(int bonusPoints) {
            return BonusPoints >= bonusPoints;
        }

        public void BonusPointsUsed(int bonusPoints, int charIndex) {
            BonusPoints -= bonusPoints;
            view.BonusPoints = BonusPoints;
            bonusPointsUsedByChar[charIndex] += bonusPoints;
        }

        public void GiveBackUsedBonusPoints(int charIndex) {
            BonusPoints += bonusPointsUsedByChar[charIndex];
            view.BonusPoints = BonusPoints;
            bonusPointsUsedByChar[charIndex] = 0;
        }

        public void ClearWithDefaultValues() {
            view.BonusPoints = 50;
            view.SetPortraitSelectedForChar(18, 0);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Knight), 0);
            view.SetPortraitSelectedForChar(4, 1);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Thief), 1);
            view.SetPortraitSelectedForChar(15, 2);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Cleric), 2);
            view.SetPortraitSelectedForChar(11, 3);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Sorcerer), 3);
            view.SetCharSelected(0);
        }

    }
}

