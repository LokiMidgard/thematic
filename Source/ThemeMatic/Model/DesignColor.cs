using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ThemeMatic.Components.Extension;

namespace ThemeMatic.Model
{
    public class DesignColor : INotifyPropertyChanged
    {
        private readonly string name;

        public DesignColor(Color baseColor, string name)
        {
            this.name = name;
            Base = baseColor;
        }

        public Color LightAccent2 { get; private set; }
        public Color LightAccent1 { get; private set; }
        public Color DarkAccent1 { get; private set; }
        public Color DarkAccent2 { get; private set; }

        private Color baseColor;
        public Color Base
        {
            get { return baseColor; }
            set
            {
                if (baseColor != value)
                {
                    baseColor = value;
                    this.Changed(() => Base, PropertyChanged);
                    LightAccent2 = baseColor.GetTint(0.7);
                    this.Changed(() => LightAccent2, PropertyChanged);
                    LightAccent1 = baseColor.GetTint(0.45);
                    this.Changed(() => LightAccent1, PropertyChanged);
                    this.DarkAccent2 = baseColor.GetHue(0.4);
                    this.Changed(() => DarkAccent2, PropertyChanged);
                    this.DarkAccent1 = baseColor.GetHue(0.8);
                    this.Changed(() => DarkAccent1, PropertyChanged);
                    if (this.ColorChanged != null)
                    {
                        ColorChanged(this, new EventArgs());
                    }
                }
            }

        }


        public void UpdateReferences(ResourceDictionary dictionary)
        {
            UpdateItem(dictionary, name + "Base", baseColor);
            UpdateItem(dictionary, name + "LightAccent1", LightAccent1);
            UpdateItem(dictionary, name + "LightAccent2", LightAccent2);
            UpdateItem(dictionary, name + "DarkAccent1", DarkAccent1);
            UpdateItem(dictionary, name + "DarkAccent2", DarkAccent2);

        }

        private void UpdateItem(ResourceDictionary dictionary, string key, Color color)
        {
            dictionary[key] = color;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ColorChanged;
    }    
}
