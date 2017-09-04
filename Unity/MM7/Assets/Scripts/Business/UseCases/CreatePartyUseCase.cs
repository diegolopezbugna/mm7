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
            var pc = CreateDummyChar("Zoltan", Race.Goblin(), Gender.Male, "18", Profession.Get(ProfessionCode.Knight), new int[] { 30, 5, 5, 13, 13, 20 });
            // TODO: put the real items for the standard party
            pc.Inventory.TryInsertItemAt(Item.GetByCode(1), 0, 0);
            pc.Inventory.TryInsertItemAt(Item.GetByCode(47), 1, 0);
            pc.Inventory.TryInsertItemAt(Item.GetByCode(66), 3, 0);
            pc.Inventory.TryInsertItemAt(Item.GetByCode(91), 0, 7);
            dummyParty.Chars.Add(pc);
            //pc = CreateDummyChar("Roderick", Race.Human(), Gender.Male, "04", Profession.Get(ProfessionCode.Thief), new int[] { 13, 9, 9, 13, 13, 13 });
            pc = CreateDummyChar("Roderick", Race.Dwarf(), Gender.Male, "13", Profession.Get(ProfessionCode.Thief), new int[] { 13, 9, 9, 13, 13, 13 });
            pc.Inventory.TryInsertItemAt(Item.GetByCode(15), 0, 0);
            pc.Inventory.TryInsertItemAt(Item.GetByCode(66), 1, 0);
            dummyParty.Chars.Add(pc);
            pc = CreateDummyChar("Serena", Race.Dwarf(), Gender.Female, "15", Profession.Get(ProfessionCode.Cleric), new int[] { 12, 9, 20, 20, 7, 11 });
            pc.Inventory.TryInsertItemAt(Item.GetByCode(50), 0, 0);
            pc.Inventory.TryInsertItemAt(Item.GetByCode(71), 1, 0);
            dummyParty.Chars.Add(pc);
            pc = CreateDummyChar("Alexis", Race.Elf(), Gender.Female, "11", Profession.Get(ProfessionCode.Sorcerer), new int[] { 5, 30, 9, 13, 13, 13 });
            pc.Inventory.TryInsertItemAt(Item.GetByCode(61), 0, 0);
            pc.Inventory.TryInsertItemAt(Item.GetByCode(66), 1, 0);
            dummyParty.Chars.Add(pc);
            return dummyParty;
        }

        private static PlayingCharacter CreateDummyChar(string name, Race race, Gender gender, string portraitCode, Profession profession, int[] attributes) {
            var pc = new PlayingCharacter(name, race, gender, portraitCode);
            pc.Profession = profession;
            //pc.Skills
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
    }
}

