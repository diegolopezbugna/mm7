using System;
using System.Collections;
using System.Collections.Generic;

using Infrastructure;

namespace Business
{
    public enum SkillCode 
    {
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

    public enum SkillLevel
    {
        None,
        Normal,
        Expert,
        Master,
        Grandmaster,
    }
    
//    public interface Skill
//    {
//        SkillCode SkillCode { get; }
//        SkillLevel Level { get; }
//        int Points { get; }
//    }

    public class Skill {
        
        public SkillCode SkillCode { get; set; }
        public string Name { get; set; }
        public string Descrition { get; set; }

        private static object allLocker = new Object();

        private static IList<Skill> _all;
        public static IList<Skill> All() {
            if (_all == null)
            {
                lock (allLocker)
                {
                    _all = new List<Skill>();
                    foreach (var s in Enum.GetNames(typeof(SkillCode)))
                    {
                        _all.Add(new Skill() { SkillCode = (SkillCode)Enum.Parse(typeof(SkillCode), s), Name = Localization.Instance.Get(s) });
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

    }


}

