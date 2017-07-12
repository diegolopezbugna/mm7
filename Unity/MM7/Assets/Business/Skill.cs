using System;

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
    
    public interface Skill
    {
        SkillCode SkillCode { get; }
        SkillLevel Level { get; }
        int Points { get; }
    }
}

