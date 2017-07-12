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
        public Race(RaceCode code,
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

        public Func<int, bool, float> GetBonusCostForMight { get; private set; }
        public Func<int, bool, float> GetBonusCostForIntellect { get; private set; }
        public Func<int, bool, float> GetBonusCostForPersonality { get; private set; }
        public Func<int, bool, float> GetBonusCostForEndurance { get; private set; }
        public Func<int, bool, float> GetBonusCostForAccuracy { get; private set; }
        public Func<int, bool, float> GetBonusCostForSpeed { get; private set; }



        public static Race HumanRace() {
            Race r = new Race(RaceCode.Human, 11, 11, 11, 9, 11, 11);
            r.GetBonusCostForMight = Race.NormalCost;
            r.GetBonusCostForIntellect = Race.NormalCost;
            r.GetBonusCostForPersonality = Race.NormalCost;
            r.GetBonusCostForEndurance = Race.Normal9Cost;
            r.GetBonusCostForAccuracy = Race.NormalCost;
            r.GetBonusCostForSpeed = Race.NormalCost;
            return r;
        }

        public static Race GoblinRace() {
            Race r = new Race(RaceCode.Goblin, 14, 7, 7, 11, 11, 14);
            r.GetBonusCostForMight = Race.ProficientCost;
            r.GetBonusCostForIntellect = Race.HandicappedCost;
            r.GetBonusCostForPersonality = Race.HandicappedCost;
            r.GetBonusCostForEndurance = Race.NormalCost;
            r.GetBonusCostForAccuracy = Race.NormalCost;
            r.GetBonusCostForSpeed = Race.ProficientCost;
            return r;
        }

        public static Race DwarfRace() {
            Race r = new Race(RaceCode.Dwarf, 14, 11, 11, 14, 7, 7);
            r.GetBonusCostForMight = Race.ProficientCost;
            r.GetBonusCostForIntellect = Race.NormalCost;
            r.GetBonusCostForPersonality = Race.NormalCost;
            r.GetBonusCostForEndurance = Race.ProficientCost;
            r.GetBonusCostForAccuracy = Race.HandicappedCost;
            r.GetBonusCostForSpeed = Race.HandicappedCost;
            return r;
        }

        public static Race ElfRace() {
            Race r = new Race(RaceCode.Elf, 7, 14, 11, 7, 14, 11);
            r.GetBonusCostForMight = Race.HandicappedCost;
            r.GetBonusCostForIntellect = Race.ProficientCost;
            r.GetBonusCostForPersonality = Race.NormalCost;
            r.GetBonusCostForEndurance = Race.HandicappedCost;
            r.GetBonusCostForAccuracy = Race.ProficientCost;
            r.GetBonusCostForSpeed = Race.NormalCost;
            return r;
        }

        // Normal 9 a 25 de a 1
        private static Func<int, bool, float> NormalCost = (int currentValue, bool isAdd) =>
        {
            if (currentValue == 25 && isAdd)
                return int.MaxValue;
            else if (currentValue == 9 && !isAdd)
                return int.MaxValue;
            else
                return isAdd ? 1.0f : -1.0f;
        };

        // Normal 7 a 20 de a 1
        private static Func<int, bool, float> Normal9Cost = (int currentValue, bool isAdd) =>
        {
            if (currentValue == 20 && isAdd)
                return int.MaxValue;
            else if (currentValue == 7 && !isAdd)
                return int.MaxValue;
            else
                return isAdd ? 1.0f : -1.0f;
        };
            
        // Attribute raises by 2 for each point spent. Going below initial value adds 2 points to pool. 
        private static Func<int, bool, float> ProficientCost = (int currentValue, bool isAdd) =>
        {
            if (currentValue == 30 && isAdd)
                return int.MaxValue;
            else if (currentValue == 12 && !isAdd)
                return int.MaxValue;
            else if (isAdd && currentValue < 14)
                return 2.0f;
            else if (!isAdd && currentValue <= 14)
                return -2.0f;
            else
                return isAdd ? 0.5f : -0.5f;
        };

        // Attribute requires 2 points to raise by one. Going below initial value adds 1/2 point to pool
        private static Func<int, bool, float> HandicappedCost = (int currentValue, bool isAdd) =>
            {
                if (currentValue == 15 && isAdd)
                    return int.MaxValue;
                else if (currentValue == 5 && !isAdd)
                    return int.MaxValue;
                else if (isAdd && currentValue < 7)
                    return 0.5f;
                else if (!isAdd && currentValue <= 7)
                    return -0.5f;
                else
                    return isAdd ? 2.0f : -2.0f;
            };


    }
}

