using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;

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
        private Race(RaceCode code) {
            RaceCode = code;
            Name = Localization.Instance.Get(code.ToString());
        }

        public RaceCode RaceCode { get; private set; }
        public string Name { get; private set; }
        public Dictionary<AttributeCode, int> DefaultAttributeValues { get; private set; }

        // TODO: resistances

        /// <summary>
        /// Gets the bonus points cost for adding/substracting attributes values on party creation.
        /// </summary>
        /// <remarks>>
        /// AttributeCode: attribute
        /// int: current attribute value
        /// bool: if it's an add operation (false for substracting values)
        /// </remarks>
        public Func<AttributeCode, int, bool, BonusCost> GetBonusCost { get; private set; }

        public static Race Human() {
            Race r = new Race(RaceCode.Human);
            r.DefaultAttributeValues = new Dictionary<AttributeCode, int>() { 
                { AttributeCode.Might, 11 },
                { AttributeCode.Intellect, 11 },
                { AttributeCode.Personality, 11 },
                { AttributeCode.Endurance, 9 },
                { AttributeCode.Accuracy, 11 },
                { AttributeCode.Speed, 11 },
            };
            r.GetBonusCost = (AttributeCode attributeCode, int  currentValue, bool isAdd) =>
            {
                if (attributeCode == AttributeCode.Endurance)
                    return Race.Normal9Cost(currentValue, isAdd);
                else
                    return Race.NormalCost(currentValue, isAdd);
            };
            return r;
        }

        public static Race Goblin() {
            Race r = new Race(RaceCode.Goblin);
            r.DefaultAttributeValues = new Dictionary<AttributeCode, int>() { 
                { AttributeCode.Might, 14 },
                { AttributeCode.Intellect, 7 },
                { AttributeCode.Personality, 7 },
                { AttributeCode.Endurance, 11 },
                { AttributeCode.Accuracy, 11 },
                { AttributeCode.Speed, 14 },
            };
            r.GetBonusCost = (AttributeCode attributeCode, int  currentValue, bool isAdd) =>
            {
                if (attributeCode == AttributeCode.Might || attributeCode == AttributeCode.Speed)
                    return Race.ProficientCost(currentValue, isAdd);
                else if (attributeCode == AttributeCode.Intellect || attributeCode == AttributeCode.Personality)
                    return Race.HandicappedCost(currentValue, isAdd);
                else
                    return Race.NormalCost(currentValue, isAdd);
            };
            return r;
        }

        public static Race Dwarf() {
            Race r = new Race(RaceCode.Dwarf);
            r.DefaultAttributeValues = new Dictionary<AttributeCode, int>() { 
                { AttributeCode.Might, 14 },
                { AttributeCode.Intellect, 11 },
                { AttributeCode.Personality, 11 },
                { AttributeCode.Endurance, 14 },
                { AttributeCode.Accuracy, 7 },
                { AttributeCode.Speed, 7 },
            };
            r.GetBonusCost = (AttributeCode attributeCode, int  currentValue, bool isAdd) =>
            {
                if (attributeCode == AttributeCode.Might || attributeCode == AttributeCode.Endurance)
                    return Race.ProficientCost(currentValue, isAdd);
                else if (attributeCode == AttributeCode.Accuracy || attributeCode == AttributeCode.Speed)
                    return Race.HandicappedCost(currentValue, isAdd);
                else
                    return Race.NormalCost(currentValue, isAdd);
            };
            return r;
        }

        public static Race Elf() {
            Race r = new Race(RaceCode.Elf);
            r.DefaultAttributeValues = new Dictionary<AttributeCode, int>() { 
                { AttributeCode.Might, 7 },
                { AttributeCode.Intellect, 14 },
                { AttributeCode.Personality, 11 },
                { AttributeCode.Endurance, 7 },
                { AttributeCode.Accuracy, 14 },
                { AttributeCode.Speed, 11 },
            };
            r.GetBonusCost = (AttributeCode attributeCode, int  currentValue, bool isAdd) =>
            {
                if (attributeCode == AttributeCode.Intellect || attributeCode == AttributeCode.Accuracy)
                    return Race.ProficientCost(currentValue, isAdd);
                else if (attributeCode == AttributeCode.Might || attributeCode == AttributeCode.Endurance)
                    return Race.HandicappedCost(currentValue, isAdd);
                else
                    return Race.NormalCost(currentValue, isAdd);
            };
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
        private static BonusCost NormalCost(int currentValue, bool isAdd) {
            BonusCost cost;
            if (currentValue == 25 && isAdd)
                cost = new BonusCost(true);
            else if (currentValue == 9 && !isAdd)
                cost = new BonusCost(true);
            else
                cost = new BonusCost(1, 1);
            return cost;
        }

        // Normal 7 a 20 de a 1
        private static BonusCost Normal9Cost(int currentValue, bool isAdd) {
            BonusCost cost;
            if (currentValue == 20 && isAdd)
                cost = new BonusCost(true);
            else if (currentValue == 7 && !isAdd)
                cost = new BonusCost(true);
            else
                cost = new BonusCost(1, 1);
            return cost;
        }
            
        // Attribute raises by 2 for each point spent. Going below initial value adds 2 points to pool. 
        private static BonusCost ProficientCost(int currentValue, bool isAdd) {
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
        }

        // Attribute requires 2 points to raise by one. Going below initial value adds 1/2 point to pool
        private static BonusCost HandicappedCost(int currentValue, bool isAdd) {
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
        }

    }
}

