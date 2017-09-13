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
                spellInfo.SpellBookPosX = float.Parse(values[3]);
                spellInfo.SpellBookPosY = float.Parse(values[4]);
                spellInfo.ResourceIndex = int.Parse(values[5]);
                spellInfo.Name = values[6];
                spellInfo.Description = values[7];
                spellInfo.Normal = values[8];
                spellInfo.Expert = values[9];
                spellInfo.Master = values[10];
                spellInfo.GrandMaster = values[11];
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

