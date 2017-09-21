using System;

namespace Business
{
    public class Enemy
    {
        public int DamageMin { get; set; }
        public int DamageMax { get; set; }
        public int MonsterLevel { get; set; }
        public int Armor { get; set; }
        public string Name { get; set; }

        // TODO: store hitPoints here?

        public Enemy()
        {
        }
    }
}

