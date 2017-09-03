﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Business
{
    public enum EquipSlot {
        None,
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
        public EquipSlot EquipSlot { get; set; }
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
                    var itemsParser = new ItemsParser("Data/ITEMS");
                    foreach (var item in itemsParser.Entities)
                        allItems.Add(item.Code, item);
                }
            }
            return allItems[code].MemberwiseClone() as Item;
        }

        public bool IsEquipmentTextureVariant {
            get {
                return EquipSlot == EquipSlot.Armor ||
                    EquipSlot == EquipSlot.Belt ||
                    EquipSlot == EquipSlot.Helm ||
                    EquipSlot == EquipSlot.Boots ||
                    EquipSlot == EquipSlot.Cloak;
            }
        }

        public Texture GetEquipmentTexture(PlayingCharacter playingCharacter) {
            var variant = playingCharacter.Gender == Gender.Female ? 2 : 1;
            if (playingCharacter.Race.RaceCode == RaceCode.Dwarf)
                variant += 2;
            var texture = Resources.Load(string.Format("Items/{0}v{1}", PictureFilename, variant)) as Texture;
            if (texture == null)
            {
                variant -= 2;
                texture = Resources.Load(string.Format("Items/{0}v{1}", PictureFilename, variant)) as Texture;
            }
            return texture ?? Texture;
        }
    }

}

