using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using ThemeMatic.Components.Extension;

namespace ThemeMatic.Model
{
    public class ColorScheme: INotifyPropertyChanged
    {
        public ColorScheme()
        {
            // branding colors
            Primary = new ThemeColor(Colors.Gray, "primary");
            Primary.ColorChanged += ColorChanged;
            Secondary = new ThemeColor(Colors.Gray, "secondary");
            Secondary.ColorChanged += ColorChanged;

            // chrome colors
            Chrome = new ThemeColor(Colors.Gray, "chrome");
            Chrome.ColorChanged += ColorChanged;
            ChromeAlternate = new ThemeColor(Colors.Gray, "chromeAlternate");
            ChromeAlternate.ColorChanged += ColorChanged;
            Disabled = new ThemeColor(Colors.Gray, "disabled");
            Disabled.ColorChanged += ColorChanged;

            // foreground and background
            Foreground = new ThemeColor(Colors.Gray, "foreground");
            Foreground.ColorChanged += ColorChanged;
            Background = new ThemeColor(Colors.Gray, "background");
            Background.ColorChanged += ColorChanged;
        }

        private void ColorChanged(object sender, EventArgs e)
        {
            if (theme != null && theme.BaseResourceDictionary != null)
            {
                var themeColor = sender as ThemeColor;
                if (themeColor != null)
                {
                    themeColor.UpdateReferences(theme.BaseResourceDictionary);
                }
            }
        }

        public ThemeColor Primary { get; private set; }
        public ThemeColor Secondary { get; set; }
        public ThemeColor Foreground { get; set; }
        public ThemeColor Background { get; set; }
        public ThemeColor Disabled { get; set; }
        public ThemeColor Chrome { get; set; }
        public ThemeColor ChromeAlternate { get; set; }


        private Theme theme;

        public Theme Theme
        {
            get { return theme; }
            set
            {
                if (theme == value) return;
                theme = value;
                this.Changed(() => Theme, PropertyChanged);
                UpdateDictionary();
            }
        }

        private void UpdateDictionary()
        {
            Primary.UpdateReferences(theme.BaseResourceDictionary);
            Secondary.UpdateReferences(theme.BaseResourceDictionary);
            Foreground.UpdateReferences(theme.BaseResourceDictionary);
            Background.UpdateReferences(theme.BaseResourceDictionary);
            Disabled.UpdateReferences(theme.BaseResourceDictionary);
            Chrome.UpdateReferences(theme.BaseResourceDictionary);
            ChromeAlternate.UpdateReferences(theme.BaseResourceDictionary);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
