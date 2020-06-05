using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alarm
{
    public class SkinDictionary : ResourceDictionary
    {
        private Uri lightSkinSource;
        private Uri darkSkinSource;

        public Uri LightSkinSource
        {
            get => lightSkinSource;
            set
            {
                lightSkinSource = value;
                UpdateSource();
            }
        }
        public Uri DarkSkinSource
        {
            get => darkSkinSource;
            set
            {
                darkSkinSource = value;
                UpdateSource();
            }
        }

        internal void UpdateSource()
        {
            var val = App.Skin == SkinType.Light ? LightSkinSource : DarkSkinSource;
            if (val != null && base.Source != val)
                base.Source = val;
        }
    }
}
