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

        public const string English = "English";

        internal void UpdateSource(string languageType)
        {
            if (!LanguageDict.ContainsKey(languageType)) throw new KeyNotFoundException("Language Key not found.");
            var val = LanguageDict[languageType];
            if (val != null && base.Source != val)
                base.Source = val;
        }
    }
}
