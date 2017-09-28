using System;
using System.Collections.Generic;

namespace Business
{
    public class Building
    {
        public string Name { get; set; }
        public string VideoFilename { get; set; }

        public Building()
        {
        }

        public static Building GetByLocationCode(string locationCode) {

            // TODO: remove hardcoding! Read from TXTs

            switch (locationCode)
            {
                case "1":
                    return new Building() { Name = "The Knight's Blade", VideoFilename = "Human Weapon Smith01" };
                case "15":
                    return new Building() { Name = "Erik's Armory", VideoFilename = "human Armor01" };
                case "29":
                    return new Building() { Name = "Emerald Enchantments", VideoFilename = "Human Magic Shop01" };
                case "42":
                    return new Building() { Name = "The Blue Bottle", VideoFilename = "Human Alchemisht01" };
                case "74":
                    return new Building() { Name = "Healer's Tent", VideoFilename = "Human Temple01" };
                case "89":
                    return new Building() { Name = "Island Training Grounds", VideoFilename = "Human Training Ground01" };
                case "107":
                    return new Building() { Name = "Two Palms Tavern", VideoFilename = "Human Tavern01" };
                case "139":
                    return new Building() { Name = "Initiate Guild of Fire", VideoFilename = "Fire Guild" };
                case "143":
                    return new Building() { Name = "Initiate Guild of Air", VideoFilename = "Air Guild" };
                case "155":
                    return new Building() { Name = "Initiate Guild of Spirit", VideoFilename = "Spirit Guild" };
                case "163":
                    return new Building() { Name = "Initiate Guild of Body", VideoFilename = "Body Guild" };
                case "186":
                    return new Building() { Name = "Markham's Headquarters", VideoFilename = "Lord and Judge Out01" };
                case "224":
                    return new Building() { Name = "Donna Wyrith's Residence", VideoFilename = "Human Poor House 1" };
                case "225":
                    return new Building() { Name = "Mia Lucille' Home", VideoFilename = "Human Poor House 2" };
                case "238":
                    return new Building() { Name = "Lady Margaret", VideoFilename = "Boat01" };
                case "239":
                    return new Building() { Name = "Carolyn Weathers' House", VideoFilename = "Human Medium House 1" };
                case "240":
                    return new Building() { Name = "Tellmar Residence", VideoFilename = "Human Medium House 2" };

            }

            return null;
        }
    }
}

