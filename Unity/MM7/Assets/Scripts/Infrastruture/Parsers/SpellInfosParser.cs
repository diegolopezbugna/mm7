using System;
using UnityEngine;

namespace Business
{
    public class SpellInfosParser : TxtParser<SpellInfo>
    {
        public SpellInfosParser(string filepath) : base(filepath) {
        }

        #region implemented abstract members of TxtParser

        public override SpellInfo ParseValues(string[] values)
        {
            try
            {
                var spellInfo = new SpellInfo();
                spellInfo.Code = int.Parse(values[0]);
                spellInfo.SkillCode = values[2].ToEnum<SkillCode>(SkillCode.None);
                spellInfo.Name = values[3];
                spellInfo.Description = values[6];
                spellInfo.Normal = values[7];
                spellInfo.Expert = values[8];
                spellInfo.Master = values[9];
                spellInfo.GrandMaster = values[10];
                return spellInfo;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
                return null;
            }
        }

        #endregion
    }
}

