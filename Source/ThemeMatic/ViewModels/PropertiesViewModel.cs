using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using ThemeMatic.Components.Extension;
using ThemeMatic.Model;

namespace ThemeMatic.ViewModels
{
    public class PropertiesViewModel : INotifyPropertyChanged
    {
        private readonly Design design;

        public PropertiesViewModel(Design design)
        {
            this.design = design;
        }

        public string OutputLocation
        {
            get { return design.OutputLocation; }
            set
            {
                if (design.OutputLocation == value) return;
                design.OutputLocation = value;
                this.Changed(() => OutputLocation, PropertyChanged);
            }
        }

        private bool settingThemeAssemblyName;

        public string ProjectName
        {
            get { return design.ProjectName; }
            set
            {
                if (design.ProjectName == value) return;
                design.ProjectName = value;
                this.Changed(() => ProjectName, PropertyChanged);
                if (string.IsNullOrEmpty(design.ThemeAssemblyName))
                {
                    settingThemeAssemblyName = true;
                }
                if (settingThemeAssemblyName)
                {
                    design.ThemeAssemblyName = design.ProjectName + "." + design.Theme.Name;
                    this.Changed(() => ThemeAssemblyName, PropertyChanged);
                }
            }
        }

        public string ThemeAssemblyName
        {
            get { return design.ThemeAssemblyName; }
            set
            {
                if (design.ThemeAssemblyName == value) return;
                design.ThemeAssemblyName = value;
                this.Changed(() => ThemeAssemblyName, PropertyChanged);
                settingThemeAssemblyName = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsOutputLocationValid()
        {
            return !string.IsNullOrEmpty(OutputLocation) && Directory.Exists(OutputLocation);
        }
    }
}
