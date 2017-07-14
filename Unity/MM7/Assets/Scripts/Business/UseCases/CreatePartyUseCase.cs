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
            view.SetPortraitSelectedForChar(18, 0);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Knight), 0);
            view.SetPortraitSelectedForChar(4, 1);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Thief), 1);
            view.SetPortraitSelectedForChar(15, 2);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Cleric), 2);
            view.SetPortraitSelectedForChar(11, 3);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Sorcerer), 3);
            view.SetCharSelected(0);
            view.SetSkillForChar(Skill.Get(SkillCode.Bow), 0);
            view.SetSkillForChar(Skill.Get(SkillCode.Armsmaster), 0);
            view.SetSkillForChar(Skill.Get(SkillCode.Leather), 1);
            view.SetSkillForChar(Skill.Get(SkillCode.DisarmTrap), 1);
            view.SetSkillForChar(Skill.Get(SkillCode.Leather), 2);
            view.SetSkillForChar(Skill.Get(SkillCode.Alchemy), 2);
            view.SetSkillForChar(Skill.Get(SkillCode.Leather), 3);
            view.SetSkillForChar(Skill.Get(SkillCode.AirMagic), 3);
            view.SetNameForChar("Zoltan", 0);
            view.SetNameForChar("Roderick", 1);
            view.SetNameForChar("Serena", 2);
            view.SetNameForChar("Alexis", 3);
            view.SetAttributeValuesForChar(new int[] { 30, 5, 5, 13, 13, 20 }, 0);
            view.SetAttributeValuesForChar(new int[] { 13, 9, 9, 13, 13, 13 }, 1);
            view.SetAttributeValuesForChar(new int[] { 12, 9, 20, 20, 7, 11 }, 2);
            view.SetAttributeValuesForChar(new int[] { 5, 30, 9, 13, 13, 13 }, 3);
            bonusPointsUsedByChar = new int[4] { 13, 6, 14, 17 };
            BonusPoints = 0;
            view.BonusPoints = 0;
        }

        public void Clear() {
            view.SetPortraitSelectedForChar(18, 0);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Knight), 0);
            view.SetPortraitSelectedForChar(4, 1);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Thief), 1);
            view.SetPortraitSelectedForChar(15, 2);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Cleric), 2);
            view.SetPortraitSelectedForChar(11, 3);
            view.SetProfessionForChar(Profession.Get(ProfessionCode.Sorcerer), 3);
            view.SetCharSelected(0);
            bonusPointsUsedByChar = new int[4];
            BonusPoints = 50;
            view.BonusPoints = 50;
        }
    }
}

