using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThemeMatic.Model;

namespace ThemeMatic.ViewModels
{
    public class ApplicationViewModel
    {
        private readonly Design design;

        public ApplicationViewModel(Design design, List<Theme> allThemes)
        {
            this.design = design;
            colorSelectionViewModel = new ColorSelectionViewModel(design.ColorScheme);
            themeViewModel = new ThemeViewModel(design, allThemes);
        }

        private ColorSelectionViewModel colorSelectionViewModel;
        public ColorSelectionViewModel ColorSelectionViewModel
        {
            get { return colorSelectionViewModel; }
        }

        private ThemeViewModel themeViewModel;
        public ThemeViewModel ThemeViewModel
        {
            get { return themeViewModel; }
        }
    }
}
