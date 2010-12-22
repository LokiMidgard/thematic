using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThemeMatic.Model.VisualStudio
{
    public class Solution
    {
        public Solution()
        {
            Projects = new List<Project>();
        }

        public string Name { get; set; }
        private string directory;
        public string Directory
        {
            get { return directory; }
            set
            {
                if (!System.IO.Directory.Exists(value))
                {
                    System.IO.Directory.CreateDirectory(value);
                }
                directory = value;
            }
        }

        public List<Project> Projects { get; private set; }

        private const string projectSectionInSlnFormatString =
    @"Project(""{{{0}}}"") = ""{1}"", ""{2}"", ""{{{3}}}""
EndProject";

        private const string projectBuildConfigurationsInSlnFormatString =
            @"{{0}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{0}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{0}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{0}}.Release|Any CPU.Build.0 = Release|Any CPU";

        private const string solutionFormatString = @"Microsoft Visual Studio Solution File, Format Version 11.00
# Visual Studio 2010
{0}
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{1}
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal";

        public void Generate()
        {
            // TODO - generate solution
            foreach (var project in Projects)
            {
                project.Generate();
            }
        }
    }
}
