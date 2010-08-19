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

        public ApplicationViewModel(Design design)
        {
            this.design = design;
            colorSelectionViewModel = new ColorSelectionViewModel(design.ColorScheme);
        }

        private ColorSelectionViewModel colorSelectionViewModel;
        public ColorSelectionViewModel ColorSelectionViewModel
        {
            get { return colorSelectionViewModel; }
        }
    }
}
