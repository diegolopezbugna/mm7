using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Business
{
    public abstract class TxtParser<T>
    {
        public List<T> Entities = new List<T>();

        public TxtParser(string resourcesPath) {
            var textAsset = Resources.Load(resourcesPath) as TextAsset;
            var lines = textAsset.text.Split('\n');
            foreach (var line in lines)
            {
                string[] values = line.Split('\t');
                var entity = ParseValues(values);
                if (entity != null)
                    Entities.Add(entity);
            }
        }

        public abstract T ParseValues(string[] values);
    }
}

