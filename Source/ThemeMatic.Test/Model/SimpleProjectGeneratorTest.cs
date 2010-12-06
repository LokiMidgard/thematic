using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ThemeMatic.Model;
using Xunit;

namespace ThemeMatic.Test.Model
{
    public class SimpleProjectGeneratorTest
    {
        [Fact]
        public void NewProjectTextCanBeGeneratedFromDesign()
        {
            var simpleProjectGenerator = new SimpleProjectGenerator(Path.GetTempPath());
            var newProjectContent = simpleProjectGenerator.GenerateNewProjectFileContents(null, new List<string>(){"..\\foo\\bar\\test.xaml"});
            Assert.True(!string.IsNullOrEmpty(newProjectContent));
        }
    }
}
