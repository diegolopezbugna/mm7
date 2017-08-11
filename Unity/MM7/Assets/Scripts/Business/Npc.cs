using System;
using System.Collections.Generic;

namespace Business
{
    public class NpcTopic {
        public string Title { get; set; }
        public string Description { get; set; }

        public NpcTopic(string title, string description) {
            Title = title;
            Description = description;
        }
    }
    
    public class Npc
    {
        public string Name { get; set; }
        public int PictureCode { get; set; }
        public List<string> Greetings { get; set; }
        public List<NpcTopic> Topics { get; set; }



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

            if (locationCode == "225")
            {
                var npc = new Npc() { 
                    Name = "Mia Lucille",
                    PictureCode = 216, 
                    Greetings = new List<string>() { "Greetings, I'm Mia.  Do you need something?", "Yes?  What do you want?" },
                    Topics = new List<NpcTopic>() { 
                        new NpcTopic("Dragonflies", "Wild Dragonflies have infested the northwestern side of Emerald Island recently, making it dangerous to store things in our shed out there.  Dragonflies aren't terribly powerful, but they are fast and can even occasionally shoot fire at you.  Don't take them too lightly!"),
                        new NpcTopic("Dragon", "The Dragon of Emerald Island lives in a cave in the northeast.  I wouldn't go there, though, he doesn't like visitors.  He spares our town so that we may pay him tribute, and in return he keeps pirates and undesirables out.")}
                };
                npcs.Add(npc);
            }

            return npcs;
        }

    }
}

