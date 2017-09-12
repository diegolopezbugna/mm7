using System;
using System.Collections.Generic;

namespace Business
{
    public class SpellInfo
    {
        private static Dictionary<int, SpellInfo> allSpellInfos;

        public int Code { get; set; }
        public SkillCode SkillCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Normal { get; set; }
        public string Expert { get; set; }
        public string Master { get; set; }
        public string GrandMaster { get; set; }
        public int SpellPointsCost { get; set; }

        public SpellInfo()
        {
        }

        public static SpellInfo GetByCode(int code) {
            if (allSpellInfos == null)
            {
                allSpellInfos = new Dictionary<int, SpellInfo>();
                lock (allSpellInfos)
                {
                    var parser = new SpellInfosParser("Data/SPELLS");
                    foreach (var spell in parser.Entities)
                        allSpellInfos.Add(spell.Code, spell);
                }
            }
            return allSpellInfos[code];
        }
    }
}

