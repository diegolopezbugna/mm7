using System.Collections;
using System.Collections.Generic;

namespace Business
{
    public class PlayingCharacter
    {
        public Race Race { get; private set; }
        public string Portrait { get; private set; }
        public Profession Profession { get; private set; }

        public string Name { get; private set; }

        public int Level { get; private set; }
        public long Experience { get; private set; }

        public int HitPoints { get; private set; }
        public int MaxHitPoints { 
            get { 
                return Profession.StartingHitPoints + 
                    Profession.HitPointsPerLevel * (GetAttributeTableValue(Endurance) + Level); 
            } 
        }

        public int SpellPoints { get; private set; }
        public int MaxSpellPoints { 
            get { 
                var professionSpellPointsAttributeValue = Profession.GetSpellPointsAttributeValue(this);
                return Profession.StartingSpellPoints + 
                    Profession.SpellPointsPerLevel * (GetAttributeTableValue(professionSpellPointsAttributeValue) + Level);
            }
        }

        public int Might { get; private set; }
        public int Intellect { get; private set; }
        public int Personality { get; private set; }
        public int Endurance { get; private set; }
        public int Accuracy { get; private set; }
        public int Speed { get; private set; }

        // TODO: resistances

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