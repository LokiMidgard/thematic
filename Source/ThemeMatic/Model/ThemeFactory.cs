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
            themes.Add(new Theme() { Description = "A simple, clean control theme originally developed by Robbie Ingebretsen", Name = "SimpleStyles", BaseResourceDictionaryUrl = "/Thematic;component/Resources/Themes/WPF/SimpleStyles.xaml", PreviewImage = new BitmapImage(new Uri("/Themematic;component/Resources/Themes/WPF/SimpleStyles/preview.png", UriKind.Relative))});
            themes.Add(new Theme() { Description = "A modern theme inspired by the Zune and Windows Phone 7 UI. Originally released by Microsoft for Silverlight.", Name = "Cosmopolitan", BaseResourceDictionaryUrl = "/Thematic;component/Resources/Themes/WPF/Cosmopolitan.xaml", PreviewImage = new BitmapImage(new Uri("/Themematic;component/Resources/Themes/WPF/Cosmopolitan/preview.png", UriKind.Relative)) });
            themes.Add(new Theme() { Description = "A modern theme for business applications. Originally released by Microsoft for Silverlight.", Name = "JetPack", BaseResourceDictionaryUrl = "/TheMatic;component/Resources/Themes/WPF/JetPack.xaml", PreviewImage = new BitmapImage((new Uri("/Themematic;component/Resources/Themes/WPF/JetPack/preview.png", UriKind.Relative)))});
            return themes;
        }
    }
}
