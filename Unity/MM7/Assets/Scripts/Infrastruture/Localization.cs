using System;

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
        
        public Localization()
        {
        }

        public string Get(string localizationKey) {
            return localizationKey.ToSentenceCase();
        }
    }
}

