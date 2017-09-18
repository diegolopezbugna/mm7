using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Business
{
    public enum SpellCodes 
    {
        Fire_FireBolt = 2,
        Fire_Immolation = 8,
        Fire_Incinerate = 11,
        Air_LightningBolt = 18,
        Air_Implosion = 20,
        Water_PoisonSpray = 24,
        Water_IceBolt = 26,
        Water_AcidBurst = 29,
        Earth_RockBlast = 41,
        Body_Heal = 68,
    }
    
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
        public Dictionary<SkillLevel, int> RecoveryTimes { get; set; }
        public int BaseDamage { get; set; }
        public int SkillPointDamageBonus { get; set; }

        public float SpellBookPosX { get; set; }
        public float SpellBookPosY { get; set; }
        public int ResourceIndex { get; set; }
        public string SpellFxName { get; set; }

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

        private List<Texture> _portraitAnimationTextures;
        public List<Texture> PortraitAnimationTextures {
            get {
                if (_portraitAnimationTextures == null)
                {
                    _portraitAnimationTextures = new List<Texture>();
                    lock (_portraitAnimationTextures)
                    {
                        var spellCode = string.Format("{0:D2}", Code);
                        var t = Resources.Load<Texture>("SpellPortraitSprites/sp" + spellCode + "e");
                        if (t != null)
                        {
                            _portraitAnimationTextures.Add(t);
                        }
                        else
                        {
                            var i = 1;
                            while (true)
                            {
                                t = Resources.Load<Texture>("SpellPortraitSprites/sp" + spellCode + "e" + i.ToString());
                                if (t == null)
                                    break;
                                _portraitAnimationTextures.Add(t);
                                i++;
                            }
                        }
                    }
                }
                return _portraitAnimationTextures;
            }
        }


        public SpellInfo()
        {
            RecoveryTimes = new Dictionary<SkillLevel, int>();
        }

        public static SpellInfo GetByCode(int code) {
            return AllSpellInfos[code];
        }

        public static List<SpellInfo> GetAllBySkill(SkillCode skillCode) {
            return AllSpellInfos.Values.Where(s => s.SkillCode == skillCode).ToList();
        }

        public bool NeedsPartyTarget {
            get {
                // TODO: each spell? loaded from .txt?
                if (Code == (int)SpellCodes.Body_Heal)
                    return true;
                return false;
            }
        }

        public bool Needs3dTarget {
            get {
                // TODO: each spell? loaded from .txt?
                switch (Code)
                {
                    case (int)SpellCodes.Fire_FireBolt:
                    case (int)SpellCodes.Air_Implosion:
                        return true;
                }
                return false;
            }
        }

        public bool HasNoTrail {
            get {
                switch (Code)
                {
                    case (int)SpellCodes.Air_Implosion:
                        return true;
                }
                return false;
            }
        }

    }
}

