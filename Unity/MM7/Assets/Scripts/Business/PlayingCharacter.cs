﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Business
{
    // TODO: EQUIPPED RINGS AND AMULETS BONUSES
    public class PlayingCharacter
    {
        public string Name { get; private set; }
        public Race Race { get; private set; }
        public Gender Gender { get; private set; }
        public string PortraitCode { get; private set; }

        public Profession Profession { get; set; }
        public int Age { get; set; }
        public int Level { get; set; }
        public int SkillPointsLeft { get; private set; }
        public long Experience { get; set; }

        public Dictionary<SkillCode, SkillStatus> Skills { get; private set; }

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

        public float LastAttackTimeFrom { get; set; }
        public float LastAttackTimeTo { get; set; }

        public bool IsActive
        {
            get
            {
                return ConditionStatus != ConditionStatus.Unconscious && 
                    ConditionStatus != ConditionStatus.Dead;
            }
        }

        public PlayingCharacter(string name, Race race, Gender gender, string portraitCode, SkillCode[] startingSkills) {
            Name = name;
            Race = race;
            Gender = gender;
            PortraitCode = portraitCode;
            Age = Random.Range(18, 22);
            Level = 1;
            Skills = new Dictionary<SkillCode, SkillStatus>();
            foreach (var s in startingSkills)
                Skills.Add(s, new SkillStatus(s));
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
                // TODO: add skills, spell bonus
                var ac = GetAttributeTableValue(Speed);
                if (EquippedItems.Armor != null)
                    ac += EquippedItems.Armor.GetArmorBonus();
                if (EquippedItems.Helm != null)
                    ac += EquippedItems.Helm.GetArmorBonus();
                if (EquippedItems.Cloak != null)
                    ac += EquippedItems.Cloak.GetArmorBonus();
                if (EquippedItems.Gauntlets[0] != null)
                    ac += EquippedItems.Gauntlets[0].GetArmorBonus();
                if (EquippedItems.Gauntlets[1] != null)
                    ac += EquippedItems.Gauntlets[1].GetArmorBonus();
                if (EquippedItems.Shield != null)
                    ac += EquippedItems.Shield.GetArmorBonus();
                if (EquippedItems.Boots != null)
                    ac += EquippedItems.Boots.GetArmorBonus();
                // TODO: rings?
                return ac;
            }
        }

        public int AttackBonus {
            get {
                // TODO: add skills, spell bonus
                var b = GetAttributeTableValue(Accuracy);
                if (EquippedItems.WeaponRight != null)
                    b += EquippedItems.WeaponRight.GetAttackBonus();
                if (EquippedItems.IsDualWeaponsWielding)
                    b += EquippedItems.WeaponLeft.GetAttackBonus();
                return b;
            }
        }

        public int DamageMin {
            get {
                // TODO: add skills, spell bonus
                var d = GetAttributeTableValue(Might);
                if (EquippedItems.WeaponRight != null)
                    d += EquippedItems.WeaponRight.GetMinDamage(EquippedItems.IsDualHandWeapon1or2Equipped);
                if (EquippedItems.IsDualWeaponsWielding)
                    d += EquippedItems.WeaponLeft.GetMinDamage();
                return d > 0 ? d : 0;
            }
        }

        public int DamageMax {
            get {
                // TODO: add skills, spell bonus
                var d = GetAttributeTableValue(Might);
                if (EquippedItems.WeaponRight != null)
                    d += EquippedItems.WeaponRight.GetMaxDamage(EquippedItems.IsDualHandWeapon1or2Equipped);
                if (EquippedItems.IsDualWeaponsWielding)
                    d += EquippedItems.WeaponLeft.GetMaxDamage();
                return d > 0 ? d : 0;
            }
        }

        public int RangedAttackBonus {
            get {
                // TODO: add skills, spell bonus
                var b = GetAttributeTableValue(Accuracy);
                if (EquippedItems.Missile != null)
                    b += EquippedItems.Missile.GetAttackBonus();
                return b;
            }
        }

        public int RangedDamageMin {
            get {
                // TODO: add skills, spell bonus
                if (EquippedItems.Missile != null)
                    return EquippedItems.Missile.GetMinDamage();
                else
                    return 0;
            }
        }

        public int RangedDamageMax {
            get {
                // TODO: add skills
                if (EquippedItems.Missile != null)
                    return EquippedItems.Missile.GetMaxDamage();
                else
                    return 0;
            }
        }

        public float RecoveryTime {
            get {
                // TODO: formula! depends on speed? armor?
                return 4f;
            }
        }

        public float RangedRecoveryTime {
            get {
                // TODO: formula! depends on speed? armor?
                return 4f;
            }
        }

        public ConditionStatus ConditionStatus { get; set; }

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

        public int GetTotalSkillBonus(SkillCode skillCode) {
            if (!Skills.ContainsKey(skillCode))
                return 0; // TODO: NPCs, items

            return Skills[skillCode].Points; // TODO: Skill level modifiers, NPC bonuses, items
        }
    }
}