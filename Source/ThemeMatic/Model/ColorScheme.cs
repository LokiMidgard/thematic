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
            Primary = new DesignColor(Colors.Gray, "primary");
            Primary.ColorChanged += ColorChanged;
            Secondary = new DesignColor(Colors.Gray, "secondary");
            Secondary.ColorChanged += ColorChanged;

            // chrome colors
            Chrome = new DesignColor(Colors.Gray, "chrome");
            Chrome.ColorChanged += ColorChanged;
            ChromeAlternate = new DesignColor(Colors.Gray, "chromeAlternate");
            ChromeAlternate.ColorChanged += ColorChanged;
            Disabled = new DesignColor(Colors.Gray, "disabled");
            Disabled.ColorChanged += ColorChanged;

            // foreground and background
            Foreground = new DesignColor(Colors.Gray, "foreground");
            Foreground.ColorChanged += ColorChanged;
            Background = new DesignColor(Colors.Gray, "background");
            Background.ColorChanged += ColorChanged;
        }

        private void ColorChanged(object sender, EventArgs e)
        {
            if (theme != null && theme.BaseResourceDictionary != null)
            {
                var themeColor = sender as DesignColor;
                if (themeColor != null)
                {
                    themeColor.UpdateReferences(theme.BaseResourceDictionary);
                }
            }
        }

        public DesignColor Primary { get; private set; }
        public DesignColor Secondary { get; set; }
        public DesignColor Foreground { get; set; }
        public DesignColor Background { get; set; }
        public DesignColor Disabled { get; set; }
        public DesignColor Chrome { get; set; }
        public DesignColor ChromeAlternate { get; set; }


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
