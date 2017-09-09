using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Business
{
    public class ItemsParser : TxtParser<Item>
    {
        public ItemsParser(string filepath) : base(filepath) {
        }
        
        #region implemented abstract members of TxtParser

        public override Item ParseValues(string[] values)
        {
            try
            {
                var item = new Item();
                item.Code = int.Parse(values[0]);
                item.PictureFilename = values[1];
                item.Name = values[2];
                item.Value = int.Parse(values[3]);
                item.EquipSlot = values[4].ToEnum<EquipSlot>(EquipSlot.None);
                item.SkillGroup = values[5].ToEnum<SkillCode>(SkillCode.None);
                item.Mod1 = values[6];
                item.Mod2 = int.Parse(values[7]);
                //TODO: material??
                item.IdItemRequiredLevel = int.Parse(values[9]);
                item.NotIdentifiedName = values[10];
                item.EquipX = float.Parse(values[14]);
                item.EquipY = float.Parse(values[15]);
                item.RandomItemGenerationChanceByTreasureLevel = new int[6];
                for (int i=0; i<6; i++)
                    int.TryParse(values[16 + i], out item.RandomItemGenerationChanceByTreasureLevel[i]);
                item.Description = values[22];//.TrimEnd('\r');
                return item;
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

