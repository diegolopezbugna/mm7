using System;

namespace Business
{
    public class BonusCost
    {
        public int AttributeChange
        {
            get;
            set;
        }

        public int BonusChange
        {
            get;
            set;
        }

        public bool Offlimits
        {
            get;
            set;
        }

        public BonusCost(bool isOfflimits) {
            Offlimits = isOfflimits;
        }

        public BonusCost(int attributeChange, int bonusChange) {
            AttributeChange = attributeChange;
            BonusChange = bonusChange;
        }
    }
}

