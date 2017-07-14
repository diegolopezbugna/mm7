using System.Collections;
using System.Collections.Generic;

namespace Business
{
    public class PlayingCharacter
    {
        public string Name { get; private set; }
        public Race Race { get; private set; }
        public string PortraitCode { get; private set; }

        public Profession Profession { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }

        public int HitPoints { get; set; }
        public int MaxHitPoints { 
            get { 
                return Profession.StartingHitPoints + 
                    Profession.HitPointsPerLevel * (GetAttributeTableValue(Endurance) + Level); 
            } 
        }

        public int SpellPoints { get; set; }
        public int MaxSpellPoints { 
            get { 
                var professionSpellPointsAttributeValue = Profession.GetSpellPointsAttributeValue(this);
                return Profession.StartingSpellPoints + 
                    Profession.SpellPointsPerLevel * (GetAttributeTableValue(professionSpellPointsAttributeValue) + Level);
            }
        }

        public int Might { get; set; }
        public int Intellect { get; set; }
        public int Personality { get; set; }
        public int Endurance { get; set; }
        public int Accuracy { get; set; }
        public int Speed { get; set; }

        // TODO: resistances

        public PlayingCharacter(string name, Race race, string portraitCode) {
            Name = name;
            Race = race;
            PortraitCode = portraitCode;
            Level = 1;
            Experience = 0;
        }

        public int GetAttributeTableValue(int attributeValue) {
            if (attributeValue < 3)
                return -6;
            else if (attributeValue <= 21)
                return (attributeValue / 2 + attributeValue % 2) - 7;
            else if (attributeValue <= 40)
                return attributeValue / 5;
            else
                return attributeValue / 25 + 7;
        }
    }
}