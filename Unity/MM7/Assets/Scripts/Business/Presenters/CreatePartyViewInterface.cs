﻿using System;

namespace Business
{
    public interface CreatePartyViewInterface
    {
        int BonusPoints { get; set; }
        void SetPortraitSelectedForChar(int portrait, int charIndex);
        void SetCharSelected(int charIndex);
        void SetProfessionForChar(Profession profession, int charIndex);
    }
}

