using System;
using System.Collections;
using System.Collections.Generic;

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
        Fire,
        Air,
        Water,
        Earth,
        Spirit,
        Mind,
        Body,
        Light,
        Dark,
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

//    public class Skill {
//        public SkillCode SkillCode { get; set; }
//        public string Name { get; set; }
//        public string Descrition { get; set; }
//
//        private static IList<Skill> _all;
//        public static IList<Skill> All() {
//            if (_all == null)
//            {
//                _all = new List<Skill>();
//                _all.Add(new Skill() { SkillCode = SkillCode.Axe, Name = "Axe" });
//            }
//        }
//
//    }


}

