﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        gold,
    }
    
    public class Item
    {
        private const int ITEM_GOLD_SMALL = 197;
        private const int ITEM_GOLD_MEDIUM = 198;
        private const int ITEM_GOLD_LARGE = 199;

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
        public int[] RandomItemGenerationChanceByTreasureLevel { get; set; }

        private static Dictionary<int, Item> _allItems;
        private static Dictionary<int, Item> AllItems {
            get {
                if (_allItems == null)
                {
                    _allItems = new Dictionary<int, Item>();
                    lock (_allItems)
                    {
                        var itemsParser = new ItemsParser("Data/ITEMS");
                        foreach (var item in itemsParser.Entities)
                            _allItems.Add(item.Code, item);
                    }
                }
                return _allItems;
            }
        }

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

        public bool IsHandToHandWeapon {
            get {
                return EquipSlot == EquipSlot.Weapon || EquipSlot == EquipSlot.Weapon1or2 || EquipSlot == EquipSlot.Weapon2;
            }
        }

        public bool IsLongRangeWeapon {
            get {
                return EquipSlot == EquipSlot.Missile;
            }
        }

        public bool IsWandWeapon {
            get {
                return EquipSlot == EquipSlot.WeaponW;
            }
        }

        public bool IsArmor {
            get {
                return EquipSlot == EquipSlot.Armor || 
                    EquipSlot == EquipSlot.Helm || 
                    EquipSlot == EquipSlot.Shield || 
                    EquipSlot == EquipSlot.Gauntlets || 
                    EquipSlot == EquipSlot.Boots ||
                    EquipSlot == EquipSlot.Cloak;
            }
        }

        public int GetArmorBonus() {
            return int.Parse(Mod1) + Mod2;
        }

        public int GetAttackBonus() {
            return Mod2;
        }

        public int GetShootBonus() {
            return Mod2;
        }

        public int GetMinDamage(bool shouldAddOneDice = false) {
            if (string.IsNullOrEmpty(Mod1))
                return 0;
            if (Mod1.Length < 3)
                return 0;
            var dices = int.Parse(Mod1.Substring(0, 1));
            if (shouldAddOneDice)
                dices++;
            //var diceFaces = int.Parse(Mod1.Substring(2));
            return dices;
        }

        public int GetMaxDamage(bool shouldAddOneDice = false) {
            if (string.IsNullOrEmpty(Mod1))
                return 0;
            if (Mod1.Length < 3)
                return 0;
            var dices = int.Parse(Mod1.Substring(0, 1));
            var diceFaces = int.Parse(Mod1.Substring(2));
            if (shouldAddOneDice)
                dices++;
            return dices * diceFaces;
        }

        public int GetChargesLeft() {
            // TODO: use wand, control charges
            return Mod2;
        }

        public SpellInfo GetSpellInfoAssociated() {
            if (Mod1.Length > 0)
                return SpellInfo.GetByCode(int.Parse(Mod1.Substring(1)));
            else
                return null;
        }


        public static Item GetByCode(int code) {
            return AllItems[code].MemberwiseClone() as Item;
        }

        public static List<Item> GetItemsToBuyAt(ShopType shopType, int treasureLevel) 
        {
            var items = new List<Item>();

            if (shopType == ShopType.WeaponSmith)
            {
                for (int i = 0; i < 6; i++) // TODO: remove hardcoding
                    items.Add(GenerateItem(new List<EquipSlot>() { EquipSlot.Weapon, EquipSlot.Weapon1or2, EquipSlot.Weapon2 }, treasureLevel));
            }
            else if (shopType == ShopType.Armory)
            {
                for (int i = 0; i < 4; i++) // TODO: remove hardcoding
                    items.Add(GenerateItem(new List<EquipSlot>() { EquipSlot.Gauntlets, EquipSlot.Helm }, treasureLevel));
                for (int i = 0; i < 4; i++) // TODO: remove hardcoding
                    items.Add(GenerateItem(new List<EquipSlot>() { EquipSlot.Armor, EquipSlot.Shield }, treasureLevel));
            }
            else if (shopType == ShopType.MagicShop)
            {
                for (int i = 0; i < 12; i++) // TODO: remove hardcoding
                    items.Add(GenerateItem(SkillCode.None, treasureLevel));
            }
            else if (shopType == ShopType.Alchemist)
            {
                for (int i = 0; i < 6; i++) // TODO: remove hardcoding
                    items.Add(GenerateItem(new List<EquipSlot>() { EquipSlot.Bottle }, treasureLevel));
                for (int i = 0; i < 6; i++) // TODO: remove hardcoding
                    items.Add(GenerateItem(new List<EquipSlot>() { EquipSlot.Reagent }, treasureLevel));
            }

            return items;
        }

        public static List<Item> GetBooksToBuyAt(ShopType shopType, GuildLevel guildLevel) 
        {
            var numberOfBooksByGuildLevel = Shop.GetNumberOfBooksByGuildLevel(guildLevel);
            var booksFromThisGuild = AllItems.Values.Where(i => i.SkillGroup == (SkillCode)(Enum.Parse(typeof(SkillCode), shopType.ToString().Replace("Guild", "Magic"))));
            var booksFromThisGuildLevel = booksFromThisGuild.Take(numberOfBooksByGuildLevel).ToList();

            var books = new List<Item>();
            for (int i = 0; i < 12; i++) // TODO: remove hardcoding
                books.Add(booksFromThisGuildLevel[UnityEngine.Random.Range(0, booksFromThisGuildLevel.Count)].MemberwiseClone() as Item);
            return books;
        }
            
        public static Item GenerateItem(int treasureLevel) 
        {
            return GenerateItem(AllItems.Values, treasureLevel);
        }

        public static Item GenerateItem(List<EquipSlot> equipSlots, int treasureLevel) 
        {
            var thisTypeItems = AllItems.Values.Where(i => equipSlots.Contains(i.EquipSlot));
            return GenerateItem(thisTypeItems, treasureLevel);
        }

        public static Item GenerateItem(SkillCode skillCode, int treasureLevel) 
        {
            var thisSkillItems = AllItems.Values.Where(i => i.SkillGroup == skillCode);
            return GenerateItem(thisSkillItems, treasureLevel);
        }

        private static Item GenerateItem(IEnumerable<Item> itemsToChooseFrom, int treasureLevel)
        {
            // TODO: special bonuses generation!!

            var totalChance = 0;
            foreach (var i in itemsToChooseFrom) 
                totalChance += i.RandomItemGenerationChanceByTreasureLevel[treasureLevel - 1];

            var randomChance = UnityEngine.Random.Range(0, totalChance);

            var currentChance = 0;
            foreach (var i in itemsToChooseFrom)
            {
                currentChance += i.RandomItemGenerationChanceByTreasureLevel[treasureLevel - 1];
                if (currentChance > randomChance)
                {
                    var generatedItem = i.MemberwiseClone();
                    return generatedItem as Item;
                }
            }

            return itemsToChooseFrom.FirstOrDefault();
        }

        public static Item GenerateGold(int treasureLevel)
        {
            int goldAmount = 0;
            int itemCode = 0;

            switch (treasureLevel)
            {
                case 1:
                    goldAmount = UnityEngine.Random.Range(50, 100);
                    itemCode = ITEM_GOLD_SMALL;
                    break;
                case 2:
                    goldAmount = UnityEngine.Random.Range(100, 200);
                    itemCode = ITEM_GOLD_SMALL;
                    break;
                case 3:
                    goldAmount = UnityEngine.Random.Range(200, 500);
                    itemCode = ITEM_GOLD_MEDIUM;
                    break;
                case 4:
                    goldAmount = UnityEngine.Random.Range(500, 1000);
                    itemCode = ITEM_GOLD_MEDIUM;
                    break;
                case 5:
                    goldAmount = UnityEngine.Random.Range(1000, 2000);
                    itemCode = ITEM_GOLD_LARGE;
                    break;
                case 6:
                    goldAmount = UnityEngine.Random.Range(2000, 5000);
                    itemCode = ITEM_GOLD_LARGE;
                    break;
            }

            var item = Item.GetByCode(itemCode);
            item.Value = goldAmount;
            return item;
        }
    }


}

