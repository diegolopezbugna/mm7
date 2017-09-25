using System;

namespace Business
{
    public class EnemyInfo
    {
        public int DamageMin { get; set; }
        public int DamageMax { get; set; }
        public int MonsterLevel { get; set; }
        public int Armor { get; set; }
        public string Name { get; set; }
        public int LootGoldMin { get; set; }
        public int LootGoldMax { get; set; }

        // TODO: store hitPoints here?

        public EnemyInfo()
        {
        }
    }
}

