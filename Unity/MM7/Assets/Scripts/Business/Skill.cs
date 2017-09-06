using System;
using System.Collections;
using System.Collections.Generic;

using Infrastructure;

namespace Business
{
    public enum SkillCode 
    {
        None,
        Axe,
        Bow,
        Dagger,
        Mace,
        Spear,
        Staff,
        Sword,
        Unarmed,
        Blaster,
        Leather,
        Chain,
        Plate,
        Shield,
        Dodging,
        FireMagic,
        AirMagic,
        WaterMagic,
        EarthMagic,
        SpiritMagic,
        MindMagic,
        BodyMagic,
        LightMagic,
        DarkMagic,
        Alchemy,
        Armsmaster,
        BodyBuilding,
        IdentifyItem,
        IdentifyMonster,
        Learning,
        DisarmTrap,
        Meditation,
        Merchant,
        Perception,
        RepairItem,
        Stealing,
    }

    public enum SkillGroup
    {
        Weapons,
        Magic,
        Armor,
        Misc,
    }

    public enum SkillLevel
    {
        Normal,
        Expert,
        Master,
        GrandMaster,
    }

//    public interface Skill
//    {
//        SkillCode SkillCode { get; }
//        SkillLevel Level { get; }
//        int Points { get; }
//    }

    public class Skill {
        
        public SkillCode SkillCode { get; set; }
        public SkillGroup SkillGroup { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private static object allLocker = new Object();

        private static List<SkillCode> WeaponSkillCodes = new List<SkillCode>() {
            SkillCode.Axe,
            SkillCode.Bow,
            SkillCode.Dagger,
            SkillCode.Mace,
            SkillCode.Spear,
            SkillCode.Staff,
            SkillCode.Sword,
            SkillCode.Unarmed,
            SkillCode.Blaster,
        };

        private static List<SkillCode> ArmorSkillCodes = new List<SkillCode>()
        {
            SkillCode.Leather,
            SkillCode.Chain,
            SkillCode.Plate,
            SkillCode.Shield,
            SkillCode.Dodging,
        };

        private static List<SkillCode> MagicSkillCodes = new List<SkillCode>()
        {
            SkillCode.FireMagic,
            SkillCode.AirMagic,
            SkillCode.WaterMagic,
            SkillCode.EarthMagic,
            SkillCode.SpiritMagic,
            SkillCode.MindMagic,
            SkillCode.BodyMagic,
            SkillCode.LightMagic,
            SkillCode.DarkMagic,
        };

        private static IList<Skill> _all;
        public static IList<Skill> All() {
            if (_all == null)
            {
                lock (allLocker)
                {
                    _all = new List<Skill>();
                    foreach (var s in Enum.GetNames(typeof(SkillCode)))
                    {
                        var skill = new Skill() { SkillCode = (SkillCode)Enum.Parse(typeof(SkillCode), s), Name = Localization.Instance.Get(s) };
                        skill.SkillGroup = GetSkillGroup(skill.SkillCode);
                        _all.Add(skill);
                    }
                }

            }
            return _all;
        }

        public static Skill Get(SkillCode code) {
            foreach (var s in All())
                if (code == s.SkillCode)
                    return s;
            return null;
        }

        public static SkillGroup GetSkillGroup(SkillCode skillCode) {
            if (WeaponSkillCodes.Contains(skillCode))
                return SkillGroup.Weapons;
            else if (ArmorSkillCodes.Contains(skillCode))
                return SkillGroup.Armor;
            else if (MagicSkillCodes.Contains(skillCode))
                return SkillGroup.Magic;
            else
                return SkillGroup.Misc;
        }
    }


}

