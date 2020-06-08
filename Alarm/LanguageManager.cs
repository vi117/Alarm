using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alarm
{
    public class LanguageDictionary : Dictionary<string, Uri>
    {}
    public class LanguageManager : ResourceDictionary
    {
        public LanguageDictionary LanguageDict { get; set; }

        public const string English = "LightSkin";

        internal void UpdateSource(string skinType)
        {
            if (!LanguageDict.ContainsKey(skinType)) throw new KeyNotFoundException("Skin Key not found.");
            var val = LanguageDict[skinType];
            if (val != null && base.Source != val)
                base.Source = val;
        }
    }
}
