using System;

namespace Business
{
    public class DungeonEntranceInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoFilename { get; set; }
        public string EnterText { get; set; }
        public string SceneName { get; set; }

        public DungeonEntranceInfo()
        {
        }

        // TODO: remove hardcoding! Read from TXTs
        public static DungeonEntranceInfo GetByLocationCode(string locationCode)
        {
            switch (locationCode)
            {
                case "191":
                    return new DungeonEntranceInfo() { 
                        Name = "The Temple of the Moon", 
                        VideoFilename = "Out01 Temple of the Moon",
                        Description = "The dripping of water and a strange, unidentifiable squeaking are the only noises that seem to come from the entrance to this cave.",
                        EnterText = "Enter the cave",
                        SceneName = "TempleOfTheMoon",
                    };
                case "192":
                    return new DungeonEntranceInfo() { 
                        Name = "The Dragon's Lair", 
                        VideoFilename = "Out01 Dragon Cave", 
                        Description = "A large dragon obviously makes his residence in these caves. Perhaps this is where those missing adventurers are...",
                        EnterText = "Enter the cave",
                        SceneName = "DragonsCave",
                    };
            }
            return null;
        }
    }
}

