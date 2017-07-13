using System;

namespace Business
{
    public enum RaceCode
    {
        Human,
        Goblin,
        Dwarf,
        Elf,
    }
    
    public class Race
    {
        private Race(RaceCode code,
            int might, int intellect, int personality, int endurance, int accuracy, int speed) {
            RaceCode = code;
            DefaultMight = might;
            DefaultIntellect = intellect;
            DefaultPersonality = personality;
            DefaultEndurance = endurance;
            DefaultAccuracy = accuracy;
            DefaultSpeed = speed;
        }

        public RaceCode RaceCode { get; private set; }
        public string Name { get { return RaceCode.ToString(); } }

        public int DefaultMight { get; private set; }
        public int DefaultIntellect { get; private set; }
        public int DefaultPersonality { get; private set; }
        public int DefaultEndurance { get; private set; }
        public int DefaultAccuracy { get; private set; }
        public int DefaultSpeed { get; private set; }

        // TODO: resistances

        public Func<int, bool, BonusCost> GetBonusCostForMight { get; private set; }
        public Func<int, bool, BonusCost> GetBonusCostForIntellect { get; private set; }
        public Func<int, bool, BonusCost> GetBonusCostForPersonality { get; private set; }
        public Func<int, bool, BonusCost> GetBonusCostForEndurance { get; private set; }
        public Func<int, bool, BonusCost> GetBonusCostForAccuracy { get; private set; }
        public Func<int, bool, BonusCost> GetBonusCostForSpeed { get; private set; }



        public static Race Human() {
            Race r = new Race(RaceCode.Human, 11, 11, 11, 9, 11, 11);
            r.GetBonusCostForMight = Race.NormalCost;
            r.GetBonusCostForIntellect = Race.NormalCost;
            r.GetBonusCostForPersonality = Race.NormalCost;
            r.GetBonusCostForEndurance = Race.Normal9Cost;
            r.GetBonusCostForAccuracy = Race.NormalCost;
            r.GetBonusCostForSpeed = Race.NormalCost;
            return r;
        }

        public static Race Goblin() {
            Race r = new Race(RaceCode.Goblin, 14, 7, 7, 11, 11, 14);
            r.GetBonusCostForMight = Race.ProficientCost;
            r.GetBonusCostForIntellect = Race.HandicappedCost;
            r.GetBonusCostForPersonality = Race.HandicappedCost;
            r.GetBonusCostForEndurance = Race.NormalCost;
            r.GetBonusCostForAccuracy = Race.NormalCost;
            r.GetBonusCostForSpeed = Race.ProficientCost;
            return r;
        }

        public static Race Dwarf() {
            Race r = new Race(RaceCode.Dwarf, 14, 11, 11, 14, 7, 7);
            r.GetBonusCostForMight = Race.ProficientCost;
            r.GetBonusCostForIntellect = Race.NormalCost;
            r.GetBonusCostForPersonality = Race.NormalCost;
            r.GetBonusCostForEndurance = Race.ProficientCost;
            r.GetBonusCostForAccuracy = Race.HandicappedCost;
            r.GetBonusCostForSpeed = Race.HandicappedCost;
            return r;
        }

        public static Race Elf() {
            Race r = new Race(RaceCode.Elf, 7, 14, 11, 7, 14, 11);
            r.GetBonusCostForMight = Race.HandicappedCost;
            r.GetBonusCostForIntellect = Race.ProficientCost;
            r.GetBonusCostForPersonality = Race.NormalCost;
            r.GetBonusCostForEndurance = Race.HandicappedCost;
            r.GetBonusCostForAccuracy = Race.ProficientCost;
            r.GetBonusCostForSpeed = Race.NormalCost;
            return r;
        }

        public static Race FromCode(RaceCode code) {
            if (code == RaceCode.Human)
                return Race.Human();
            else if (code == RaceCode.Elf)
                return Race.Elf();
            else if (code == RaceCode.Dwarf)
                return Race.Dwarf();
            else if (code == RaceCode.Goblin)
                return Race.Goblin();
            return null;
        }

        // Normal 9 a 25 de a 1
        private static Func<int, bool, BonusCost> NormalCost = (int currentValue, bool isAdd) =>
        {
            BonusCost cost;
            if (currentValue == 25 && isAdd)
                cost = new BonusCost(true);
            else if (currentValue == 9 && !isAdd)
                cost = new BonusCost(true);
            else
                cost = new BonusCost(1, 1);
            return cost;
        };

        // Normal 7 a 20 de a 1
        private static Func<int, bool, BonusCost> Normal9Cost = (int currentValue, bool isAdd) =>
        {
            BonusCost cost;
            if (currentValue == 20 && isAdd)
                cost = new BonusCost(true);
            else if (currentValue == 7 && !isAdd)
                cost = new BonusCost(true);
            else
                cost = new BonusCost(1, 1);
            return cost;
        };
            
        // Attribute raises by 2 for each point spent. Going below initial value adds 2 points to pool. 
        private static Func<int, bool, BonusCost> ProficientCost = (int currentValue, bool isAdd) =>
        {
            BonusCost cost;
            if (currentValue == 30 && isAdd)
                cost = new BonusCost(true);
            else if (currentValue == 12 && !isAdd)
                cost = new BonusCost(true);
            else if (isAdd && currentValue < 14)
                cost = new BonusCost(1, 2);
            else if (!isAdd && currentValue <= 14)
                cost = new BonusCost(1, 2);
            else
                cost = new BonusCost(2, 1);
            return cost;
        };

        // Attribute requires 2 points to raise by one. Going below initial value adds 1/2 point to pool
        private static Func<int, bool, BonusCost> HandicappedCost = (int currentValue, bool isAdd) =>
        {
            BonusCost cost;
            if (currentValue == 15 && isAdd)
                cost = new BonusCost(true);
            else if (currentValue == 5 && !isAdd)
                cost = new BonusCost(true);
            else if (isAdd && currentValue < 7)
                cost = new BonusCost(2, 1);
            else if (!isAdd && currentValue <= 7)
                cost = new BonusCost(2, 1);
            else
                cost = new BonusCost(1, 2);
            return cost;
        };

    }
}

