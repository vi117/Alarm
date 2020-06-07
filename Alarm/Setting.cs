using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;

namespace Alarm
{
    public class Setting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string skinType = SkinManager.LightSkinName;

        public string SkinType {
            get => skinType; 
            set {
                if(value != skinType)
                {
                    skinType = value;
                    InvokeChangeEvent(nameof(SkinType));
                }
            }
        }

        public void InvokeChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Save(string path)
        {
            string jsonStr = JsonSerializer.Serialize(this);
            File.WriteAllText(path, jsonStr);
        }
        static public Setting GetDefault() {
            return new Setting() {
                SkinType = SkinManager.LightSkinName
            };
        }
        static public Setting Load(string path)
        {
            string jsonStr = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Setting>(jsonStr);
        }
    }
}
