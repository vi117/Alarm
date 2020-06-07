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
        static private string[] skinList;
        static public string[] SkinList => skinList;
        static public Setting Setting => setting;
        public void ChangeSkin(string newSkin)
        {
            setting.SkinType = newSkin;

            foreach (ResourceDictionary dict in Resources.MergedDictionaries)
            {
                if (dict is SkinManager skinDict)
                    skinDict.UpdateSource(setting.SkinType);
                else
                    dict.Source = dict.Source;
            }
        }

        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            InitializeCefSharp();
            WPFPlatform.Register();
            setting = Setting.GetDefault();
            setting.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(setting.SkinType))
                {
                    ChangeSkin((s as Setting).SkinType);
                }
            };
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            foreach (ResourceDictionary dict in Resources.MergedDictionaries)
            {
                if (dict is SkinManager skinDict)
                    skinList = skinDict.SkinDict.Keys.ToArray();
            }
        }
        public void InitializeCefSharp(){
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
