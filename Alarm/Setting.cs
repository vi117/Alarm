using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Papago;

namespace Alarm
{
    public class Setting : INotifyPropertyChanged
    {
        static public readonly string DefaultPath = "setting.json";
        public event PropertyChangedEventHandler PropertyChanged;

        private string language = LanguageManager.English;
        private string appTheme = "BaseLight";
        private string accent = "Green";
        private Papago.ApiPass pass;
        public string Language
        {
            get => language;
            set
            {
                if (value != language)
                {
                    language = value;
                    InvokeChangeEvent(nameof(Language));
                }
            }
        }

        public string AppTheme
        {
            get => appTheme;
            set
            {
                if (value != appTheme)
                {
                    appTheme = value;
                    InvokeChangeEvent(nameof(AppTheme));
                }
            }
        }
        public string Accent
        {
            get => accent;
            set
            {
                if (value != accent)
                {
                    accent = value;
                    InvokeChangeEvent(nameof(Accent));
                }
            }
        }
        public string PapagoApiID { 
            get => pass?.Id;
            set
            {
                if(pass == null)
                {
                    pass = new Papago.ApiPass();
                }
                pass.Id = value;
                InvokeChangeEvent(nameof(PapagoApiID));
            }
        }
        public string PapagoApiSecret {
            get => pass?.Secret;
            set
            {
                if (pass == null)
                {
                    pass = new Papago.ApiPass();
                }
                pass.Secret = value;
                InvokeChangeEvent(nameof(PapagoApiSecret));
            }
        }
        [JsonIgnore]
        public ApiPass PapagoApiPass
        {
            get => pass;
            set => pass = value;
        }
        public void InvokeChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void CopyTo(Setting dest)
        {
            var propertyInfos = typeof(Setting).GetProperties(
                System.Reflection.BindingFlags.Instance|
                System.Reflection.BindingFlags.Public|
                System.Reflection.BindingFlags.SetProperty|
                System.Reflection.BindingFlags.GetProperty);
            foreach (var item in propertyInfos)
            {
                item.SetValue(dest, item.GetValue(this));
            }
        }
        static public Setting GetDefault()
        {
            return new Setting()
            {
                Language = LanguageManager.English,
                Accent = "Blue",
                AppTheme = "BaseLight",
            };
        }
        public void Save(string path)
        {
            var option = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            var jsonStr = JsonSerializer.SerializeToUtf8Bytes(this,option);
            File.WriteAllBytes(path, jsonStr);
        }
        static public Setting Load(string path)
        {
            string jsonStr = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Setting>(jsonStr);
        }
        static public async Task<Setting> LoadAsync(string path) {
            using (var file = new FileStream(path,FileMode.Open)) {
                return await JsonSerializer.DeserializeAsync<Setting>(file);
            }
        }
    }
}
