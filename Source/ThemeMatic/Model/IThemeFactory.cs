using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThemeMatic.Model
{
    public interface IThemeFactory
    {
        List<Theme> GetAllThemes();
    }
}
