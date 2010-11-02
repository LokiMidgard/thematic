using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ThemeMatic.Components.Extension;

namespace ThemeMatic.Model
{
    public class Design : INotifyPropertyChanged
    {
        private ColorScheme colorScheme;
        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
            set
            {
                if (colorScheme == value) return;
                colorScheme = value;
                this.Changed(() => ColorScheme, PropertyChanged);
            }
        }

        private Theme theme;
        public Theme Theme
        {
            get { return theme; }
            set
            {
                if (theme == value) return;
                theme = value;
                this.Changed(() => Theme, PropertyChanged);
                if (colorScheme != null)
                {
                    colorScheme.Theme = theme;
                }
            }
        }

        private string outputLocation;
        public string OutputLocation
        {
            get { return outputLocation; }
            set
            {
                if (outputLocation == value) return;
                outputLocation = value;
                this.Changed(() => OutputLocation, PropertyChanged);
            }
        }

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                if (projectName == value) return;
                projectName = value;
                this.Changed(() => ProjectName, PropertyChanged);
            }
        }

        private string themeAssemblyName;
        public string ThemeAssemblyName
        {
            get { return themeAssemblyName; }
            set
            {
                if (themeAssemblyName == value) return;
                themeAssemblyName = value;
                this.Changed(() => ThemeAssemblyName, PropertyChanged);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
