using System;

namespace Business
{
    public class DungeonEntranceInfo
    {
        public string LocationCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoFilename { get; set; }
        public string EnterText { get; set; }
        public string LeaveText { get; set; }
        public string EnterSceneName { get; set; }
        public string LeaveSceneName { get; set; }

        public DungeonEntranceInfo()
        {
        }

        // TODO: remove hardcoding! Read from TXTs
        public static DungeonEntranceInfo GetByLocationCode(string locationCode)
        {
            switch (locationCode)
            {
//                case "191":
//                    return new DungeonEntranceInfo() { 
//                        LocationCode = locationCode,
//                        Name = "The Temple of the Moon", 
//                        VideoFilename = "caveEntrance",
//                        Description = "The dripping of water and a strange, unidentifiable squeaking are the only noises that seem to come from the entrance to this cave.",
//                        EnterText = "Enter the cave",
//                        LeaveText = "Leave",
//                        EnterSceneName = "TempleOfTheMoon",
//                        LeaveSceneName = "EmeraldIsland",
//                    };
                case "191":
                    return new DungeonEntranceInfo() { 
                        LocationCode = locationCode,
                        EnterText = "Enter the cave",
                        LeaveText = "Leave",
                        EnterSceneName = "TempleOfTheMoon",
                        LeaveSceneName = "EmeraldIsland",
                    };
//                case "192":
//                    return new DungeonEntranceInfo() { 
//                        LocationCode = locationCode,
//                        Name = "The Dragon's Lair", 
//                        VideoFilename = "Out01 Dragon Cave", 
//                        Description = "A large dragon obviously makes his residence in these caves. Perhaps this is where those missing adventurers are...",
//                        EnterText = "Enter the cave",
//                        LeaveText = "Leave",
//                        EnterSceneName = "DragonsCave",
//                        LeaveSceneName = "EmeraldIsland",
//                    };
                case "192":
                    return new DungeonEntranceInfo() { 
                        LocationCode = locationCode,
                        EnterText = "Enter the cave",
                        LeaveText = "Leave",
                        EnterSceneName = "DragonsCave",
                        LeaveSceneName = "EmeraldIsland",
                    };
            }
            return null;
        }
    }
}

