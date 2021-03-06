﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ThemeMatic.Model;
using Xunit;

namespace ThemeMatic.Test.Model
{
    
    public class ColorUpdaterTest
    {
        [Fact]
        public void CanUpdatePrimaryColor()
        {
            var design = new Design() {ColorScheme = new ColorScheme()};
            design.ColorScheme.Primary.Base = Colors.Red;

            var updater = new ColorUpdater(design);
            var response = updater.UpdateThemeColors(TestResource);
            Assert.True(response.Contains(@"<Color x:Key=""primaryBase"" R=""255"""));
        }


        private const string TestResource =
            @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">

    <!-- branding -->
    <Color x:Key=""primaryBase"" />
    <Color x:Key=""primaryLightAccent1"" />
    <Color x:Key=""primaryLightAccent2""  />
    <Color x:Key=""primaryDarkAccent1""  />
    <Color x:Key=""primaryDarkAccent2""  />
    <Color x:Key=""secondaryBase"" />
    <Color x:Key=""secondaryLightAccent1"" />
    <Color x:Key=""secondaryLightAccent2""  />
    <Color x:Key=""secondaryDarkAccent1""  />
    <Color x:Key=""secondaryDarkAccent2""  />

    <!-- control chrome -->
    <Color x:Key=""chromeBase"" />
    <Color x:Key=""chromeLightAccent1"" />
    <Color x:Key=""chromeLightAccent2""  />
    <Color x:Key=""chromeDarkAccent1""  />
    <Color x:Key=""chromeDarkAccent2""  />
    <Color x:Key=""chromeAlternateBase""  />
    <Color x:Key=""chromeAlternateLightAccent1""  />
    <Color x:Key=""chromeAlternateLightAccent2""  />
    <Color x:Key=""chromeAlternateDarkAccent1""  />
    <Color x:Key=""chromeAlternateDarkAccent2""  />
    <Color x:Key=""disabledBase""  />
    <Color x:Key=""disabledLightAccent1""  />
    <Color x:Key=""disabledLightAccent2""  />
    <Color x:Key=""disabledDarkAccent1""  />
    <Color x:Key=""disabledDarkAccent2""  />

    <Color x:Key=""foregroundBase""  />
    <Color x:Key=""foregroundLightAccent1""  />
    <Color x:Key=""foregroundLightAccent2""  />
    <Color x:Key=""foregroundDarkAccent1""  />
    <Color x:Key=""foregroundDarkAccent2""  />
    <Color x:Key=""backgroundBase""  />
    <Color x:Key=""backgroundLightAccent1""  />
    <Color x:Key=""backgroundLightAccent2""  />
    <Color x:Key=""backgroundDarkAccent1""  />
    <Color x:Key=""backgroundDarkAccent2""  />
    </ResourceDictionary>";
    }
}
