using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThemeMatic.Model
{
    public interface IProjectGenerator
    {
        void Generate(Design design);
    }
}
