﻿using System.Collections;
using System.Collections.Generic;

namespace Business
{
    public class PlayingCharacter
    {
        public string Name { get; private set; }
        public Race Race { get; private set; }
        public Gender Gender { get; private set; }
        public string PortraitCode { get; private set; }

        public Profession Profession { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }

        public int HitPoints { get; set; }
        public int MaxHitPoints { 
            get { 
                return Profession.StartingHitPoints + 
                    Profession.HitPointsPerLevel * (GetAttributeTableValue(Endurance) + Level); 
            } 
        }

        public int SpellPoints { get; set; }
        public int MaxSpellPoints { 
            get { 
                var professionSpellPointsAttributeValue = Profession.GetSpellPointsAttributeValue(this);
                return Profession.StartingSpellPoints + 
                    Profession.SpellPointsPerLevel * (GetAttributeTableValue(professionSpellPointsAttributeValue) + Level);
            }
        }

        public int Might { get; set; }
        public int Intellect { get; set; }
        public int Personality { get; set; }
        public int Endurance { get; set; }
        public int Accuracy { get; set; }
        public int Speed { get; set; }

        // TODO: resistances

        public Inventory Inventory { get; set; }
        public EquippedItems EquippedItems { get; set; }

        public PlayingCharacter(string name, Race race, Gender gender, string portraitCode) {
            Name = name;
            Race = race;
            Gender = gender;
            PortraitCode = portraitCode;
            Level = 1;
            Experience = 0;
            Inventory = new Inventory(14, 9, 454f / 14, 293f / 9); // TODO: dimensions should be on InventoryUI
            EquippedItems = new EquippedItems();
        }

        public int GetAttributeTableValue(int attributeValue) {
            if (attributeValue < 3)
                return -6;
            else if (attributeValue <= 21)
                return (attributeValue / 2 + attributeValue % 2) - 7;
            else if (attributeValue <= 40)
                return attributeValue / 5;
            else
                return attributeValue / 25 + 7;
        }

        public int ArmorClass {
            get {
                // TODO: add items and skills
                return GetAttributeTableValue(Speed);
            }
        }

        public int AttackBonus {
            get {
                // TODO: add items and skills
                return GetAttributeTableValue(Accuracy);
            }
        }

        public int DamageMin {
            get {
                // TODO: add items and skills
                var d = GetAttributeTableValue(Might);
                d += 4;
                return d > 0 ? d : 0;
            }
        }

        public int DamageMax {
            get {
                // TODO: add items and skills
                var d = GetAttributeTableValue(Might);
                d += 8;
                return d > 0 ? d : 0;
            }
        }

        public int RangedAttackBonus {
            get {
                // TODO: add items and skills
                return GetAttributeTableValue(Accuracy);
            }
        }

        public int RangedDamageMin {
            get {
                // TODO: add items and skills
                return 4;
            }
        }

        public int RangedDamageMax {
            get {
                // TODO: add items and skills
                return 8;
            }
        }

        public float RecoveryTime {
            get {
                // TODO: formula! depends on speed? armor?
                return 2f;
            }
        }

        public void EquipItem(Item item) {
            Item itemToBeLost = null;
            EquippedItems.TryEquipItem(item, out itemToBeLost);
        }

        public bool TryEquipItem(Item item, out Item oldEquippedItem) {
            return EquippedItems.TryEquipItem(item, out oldEquippedItem);
        }

        public bool CanEquipItem(Item item) {
            // TODO: check skills and requirements
            return true;
        }

        public void UnequipItem(Item item) {
            EquippedItems.UnequipItem(item);
        }
    }
}