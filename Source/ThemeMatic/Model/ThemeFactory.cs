using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace ThemeMatic.Model
{
    public class ThemeFactory : IThemeFactory
    {
        public List<Theme> GetAllThemes()
        {
            var themes = new List<Theme>();
            themes.Add(new Theme() { Description = "A simple, clean control theme originally developed by Robbie Ingebretsen", Name = "Simple", BaseResourceDictionaryUrl = "/Themematic;component/Resources/Themes/WPF/SimpleStyles.xaml", PreviewImage = new BitmapImage(new Uri("/Themematic;component/Resources/Themes/WPF/SimpleStyles/preview.png", UriKind.Relative))});
            return themes;
        }
    }
}
