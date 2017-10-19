using System;
using System.Collections;
using System.Collections.Generic;

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
            var party = CreatePartyUseCase.CreateDummyParty();
            for (int i = 0; i < party.Chars.Count; i++)
            {
                view.SetPortraitSelectedForChar(int.Parse(party.Chars[i].PortraitCode), i);
                view.SetProfessionForChar(party.Chars[i].Profession, i);
                view.SetNameForChar(party.Chars[i].Name, i);
                view.SetAttributeValuesForChar(new int[] { 
                    party.Chars[i].Might, 
                    party.Chars[i].Intellect, 
                    party.Chars[i].Personality, 
                    party.Chars[i].Endurance, 
                    party.Chars[i].Accuracy, 
                    party.Chars[i].Speed }, i);
            }

            // TODO: change
            view.SetSkillForChar(Skill.Get(SkillCode.Bow), 0);
            view.SetSkillForChar(Skill.Get(SkillCode.Armsmaster), 0);
            view.SetSkillForChar(Skill.Get(SkillCode.Leather), 1);
            view.SetSkillForChar(Skill.Get(SkillCode.DisarmTrap), 1);
            view.SetSkillForChar(Skill.Get(SkillCode.Leather), 2);
            view.SetSkillForChar(Skill.Get(SkillCode.Alchemy), 2);
            view.SetSkillForChar(Skill.Get(SkillCode.Leather), 3);
            view.SetSkillForChar(Skill.Get(SkillCode.AirMagic), 3);
                
            bonusPointsUsedByChar = new int[4] { 13, 6, 14, 17 };
            BonusPoints = 0;
            view.BonusPoints = 0;
        }

        public static PartyStats CreateDummyParty() {
            var dummyParty = new PartyStats();
            dummyParty.Chars = new List<PlayingCharacter>();
            var pc = CreateDummyChar("Zoltan", Race.Goblin(), Gender.Male, "18", Profession.Get(ProfessionCode.Knight), new int[] { 30, 5, 5, 13, 13, 20 },
                new SkillCode[] { SkillCode.Sword, SkillCode.Leather, SkillCode.Bow, SkillCode.Armsmaster });
            pc.Inventory.TryInsertItem(Item.GetByCode(1));
            pc.Inventory.TryInsertItem(Item.GetByCode(47));
            pc.Inventory.TryInsertItem(Item.GetByCode(66));
            pc.Inventory.TryInsertItem(Item.GetByCode(120));
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(91), 0, 7);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(89), 6, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(90), 6, 2);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(91), 6, 4);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(92), 6, 6);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(93), 8, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(94), 8, 2);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(95), 8, 4);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(96), 8, 6);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(97), 10, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(98), 10, 2);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(99), 10, 4);
            dummyParty.Chars.Add(pc);
            pc = CreateDummyChar("Roderick", Race.Human(), Gender.Male, "04", Profession.Get(ProfessionCode.Thief), new int[] { 13, 9, 9, 13, 13, 13 },
            //pc = CreateDummyChar("Roderick", Race.Dwarf(), Gender.Male, "13", Profession.Get(ProfessionCode.Thief), new int[] { 13, 9, 9, 13, 13, 13 },
                new SkillCode[] { SkillCode.Dagger, SkillCode.Stealing, SkillCode.Leather, SkillCode.DisarmTrap });
            pc.Inventory.TryInsertItem(Item.GetByCode(15));
            pc.Inventory.TryInsertItem(Item.GetByCode(66));
            pc.Inventory.TryInsertItem(Item.GetByCode(120));
            pc.Inventory.TryInsertItem(Item.GetByCode(121));
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(1), 1, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(2), 2, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(3), 3, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(4), 4, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(24), 5, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(54), 5, 5);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(29), 7, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(37), 9, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(100), 12, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(101), 12, 1);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(102), 12, 2);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(103), 12, 3);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(104), 12, 4);
            dummyParty.Chars.Add(pc);
            pc = CreateDummyChar("Serena", Race.Dwarf(), Gender.Female, "15", Profession.Get(ProfessionCode.Cleric), new int[] { 12, 9, 20, 20, 7, 11 },
                new SkillCode[] { SkillCode.Mace, SkillCode.BodyMagic, SkillCode.Leather, SkillCode.Alchemy });
            pc.Inventory.TryInsertItem(Item.GetByCode(50));
            pc.Inventory.TryInsertItem(Item.GetByCode(66));
            pc.Inventory.TryInsertItem(Item.GetByCode(467));
            pc.Inventory.TryInsertItem(Item.GetByCode(220));
            pc.Inventory.TryInsertItem(Item.GetByCode(200));
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(71), 1, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(76), 6, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(88), 6, 4);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(94), 10, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(97), 10, 3);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(300), 12, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(301), 12, 2);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(400), 12, 4);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(401), 0, 5);
            dummyParty.Chars.Add(pc);
            pc = CreateDummyChar("Alexis", Race.Elf(), Gender.Female, "11", Profession.Get(ProfessionCode.Sorcerer), new int[] { 5, 30, 9, 13, 13, 13 },
                new SkillCode[] { SkillCode.Staff, SkillCode.FireMagic, SkillCode.Leather, SkillCode.AirMagic });
            pc.Inventory.TryInsertItem(Item.GetByCode(61));
            pc.Inventory.TryInsertItem(Item.GetByCode(66));
            pc.Inventory.TryInsertItem(Item.GetByCode(401));
            pc.Inventory.TryInsertItem(Item.GetByCode(412));
            pc.Inventory.TryInsertItem(Item.GetByCode(121));
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(119), 2, 5);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(73), 1, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(74), 6, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(110), 12, 6);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(111), 12, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(112), 12, 2);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(122), 12, 4);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(129), 5, 8);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(123), 6, 8);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(124), 7, 8);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(125), 8, 8);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(126), 9, 8);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(127), 10, 8);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(128), 11, 8);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(130), 13, 0);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(131), 13, 2);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(132), 13, 3);
//            pc.Inventory.TryInsertItemAt(Item.GetByCode(135), 10, 0);
            dummyParty.Chars.Add(pc);
            return dummyParty;
        }

        private static PlayingCharacter CreateDummyChar(string name, Race race, Gender gender, string portraitCode, Profession profession, int[] attributes, SkillCode[] skills) {
            var pc = new PlayingCharacter(name, race, gender, portraitCode, skills);
            pc.Profession = profession;
            pc.Might = attributes[0];
            pc.Intellect = attributes[1];
            pc.Personality = attributes[2];
            pc.Endurance = attributes[3];
            pc.Accuracy = attributes[4];
            pc.Speed = attributes[5];
            pc.HitPoints = pc.MaxHitPoints;
            pc.SpellPoints = pc.MaxSpellPoints;
            return pc;
        }

        public void Clear() {
            var party = CreatePartyUseCase.CreateDummyParty();
            for (int i = 0; i < party.Chars.Count; i++)
            {
                view.SetPortraitSelectedForChar(int.Parse(party.Chars[i].PortraitCode), i);
                view.SetProfessionForChar(party.Chars[i].Profession, i);
            }

            bonusPointsUsedByChar = new int[4];
            BonusPoints = 50;
            view.BonusPoints = 50;
        }

        public void AddStartingInventoryItems(PlayingCharacter playingCharacter)
        {
            foreach (var skillCode in playingCharacter.Skills.Keys)
            {
                foreach (var i in GetStartingItemsForSkill(skillCode))
                    playingCharacter.Inventory.TryInsertItem(i);
            }
        }

        private List<Item> GetStartingItemsForSkill(SkillCode skillCode)
        {
            var items = new List<Item>();
            switch (skillCode)
            {
                case SkillCode.Sword:
                    items.Add(Item.GetByCode(1));
                    break;
                case SkillCode.Dagger:
                    items.Add(Item.GetByCode(15));
                    break;
                case SkillCode.Axe:
                    items.Add(Item.GetByCode(23));
                    break;
                case SkillCode.Spear:
                    items.Add(Item.GetByCode(31));
                    break;
                case SkillCode.Bow:
                    items.Add(Item.GetByCode(47));
                    break;
                case SkillCode.Mace:
                    items.Add(Item.GetByCode(50));
                    break;
                case SkillCode.Staff:
                    items.Add(Item.GetByCode(61));
                    break;
                case SkillCode.Leather:
                    items.Add(Item.GetByCode(66));
                    break;
                case SkillCode.Chain:
                    items.Add(Item.GetByCode(71));
                    break;
                case SkillCode.Shield:
                    items.Add(Item.GetByCode(84));
                    break;
                case SkillCode.FireMagic:
                    items.Add(Item.GetByCode(401));
                    break;
                case SkillCode.AirMagic:
                    items.Add(Item.GetByCode(412));
                    break;
                case SkillCode.WaterMagic:
                    items.Add(Item.GetByCode(423));
                    break;
                case SkillCode.EarthMagic:
                    items.Add(Item.GetByCode(434));
                    break;
                case SkillCode.SpiritMagic:
                    items.Add(Item.GetByCode(445));
                    break;
                case SkillCode.MindMagic:
                    items.Add(Item.GetByCode(456));
                    break;
                case SkillCode.BodyMagic:
                    items.Add(Item.GetByCode(467));
                    break;
                case SkillCode.Alchemy:
                    items.Add(Item.GetByCode(200));
                    items.Add(Item.GetByCode(220));
                    break;
                default:
                    items.Add(Item.GetByCode(UnityEngine.Random.Range(120, 122)));
                    break;
            }
            return items;
        }
    }
}

