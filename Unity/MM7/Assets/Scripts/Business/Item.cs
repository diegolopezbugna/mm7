using System;
using System.Collections.Generic;

namespace Business
{
    public enum EquipStat {
        Weapon,
        Weapon2,
        Weapon1or2,
        Missile,
        Armor,
        Shield,
        Helm,
        Belt,
        Cloak,
        Gauntlets,
        Boots,
        Ring,
        Amulet,
        WeaponW,
        Gem,
        Reagent,
        Bottle,
        Sscroll,
        Mscroll,
        Book,
    }
    
    public class Item
    {
        public string Code { get; private set; }
        public string Name { get; set; }
        public string NotIdentifiedName { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public SkillCode SkillGroup { get; set; }
        public string Mod1 { get; set; }
        public int Mod2 { get; set; }
        public int IdItemRequiredLevel { get; set; }
        public EquipStat EquipStat { get; set; }
        public float EquipX { get; set; }
        public float EquipY { get; set; }
        public int InventorySlotsRequiredH { get; set; }
        public int InventorySlotsRequiredV { get; set; }

        public Item()
        {
            
        }

        // TODO: read from items.txt
        public static Item GetByCode(string code) {
            if (code == "1")
            {
                return new Item()
                {
                    Code = "1",
                    Name = "Crude Longsword",
                    NotIdentifiedName = "Longsword",
                    Description = "Though notched and dented, this longsword is still an effective weapon.",
                    Value = 50,
                    SkillGroup = SkillCode.Sword,
                    Mod1 = "3d3",
                    Mod2 = 0,
                    IdItemRequiredLevel = 1,
                    EquipStat = EquipStat.Weapon,
                    EquipX = 5,
                    EquipY = 120,
                    InventorySlotsRequiredH = 1,
                    InventorySlotsRequiredV = 4, // TODO: check
                };
            }

            return null;
        }
    }

}

