using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Business
{
    public class SpellInfo
    {
        private static Dictionary<int, SpellInfo> _allSpellInfos = null;
        private static Dictionary<int, SpellInfo> AllSpellInfos {
            get {
                if (_allSpellInfos == null)
                {
                    _allSpellInfos = new Dictionary<int, SpellInfo>();
                    lock (_allSpellInfos)
                    {
                        var parser = new SpellInfosParser("Data/SPELLS");
                        foreach (var spell in parser.Entities)
                            _allSpellInfos.Add(spell.Code, spell);
                    }
                }
                return _allSpellInfos;
            }
        }

        public int Code { get; set; }
        public SkillCode SkillCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Normal { get; set; }
        public string Expert { get; set; }
        public string Master { get; set; }
        public string GrandMaster { get; set; }
        public int SpellPointsCost { get; set; }

        public float SpellBookPosX { get; set; }
        public float SpellBookPosY { get; set; }
        public int ResourceIndex { get; set; }

        private Texture _textureOn;
        public Texture TextureOn {
            get {
                if (_textureOn == null)
                    _textureOn = Resources.Load<Texture>(string.Format("Spells/{0}/{1}On{2:D2}", SkillCode, SkillCode.ToString().Replace("Magic", ""), ResourceIndex));
                return _textureOn;
            }
        }

        private Texture _textureOff;
        public Texture TextureOff {
            get {
                if (_textureOff == null)
                    _textureOff = Resources.Load<Texture>(string.Format("Spells/{0}/{1}Off{2:D2}", SkillCode, SkillCode.ToString().Replace("Magic", ""), ResourceIndex));
                return _textureOff;
            }
        }

        public SpellInfo()
        {
        }

        public static SpellInfo GetByCode(int code) {
            return AllSpellInfos[code];
        }

        public static List<SpellInfo> GetAllBySkill(SkillCode skillCode) {
            return AllSpellInfos.Values.Where(s => s.SkillCode == skillCode).ToList();
        }
    }
}

