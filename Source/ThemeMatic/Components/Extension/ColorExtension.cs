using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ThemeMatic.Components.Extension
{
    public static class ColorExtension
    {
        public static Color GetTint(this Color self, double factor)
        {
            return Color.FromArgb(self.A, Convert.ToByte(self.R + (255 - self.R) * factor),
                                  Convert.ToByte(self.G + (255 - self.G) * factor),
                                  Convert.ToByte(self.B + (255 - self.B) * factor));
        }

        public static Color GetHue(this Color self, double factor)
        {
            return Color.FromArgb(self.A, Convert.ToByte(self.R * factor), Convert.ToByte(self.G * factor),
                                  Convert.ToByte(self.B * factor));
        }
    }
}
