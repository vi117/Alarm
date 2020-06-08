using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Alarm.ViewModels;
using CefSharp;
using CefSharp.Wpf;
using MahApps.Metro;

namespace Alarm
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Do not change.
        /// </summary>
        static private Setting setting;
        static private string[] accentList;
        static public string[] AccentList => accentList;
        static private string[] themeList;
        static public string[] ThemeList => themeList;
        static private string[] languageList;
        static public string[] LanguageList => languageList;
        static public Setting Setting => setting;

        public void ChangeLanguage(string newLang)
        {
            setting.Language = newLang;

            foreach (ResourceDictionary dict in Resources.MergedDictionaries)
            {
                if (dict is LanguageManager langDict)
                    langDict.UpdateSource(setting.Language);
                else
                    dict.Source = dict.Source;
            }
        }
        public void ChangeTheme(string accent, string appTheme)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                                                    ThemeManager.GetAccent(accent),
                                                    ThemeManager.GetAppTheme(appTheme));
        }
        private Task<Setting> initSettingTask;
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            InitializeCefSharp();
            WPFPlatform.Register();
            initSettingTask = LoadSetting();
        }
        private async Task<Setting> LoadSetting()
        {
            string path = Setting.DefaultPath;
            Setting s;
            if (File.Exists(path))
                s = await Setting.LoadAsync(path);
            else
                s = Setting.GetDefault();
            s.PropertyChanged += (sender, e) =>
            {
                var set = sender as Setting;
                switch (e.PropertyName)
                {
                    case nameof(set.Language):
                        ChangeLanguage(set.Language);
                        break;
                    case nameof(set.Accent):
                        ChangeTheme(set.Accent, set.AppTheme);
                        break;
                    case nameof(set.AppTheme):
                        ChangeTheme(set.Accent, set.AppTheme);
                        break;
                }
            };
            return s;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            //task.Start();
            //Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Current,
                                    ThemeManager.GetAccent("Green"),
                                    ThemeManager.GetAppTheme("BaseLight"));
            base.OnStartup(e);
            accentList = ThemeManager.Accents.Select(x => x.Name).ToArray();
            themeList = ThemeManager.AppThemes.Select(x => x.Name).ToArray();
            foreach (ResourceDictionary dict in Resources.MergedDictionaries)
            {
                if (dict is LanguageManager skinDict)
                    languageList = skinDict.LanguageDict.Keys.ToArray();
            }
            initSettingTask.Wait();
            setting = initSettingTask.Result;
            ChangeTheme(setting.Accent, setting.AppTheme);
        }
        public void InitializeCefSharp()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }
        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string architectureSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    assemblyName);

                return File.Exists(architectureSpecificPath)
                    ? Assembly.LoadFile(architectureSpecificPath)
                    : null;
            }

            return null;
        }
    }
}
