using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace TestClientApplication
{{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {{
        protected override void OnStartup(StartupEventArgs e)
        {{
            base.OnStartup(e);
            this.Resources.MergedDictionaries.Clear();
            string packUri = @"/{0};component/Resources/Themes/Wpf/{1}.xaml";
            var resourceDictionary = Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
            this.Resources.MergedDictionaries.Add(resourceDictionary);
        }}
    }}
}}
