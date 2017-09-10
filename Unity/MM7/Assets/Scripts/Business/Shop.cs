using System;
using System.Collections.Generic;

namespace Business
{
    public enum ShopType
    {
        None,
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

    public enum ShopActionType
    {
        None,
        BuyStandard,
        BuySpecial,
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

        public Shop(ShopType shopType, int treasureLevelStandard, int treasureLevelSpecial)
        {
            ShopType = shopType;
            TreasureLevelStandard = treasureLevelStandard;
            TreasureLevelSpecial = treasureLevelSpecial;
        }

        public static List<NpcTopic> GetCommonShopTopics()
        {
            return new List<NpcTopic>()
            { 
                new NpcTopic("Buy standard", ShopActionType.BuyStandard),
                new NpcTopic("Buy special", ShopActionType.BuySpecial),
                new NpcTopic("Sell", ShopActionType.Sell),
                new NpcTopic("Identify Item", ShopActionType.IdentifyItem),
                new NpcTopic("Repair Item", ShopActionType.RepairItem),
                new NpcTopic("Learn skills", ShopActionType.LearnSkills),
            };
        }
    }
}

