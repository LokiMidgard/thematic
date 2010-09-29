using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ThemeMatic.Components;
using ThemeMatic.Model;

namespace ThemeMatic.ViewModels
{
    public class ApplicationViewModel
    {
        private readonly Design design;

        public ApplicationViewModel(Design design, List<Theme> allThemes)
        {
            if (allThemes == null || allThemes.Count == 0)
            {
                throw new ArgumentException("No themes were provided.");
            }

            this.design = design;
            design.ColorScheme = new ColorScheme();
            colorSelectionViewModel = new ColorSelectionViewModel(design.ColorScheme);

            design.Theme = allThemes[0];
            themeViewModel = new ThemeViewModel(design, allThemes);

            PreviewCommand = new DelegateCommand(PreviewExecute, CanPreviewExecute);
        }

        private void PreviewExecute(object obj)
        {
            var previewWindow = new PreviewWindow();
            previewWindow.Resources.Clear();
            previewWindow.Resources.MergedDictionaries.Add(design.Theme.BaseResourceDictionary);
            previewWindow.Show();
        }

        private bool CanPreviewExecute()
        {
            return design.Theme != null && design.Theme.BaseResourceDictionary != null;
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

        public ICommand PreviewCommand { get; private set; }
    }
}
