using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThemeMatic.Model
{
    // I'm going to call this the 'simple' project generator because it will do simple string.format() etc
    // to generate the project. In the future a more complicated T4 project generator or something like that
    // might come along
    public class SimpleProjectGenerator : IProjectGenerator
    {
        public void Generate(Design design)
        {
            // TODO - generate code here
        }
    }
}
