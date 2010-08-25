using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ThemeMatic.Components.Extension
{
    public static class ColorExtension
    {

        // for getting lighter and darker colors
        public static Color GetTint(this Color self, double factor)
        {
            return Color.FromArgb(self.A, Convert.ToByte(self.R + (255 - self.R) * factor),
                                  Convert.ToByte(self.G + (255 - self.G) * factor),
                                  Convert.ToByte(self.B + (255 - self.B) * factor));
        }

        public static Color GetDarkerHue(this Color self, double factor)
        {
            return Color.FromArgb(self.A, Convert.ToByte(self.R * factor), Convert.ToByte(self.G * factor),
                                  Convert.ToByte(self.B * factor));
        }

        // returns the HUE
        public static Single GetHue(this Color self)
        {
            if (self.R == self.G && self.G == self.B)
                return 0; // Undefined - return 0 

            float r = self.R / 255.0f;
            float g = self.G / 255.0f;
            float b = self.B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue / 360;
        }

        // System.Drawing.Color calls this GetBrightness but it returns the HSL value, not HSV/B so I changed the name
        public static float GetLightness(this Color self)
        {
            float r = self.R / 255.0f;
            float g = self.G / 255.0f;
            float b = self.B / 255.0f;

            float max, min;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            return (max + min) / 2;
        }

        public static float GetSaturation(this Color self)
        {
            float r = self.R / 255.0f;
            float g = self.G / 255.0f;
            float b = self.B / 255.0f;

            float max, min;
            float l, saturation = 0;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            // if max == min, then there is no color and
            // the saturation is zero.
            if (max != min)
            {
                l = (max + min) / 2;

                if (l <= .5)
                {
                    saturation = (max - min) / (max + min);
                }
                else
                {
                    saturation = (max - min) / (2 - max - min);
                }
            }
            return saturation;
        }

        public static Color FromHSL(double hue, double saturation, double lightness)
        {
            double r = 0, g = 0, b = 0;

            if (lightness == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (saturation == 0)
                {
                    r = g = b = lightness;
                }
                else
                {
                    double temp2 = ((lightness <= 0.5) ? lightness * (1.0 + saturation) : lightness + saturation - (lightness * saturation));
                    double temp1 = 2.0 * lightness - temp2;

                    double[] t3 = new double[] { hue + 1.0 / 3.0, hue, hue - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                            t3[i] += 1.0;
                        if (t3[i] > 1)
                            t3[i] -= 1.0;

                        if (6.0 * t3[i] < 1.0)
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        else if (2.0 * t3[i] < 1.0)
                            clr[i] = temp2;
                        else if (3.0 * t3[i] < 2.0)
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                        else
                            clr[i] = temp1;
                    }
                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            return Color.FromRgb((byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }

    }
}
