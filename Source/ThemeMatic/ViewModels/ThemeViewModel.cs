using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ThemeMatic.Components.Extension;
using ThemeMatic.Model;

namespace ThemeMatic.ViewModels
{
    public class ThemeViewModel : INotifyPropertyChanged
    {
        private readonly Design design;
        private readonly IThemeFactory themeFactory;

        public ThemeViewModel(Design design, List<Theme> allThemes)
        {
            this.design = design;
            this.allThemes = allThemes;
        }

        private List<Theme> allThemes;
        public List<Theme> AllThemes
        {
            get { return allThemes; }
        }

        public Theme Theme
        {
            get { return design.Theme; }
            set
            {
                if (value == design.Theme) return;
                design.Theme = value;
                this.Changed(() => Theme, PropertyChanged);
            }
        }   
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
