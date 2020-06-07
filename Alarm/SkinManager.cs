using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alarm
{
    public class SkinDictionary : Dictionary<string, Uri>
    {}
    public class SkinManager : ResourceDictionary
    {
        public SkinDictionary SkinDict { get; set; }

        public const string LightSkinName = "LightSkin";

        internal void UpdateSource(string skinType)
        {
            if (!SkinDict.ContainsKey(skinType)) throw new KeyNotFoundException("Skin Key not found.");
            var val = SkinDict[skinType];
            if (val != null && base.Source != val)
                base.Source = val;
        }
    }
}
