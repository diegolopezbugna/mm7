using System;

namespace Business
{
    public interface CreatePartyViewInterface
    {
        int BonusPoints { get; set; }
        void SetPortraitSelectedForChar(int portrait, int charIndex);
        void SetProfessionForChar(Profession profession, int charIndex);
        void SetSkillForChar(Skill skill, int charIndex);
        void SetNameForChar(string name, int charIndex);
        void SetAttributeValuesForChar(int[] values, int charIndex);

    }
}

