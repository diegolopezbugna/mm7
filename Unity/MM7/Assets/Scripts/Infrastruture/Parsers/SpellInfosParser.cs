using System;
using System.Collections.Generic;
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
                spellInfo.SpellFxName = values[6];
                spellInfo.SpellPointsCost = int.Parse(values[7]);
                spellInfo.RecoveryTimes = GetRecoveryTimes(values[8]);
                spellInfo.BaseDamage = values[9].Length > 0 ? int.Parse(values[9]) : 0;
                spellInfo.SkillPointDamageBonus = values[10].Length > 0 ? int.Parse(values[10]) : 0;

                spellInfo.Name = values[11];
                spellInfo.Description = values[12];
                spellInfo.Normal = values[13];
                spellInfo.Expert = values[14];
                spellInfo.Master = values[15];
                spellInfo.GrandMaster = values[16];
                return spellInfo;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
                return null;
            }
        }

        private Dictionary<SkillLevel, int> GetRecoveryTimes(string value) {
            var skillLevels = Enum.GetNames(typeof(SkillLevel)).Length;
            var dic = new Dictionary<SkillLevel, int>(skillLevels);
            var splitted = value.Split('-');
            if (splitted.Length == skillLevels)
            {
                for (int i = 0; i < skillLevels; i++)
                    dic.Add((SkillLevel)i, int.Parse(splitted[i]));
            }
            else
            {
                var sameRecovery = int.Parse(splitted[0]);
                for (int i = 0; i < skillLevels; i++)
                    dic.Add((SkillLevel)i, sameRecovery);
            }
            return dic;
        }

        #endregion
    }
}

