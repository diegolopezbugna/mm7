﻿using System;
using System.Collections.Generic;
using Business;
using Infrastructure;

namespace Business
{
    public enum ShopType
    {
        None,
        Inn,
        Healer,
        Armory,
        WeaponSmith,
        MagicShop,
        Alchemist,
        FireGuild,
        AirGuild,
        WaterGuild,
        EarthGuild,
        MindGuild,
        BodyGuild,
        SpiritGuild,
        LightMagicGuild,
        DarkMagicGuild,
    }

    public enum GuildLevel
    {
        Initiate,
        Adept,
        Master,
        Paramount,
    }

    public enum ShopActionType
    {
        None,
        RentRoom,
        BuyFood,
        Heal,
        Donate,
        BuyStandard,
        BuySpecial,
        BuySpells,
        Sell,
        IdentifyItem,
        RepairItem,
        LearnSkills,
    }

    // FROM MM7!!!
    //shopWeap_variation_ord = 1,1,2,2,4,4,3,2,3,3,2,3,2,2
    //shopWeap_variation_spc = 2,2,3,3,5,5,4,3,4,4,4,4,4,4
    //shopArmr_variation_ord = 1,1,2,2,4,4,3,2,3,3,3,3,3,4
    //shopArmr_variation_spc = 2,2,3,3,5,5,4,3,4,4,4,4,4,5
    //shopMagic_treasure_lvl =    1, 1, 2, 2, 4, 4, 3, 2, 2, 2, 2, 2, 2
    //shopMagicSpc_treasure_lvl = 2, 2, 3, 3, 5, 5, 4, 3, 3, 3, 3, 3, 3
    //shopAlch_treasure_lvl =     1, 1, 2, 2, 3, 3, 4, 4, 2, 2, 2, 2
    //shopAlchSpc_treasure_lvl =  2, 2, 3, 3, 4, 4, 5, 5, 3, 2, 2, 2

    public class Shop
    {
        public ShopType ShopType { get; set; }
        public int TreasureLevelStandard { get; set; }
        public int TreasureLevelSpecial { get; set; }
        public GuildLevel GuildLevel { get; set; }
        public float ShopMultiplier { get; set; }

        public Shop(ShopType shopType, int shopMultiplier)
        {
            ShopType = shopType;
            ShopMultiplier = shopMultiplier;
        }

        public Shop(ShopType shopType, float shopMultiplier, int treasureLevelStandard, int treasureLevelSpecial)
        {
            ShopType = shopType;
            ShopMultiplier = shopMultiplier;
            TreasureLevelStandard = treasureLevelStandard;
            TreasureLevelSpecial = treasureLevelSpecial;
        }

        public Shop(ShopType shopType, GuildLevel guildLevel)
        {
            ShopType = shopType;
            GuildLevel = guildLevel;
        }

        public static List<NpcTopic> GetInnShopTopics(int shopMultiplier)
        {
            return new List<NpcTopic>()
            { 
                new NpcTopic(string.Format("Rent room for {0} gold", shopMultiplier / 2), ShopActionType.RentRoom),  // TODO: depends on selected char merchant skill!!!
                new NpcTopic(string.Format("Fill packs to {0} days for {1} gold", shopMultiplier, shopMultiplier / 3), ShopActionType.BuyFood),  // TODO: depends on selected char merchant skill!!!
                new NpcTopic("Learn skills", ShopActionType.LearnSkills),
            };
        }

        public static List<NpcTopic> GetHealerShopTopics()
        {
            return new List<NpcTopic>()
            { 
                new NpcTopic("Heal {0} gold", ShopActionType.Heal),
                new NpcTopic("Donate", ShopActionType.Donate),
                new NpcTopic("Learn skills", ShopActionType.LearnSkills),
            };
        }

        public static List<NpcTopic> GetCommonShopTopics()
        {
            var topics = new List<NpcTopic>();
            if (Game.Instance.IsBuyStandardEnabled)
                topics.Add(new NpcTopic(Localization.Instance.Get("BuyStandard"), ShopActionType.BuyStandard));
            topics.Add(new NpcTopic(Localization.Instance.Get("BuySpecial"), ShopActionType.BuySpecial));
            topics.Add(new NpcTopic(Localization.Instance.Get("Sell"), ShopActionType.Sell));
            if (Game.Instance.IsIdentifyEnabled)
                topics.Add(new NpcTopic(Localization.Instance.Get("IdentifyItem"), ShopActionType.IdentifyItem));
            if (Game.Instance.IsRepairEnabled)
                topics.Add(new NpcTopic(Localization.Instance.Get("RepairItem"), ShopActionType.RepairItem));
            topics.Add(new NpcTopic(Localization.Instance.Get("LearnSkills"), ShopActionType.LearnSkills));
            return topics;
        }

        public static List<NpcTopic> GetGuildShopTopics()
        {
            return new List<NpcTopic>()
            { 
//                new NpcTopic("You must be a member of this guild to study here", ""), // TODO: guild membership
                new NpcTopic("Buy spells", ShopActionType.BuySpells),
                new NpcTopic("Learn skills", ShopActionType.LearnSkills),
            };
        }

        public static int GetNumberOfBooksByGuildLevel(GuildLevel guildLevel)
        {
            int booksInGuildLevel;
            if (guildLevel == GuildLevel.Initiate)
                booksInGuildLevel = 4;
            else if (guildLevel == GuildLevel.Adept)
                booksInGuildLevel = 7;
            else if (guildLevel == GuildLevel.Master)
                booksInGuildLevel = 10;
            else
                booksInGuildLevel = 11;
            return booksInGuildLevel;
        }

    }
}

