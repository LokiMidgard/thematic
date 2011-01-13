using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
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
            
        }

        private bool sliderTriggeringChange = false;
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
                double hue = colorWheelColor.GetHue();
                double lightness = selectedColor.Base.GetLightness();
                sliderTriggeringChange = true;
                selectedColor.Base = ColorExtension.FromHSL(hue, value, lightness);
                sliderTriggeringChange = false;
            }
        }


        public double SelectedColorLightness
        {
            get
            {
                var lightness = selectedColor.Base.GetLightness();
                System.Diagnostics.Debug.WriteLine("lightness: " + lightness.ToString());
                return lightness;
            }
            set
            {
                double lightness = selectedColor.Base.GetSaturation();
                if (lightness == value) return;
                double hue = colorWheelColor.GetHue();
                double saturation = selectedColor.Base.GetSaturation();
                sliderTriggeringChange = true;
                selectedColor.Base = ColorExtension.FromHSL(hue, saturation, value);
                sliderTriggeringChange = false;
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
                colorWheelColor = value.Base;
                if (selectedColor != null)
                {
                    selectedColor.ColorChanged += SelectedColorChanged;                    
                }
                this.Changed(() => SelectedColor, PropertyChanged);
                this.Changed(() => SelectedColorSaturation, PropertyChanged);
                this.Changed(() => SelectedColorLightness, PropertyChanged);
                var hue = selectedColor.Base.GetHue();
                UpdateSaturationFill(hue);
                UpdateLightnessFill(hue);
            }
        }

        private Color colorWheelColor;
        public Color ColorWheelColor
        {
            get { return colorWheelColor; }
            set 
            { 
                colorWheelColor = value;
                var hue = colorWheelColor.GetHue();
                SelectedColor.Base = ColorExtension.FromHSL(hue, SelectedColorSaturation, SelectedColorLightness);
                UpdateSaturationFill(hue);
                UpdateLightnessFill(hue);
            }
        }

        private void UpdateSaturationFill(double hue)
        {
            var satBrush = new LinearGradientBrush();
            satBrush.GradientStops.Add(new GradientStop(ColorExtension.FromHSL(hue, 0, SelectedColorLightness), 0));
            satBrush.GradientStops.Add(new GradientStop(ColorExtension.FromHSL(hue, 0.5, SelectedColorLightness), 0.5));
            satBrush.GradientStops.Add(new GradientStop(ColorExtension.FromHSL(hue, 1, SelectedColorLightness), 1));
            SaturationFill = satBrush;
        }

        private void UpdateLightnessFill(double hue)
        {
            var lightnessBrush = new LinearGradientBrush();
            lightnessBrush.GradientStops.Add(new GradientStop(ColorExtension.FromHSL(hue, SelectedColorSaturation, 0), 0));
            lightnessBrush.GradientStops.Add(new GradientStop(ColorExtension.FromHSL(hue, SelectedColorSaturation, 0.5), 0.5));
            lightnessBrush.GradientStops.Add(new GradientStop(ColorExtension.FromHSL(hue, SelectedColorSaturation, 1), 1));
            LightnessFill = lightnessBrush;
        }

        private LinearGradientBrush lightnessFill;
        public LinearGradientBrush LightnessFill
        {
            get { return lightnessFill; }
            set
            {
                if (lightnessFill == value) return;
                lightnessFill = value;
                this.Changed(() => LightnessFill, PropertyChanged);
            }
        }

        private LinearGradientBrush saturationFill;
        public LinearGradientBrush SaturationFill
        {
            get { return saturationFill; }
            set
            {
                if (saturationFill == value) return;
                saturationFill = value;
                this.Changed(() => SaturationFill, PropertyChanged);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SelectColorCommand { get; private set; }
    }
}
