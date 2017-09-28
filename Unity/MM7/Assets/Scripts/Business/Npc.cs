using System;
using System.Collections.Generic;

namespace Business
{
    public class Npc
    {
        public string Name { get; set; }
        public int PictureCode { get; set; }
        public List<string> Greetings { get; set; }
        public Shop Shop { get; set; }
        public List<NpcTopic> Topics { get; set; }

        private int currentGreetingIndex = 0;

        public string NextGreeting() {
            var greeting = "";
            greeting = Greetings[currentGreetingIndex];
            if (currentGreetingIndex + 1 < Greetings.Count)
                currentGreetingIndex += 1;
            return greeting;
        }

        public static List<Npc> GetByLocationCode(string locationCode) {

            // Leer 2dEvents y buscar por 1ra columna
            // -> name, ratios, open, closed
            //  -> no hace falta por ahora

            // leer npcdata y buscar por 2dLocation
            // -> name, pic, greet#, events#[], 

            // Leer npc greet por greet#
            // -> greet1, greet2

            // Leer npctopic por 1ra columna
            // -> text

            // Leer npctext 


            // TODO: remove hardcoding! Read from TXTs

            List<Npc> npcs = new List<Npc>();

            if (locationCode == "1")
            {
                var npc = new Npc()
                    { 
                        Name = "Tor the Blacksmith",
                        PictureCode = 705, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.WeaponSmith, 1.5f, 1, 2),
                        Topics = Shop.GetCommonShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "15")
            {
                var npc = new Npc()
                    { 
                        Name = "Erik the Armorsmith",
                        PictureCode = 704, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.Armory, 1.5f, 1, 2),
                        Topics = Shop.GetCommonShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "29")
            {
                var npc = new Npc()
                    { 
                        Name = "Thurston the Magician",
                        PictureCode = 712, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.MagicShop, 1.5f, 1, 2),
                        Topics = Shop.GetCommonShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "42")
            {
                var npc = new Npc()
                    { 
                        Name = "Kethry the Alchemist",
                        PictureCode = 702, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.Alchemist, 2f, 1, 2),
                        Topics = Shop.GetCommonShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "74")
            {
                var npc = new Npc()
                    { 
                        Name = "Lauren the Healer",
                        PictureCode = 731, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.Healer, 10),
                        Topics = Shop.GetHealerShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "89")
            {
                var npc = new Npc()
                    { 
                        Name = "Trajan the Instructor",
                        PictureCode = 711, 
                        Greetings = new List<string>() { "" },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("You need 1000 more experience to train to level 2", ""),
                                new NpcTopic("Learn skills", ""),
                            }
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "107")
            {
                var npc = new Npc()
                    { 
                        Name = "Aaron the Innkeep",
                        PictureCode = 706, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.Inn, 6),
                        Topics = Shop.GetInnShopTopics(6),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "139")
            {
                var npc = new Npc()
                    { 
                        Name = "Sethric the Guildmaster",
                        PictureCode = 703, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.FireGuild, GuildLevel.Initiate),
                        Topics = Shop.GetGuildShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "143")
            {
                var npc = new Npc()
                    { 
                        Name = "Jenny the Guildmaster",
                        PictureCode = 164, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.AirGuild, GuildLevel.Initiate),
                        Topics = Shop.GetGuildShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "155")
            {
                var npc = new Npc()
                    { 
                        Name = "Fialt the Guildmaster",
                        PictureCode = 722, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.SpiritGuild, GuildLevel.Initiate),
                        Topics = Shop.GetGuildShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "163")
            {
                var npc = new Npc()
                    { 
                        Name = "Standish the Guildmaster",
                        PictureCode = 794, 
                        Greetings = new List<string>() { "" },
                        Shop = new Shop(ShopType.BodyGuild, GuildLevel.Initiate),
                        Topics = Shop.GetGuildShopTopics(),
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "186")
            {
                var npc = new Npc()
                    { 
                        Name = "Lord Markham",
                        PictureCode = 709, 
                        Greetings = new List<string>() { "I am Lord Markham, the benefactor providing the castle that is the prize of the this contest.", "Nice to see you again, how are you doing on the hunt?" },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Castle Harmondale", "If you win, you'll be in charge of one of the most scenic areas in all Erathia!  Harmondale is just outside of the Tularean Forest, right on the edge of the Elf-Human border.  And I'm sure you'll love the castle.  It's a bit of a fixer-upper, but it's quite roomy and has excellent ventilation.  It breaks my heart to part with this property, but I feel that the time has come for me to give something back to the people."),
                                new NpcTopic("The Hunt", "Isn't this hunt exciting?  I really am grateful you came to my little event, and I hope you have fun, even if you don't win.  I think it's great that everyone is competing in a spirit of good sportsmanship and camaraderie."),
                                new NpcTopic("Missing Contestants", "Keep in mind I have a 1000 gold reward for the group to bring back information on the contestants that have disappeared."),
                            }
                    };
                npcs.Add(npc);
                npc = new Npc()
                    { 
                        Name = "Thomas the Judge",
                        PictureCode = 707, 
                        Greetings = new List<string>() { "Greetings, my name is Thomas.  I am the judge of the contest.", "Have you collected everything on the list yet?" },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Rules of the Hunt", "Good afternoon.  My duty is to verify that you have all the items necessary to win the contest.  You are required to bring a red potion, a longbow, a floor tile from the Temple of the Moon, a wealthy hat, seashell, and an instrument to me.  You can bring them in any order, just show them to me one at a time so that I may verify them."),
                                new NpcTopic("What do you have?", "I'm sorry, but nothing you have is necessary for the hunt.  I don't mean to belittle what you have, but I'm not looking for any of it.")
                            }
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "224")
            {
                var npc = new Npc()
                    { 
                        Name = "Donna Wyrith",
                        PictureCode = 219, 
                        Greetings = new List<string>() { "Hello, my name is Donna, I live here with my daughter Sally.", "Hello again, what can I do for you?" },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Missing People", "Hmm… I recall a few strangers poking around the entrance to the Dragon's Cave recently. I didn't notice if they went inside, but I haven't seen them around since. They must have realized how dangerous that place is and headed back to town."),
                                new NpcTopic("Abandoned Temple", "The cave right behind my house is not the Abandoned Temple. It belongs to Morcarack the Pitiless, the Dragon of Emerald Island. He doesn't appreciate visitors, so I wouldn't advise going there. The Abandoned Temple is buried in the hill south of my house. You can get to it by entering the caves at the top of the hill.")
                            }
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "225")
            {
                var npc = new Npc()
                { 
                    Name = "Mia Lucille",
                    PictureCode = 216, 
                    Greetings = new List<string>() { "Greetings, I'm Mia.  Do you need something?", "Yes?  What do you want?" },
                    Topics = new List<NpcTopic>()
                    { 
                        new NpcTopic("Dragonflies", "Wild Dragonflies have infested the northwestern side of Emerald Island recently, making it dangerous to store things in our shed out there.  Dragonflies aren't terribly powerful, but they are fast and can even occasionally shoot fire at you.  Don't take them too lightly!"),
                        new NpcTopic("Dragon", "The Dragon of Emerald Island lives in a cave in the northeast.  I wouldn't go there, though, he doesn't like visitors.  He spares our town so that we may pay him tribute, and in return he keeps pirates and undesirables out.")
                    }
                };
                npcs.Add(npc);
            }
            else if (locationCode == "238")
            {
                var npc = new Npc()
                    { 
                        Name = "William Darvees",
                        PictureCode = 77, 
                        Greetings = new List<string>() { "Ahoy matey! I be William Darvees, captain of this vessel.", "Aye, good to see you again, mates!" },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Boat Travel", "Sorry mates, this vessel's moored until a winner has been declared in the contest."),
                            }
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "239")
            {
                var npc = new Npc()
                    { 
                        Name = "Carolyn Weathers",
                        PictureCode = 158, 
                        Greetings = new List<string>() { "Hello, I'm Carolyn. I handle the guild memberships for the Fire and Air Guilds here on the island." },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Air Guild Membership", "Air is the element the sorcerers join the profession to learn!  We have all the best spells--including the glorious Fly spell.  Don't miss out on this fantastic opportunity to join the Air guild!  Just 50 gold!"),
                                new NpcTopic("Fire Guild Membership", "Why be subtle when you can learn Fire magic?  Joining our guild costs but 50 gold for a lifetime membership.")
                            }
                    };
                npcs.Add(npc);
            }
            else if (locationCode == "240")
            {
                var npc = new Npc()
                    { 
                        Name = "Roger Tellmar",
                        PictureCode = 10, 
                        Greetings = new List<string>() { "I'm Roger and I sell guild memberships for the Body and Spirit Guilds on Emerald Island." },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Body Guild Membership", "I am the guild recruiter for the Guild of Body.  A lot of people think that our spells just heal the body, but some can damage the bodies of your opponents too.  The cost is 50 gold pieces."),
                                new NpcTopic("Spirit Guild Membership", "Many believe the most powerful magic to be Light magic, or Dark magic, or something destructive, like Fire.  We at the Spirit guild would like to point out that no magic besides Light magic can raise the dead, and even then it takes a Grandmaster in the element.  If you want power over life and death, choose Spirit magic.  The cost to join is but 50 gold pieces.")
                            }
                    };
                npcs.Add(npc);
            }

            return npcs;
        }


        // TODO: Guardias
        public static Npc GetByCode(string code) {
            if (code == "4")
            {
                var npc = new Npc()
                { 
                    Name = "Ailyssa the Bard",
                    PictureCode = 163, 
                    Greetings = new List<string>() { "Hello, my name is Ailyssa.  I'm the Bard in charge of overseeing the entertainment on Emerald Island for the duration of the Scavenger Hunt.", "Hello again, how are you doing?" },
                    Topics = new List<NpcTopic>()
                    { 
                        new NpcTopic("Scavenger Hunt", "Are you contestants in Lord Markham's Scavenger Hunt?  How neat!  I'm here to provide entertainment to Lord Markham's entourage, the contestants, and to anyone else that would like to hear a song."),
                        new NpcTopic("Instruments", "I own a few instruments, but I only brought my lute with me.  Its old and not quite as well kept as some of the others, but I didn't want one of my good instruments stolen by pirates or damaged from exposure to the humid, salty air."),
                        new NpcTopic("Lute", "You say you need an instrument for the Scavenger Hunt?  I suppose you could buy my lute, but I've had it for such a long time.  I guess I'd part with it for 500 gold.  Interested?", 
                            new List<NpcTopic>() { new NpcTopic("Buy Lute for 500 gold") }), // TODO: complete, gold npc topics?
                    }
                };
                return npc;
            }
            else if (code == "5")  // TODO: sally is near the dragon's cave
            {
                var npc = new Npc()
                    { 
                        Name = "Sally",
                        PictureCode = 150, 
                        Greetings = new List<string>() { "Hi there!  I'm Sally.  I don't usually see people this far from town, are you lost?", "Still out here?  Town's back a ways to the south." },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Ocean", "I love being out on this side of the island, the view of the ocean is much better than from town, don't you agree?"),
                                new NpcTopic("Seashell", "I have some nice sea shells for sale, shells that you can only find on Emerald Island.  Can I sell one to you?  They're only a hundred gold pieces each.", 
                                    new List<NpcTopic>() { new NpcTopic("Buy Seashell for 100 gold") }), // TODO: complete, gold npc topics?
                            }
                    };
                return npc;
            }
            else if (code == "6")
            {
                var npc = new Npc()
                    { 
                        Name = "Mr. Malwick",
                        PictureCode = 21, 
                        Greetings = new List<string>() { "Pssst…  Come here, I have an offer for you.", "Ah, you've returned, no doubt to listen to my proposal." },
                        Topics = new List<NpcTopic>()
                            { 
                                new NpcTopic("Harmondale", "We have reason to believe that whoever is in charge of Harmondale in the next few months will have an unprecedented opportunity to shape the future. That is why I am here today--to make sure the shape of the future is pleasing to my associates. I'm sure you understand."),
                                new NpcTopic("Proposal", "Allow me to introduce myself.  My name is Mr. Malwick.  I represent a group of, shall we say, \"investors in the future\".  I have been observing you since you arrived on this island, and I believe your values and goals have much in common with ours.  I am empowered to grant you a fireball wand to help you win the Hunt today, in exchange for a favor in the future.  What do you say?",
                                    new List<NpcTopic>() { new NpcTopic("Accept Wand"), new NpcTopic("Reject Wand") }), // TODO: complete, custom topics?
                            }
                    };
                return npc;
            }

            return null;
        }


    }
}

