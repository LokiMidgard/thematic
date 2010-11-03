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
        private readonly IMessagePresenter messagePresenter;
        private readonly IProjectGenerator generator;

        public ApplicationViewModel(Design design, List<Theme> allThemes, IMessagePresenter messagePresenter, IProjectGenerator generator)
        {
            if (allThemes == null || allThemes.Count == 0)
            {
                throw new ArgumentException("No themes were provided.");
            }

            this.design = design;
            this.messagePresenter = messagePresenter;
            this.generator = generator;
            design.ColorScheme = new ColorScheme();
            colorSelectionViewModel = new ColorSelectionViewModel(design.ColorScheme);

            design.Theme = allThemes[0];
            themeViewModel = new ThemeViewModel(design, allThemes);

            PreviewCommand = new DelegateCommand(PreviewExecute, CanPreviewExecute);
            GenerateCommand = new DelegateCommand(GenerateExecute);

            PropertiesViewModel = new PropertiesViewModel(this.design);
        }

        private void GenerateExecute(object obj)
        {
            if (this.PropertiesViewModel.IsOutputLocationValid() && !string.IsNullOrEmpty(design.ThemeAssemblyName) && !string.IsNullOrEmpty(design.ProjectName))
            {
                generator.Generate(design);
            }
            else
            {
                messagePresenter.ShowMessage("You must specify a valid output location for the generated code, a theme assembly name and a project name in the Properties tab.");
            }
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

        public PropertiesViewModel PropertiesViewModel { get; private set; }

        public ICommand PreviewCommand { get; private set; }
        public ICommand GenerateCommand { get; private set; }
    }
}
