using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
    public class Localization
    {
        private static Localization instance;

        public static Localization Instance {
            get {
                if (instance == null){
                    instance = new Localization();
                }
                return instance;
            }
        }

        private static Dictionary<string, string> LocalizedStrings;

        public Localization()
        {
            LocalizedStrings = new Dictionary<string, string>();
            var strings = Resources.Load("Data/LocalizedStrings") as TextAsset;
            var lines = strings.text.Split('\n');
            foreach (var line in lines)
            {
                string[] values = line.Split('\t');
                LocalizedStrings[values[0]] = values[1];
            }

        }

        public string Get(string localizationKey) {
            if (LocalizedStrings.ContainsKey(localizationKey))
                return LocalizedStrings[localizationKey];
//            Debug.Log("Localization key not found:" + localizationKey);
            return localizationKey.ToSentenceCase();
        }

        public string Get(string localizationKey, params object [] parameters) {
            return string.Format(Get(localizationKey), parameters);
        }
    }
}

