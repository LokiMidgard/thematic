using System;
using System.ComponentModel;
using System.Windows.Input;
using ThemeMatic.Components;
using ThemeMatic.Components.Extension;
using ThemeMatic.Model;

namespace ThemeMatic.ViewModels
{
    public class ColorSelectionViewModel : INotifyPropertyChanged
    {
        private readonly ColorScheme colorScheme;

        public ColorSelectionViewModel(ColorScheme colorScheme)
        {
            this.colorScheme = colorScheme;
            SelectedColor = colorScheme.Primary;
            SelectColorCommand = new DelegateCommand(SelectColorExecute);
        }

        private void SelectColorExecute(object obj)
        {
            var color = obj as DesignColor;
            if (color != null)
            {
                this.SelectedColor = color;
            }
        }

        void SelectedColorChanged(object sender, EventArgs e)
        {
            this.Changed(() => SelectedColorSaturation, PropertyChanged);
        }

        public double SelectedColorSaturation
        {
            get
            {
                var saturation = selectedColor.Base.GetSaturation();
                System.Diagnostics.Debug.WriteLine("saturation: " + saturation.ToString());
                return saturation; 
            
            }
            set 
            { 
                double saturation = selectedColor.Base.GetSaturation();
                if (saturation == value) return;
                double hue = selectedColor.Base.GetHue();
                double lightness = selectedColor.Base.GetLightness();
                selectedColor.Base = ColorExtension.FromHSL(hue, value, lightness);
            }
        }

        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
        }

        private DesignColor selectedColor;
        public DesignColor SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (selectedColor != null)
                {
                    selectedColor.ColorChanged -= SelectedColorChanged;
                }
                selectedColor = value;
                if (selectedColor != null)
                {
                    selectedColor.ColorChanged += SelectedColorChanged;                    
                }
                this.Changed(() => SelectedColor, PropertyChanged);
                this.Changed(() => SelectedColorSaturation, PropertyChanged);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SelectColorCommand { get; private set; }
    }
}
