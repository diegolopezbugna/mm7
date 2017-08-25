using System;
using System.Collections.Generic;
using UnityEngine;

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
        private static Dictionary<int, Item> allItems;

        public int Code { get; set; }
        public string PictureFilename { get; set; }
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

        private Texture texture;
        public Texture Texture {
            get {
                if (texture == null)
                {
                    texture = Resources.Load("Items/" + PictureFilename) as Texture;
                }
                return texture;
            }
        }

        public static Item GetByCode(int code) {
            if (allItems == null)
            {
                allItems = new Dictionary<int, Item>();
                lock (allItems)
                {
                    var itemsParser = new ItemsParser("Assets/Resources/Data/ITEMS.TXT");
                    foreach (var item in itemsParser.Entities)
                        allItems.Add(item.Code, item);
                }
            }
            return allItems[code].MemberwiseClone() as Item;
        }
    }

}

