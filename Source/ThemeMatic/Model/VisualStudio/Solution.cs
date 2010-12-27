using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NDepend.Helpers.FileDirectoryPath;

namespace ThemeMatic.Model.VisualStudio
{
    public class Solution
    {
        public Solution()
        {
            Projects = new List<Project>();
            UniqueIdentifier = Guid.NewGuid();
        }

        public Guid UniqueIdentifier { get; private set; }

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

        public string FilePath
        {
            get { return Path.Combine(Directory, Name + ".sln"); }
        }

        public List<Project> Projects { get; private set; }

        private const string projectSectionInSlnFormatString =
    @"Project(""{{{0}}}"") = ""{1}"", ""{2}"", ""{{{3}}}""
EndProject
";

        private const string projectBuildConfigurationsInSlnFormatString =
            @"{{{0}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{{0}}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{{0}}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{{0}}}.Release|Any CPU.Build.0 = Release|Any CPU";

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
            if (!File.Exists(FilePath))
            {
                var absolutionSolutionPath = new DirectoryPathAbsolute(FilePath);
                string headerString = "";
                string buildSection = "";
                foreach (var project in Projects)
                {
                    var absoluteProjectPath = new DirectoryPathAbsolute(project.FilePath);
                    headerString += string.Format(projectSectionInSlnFormatString, this.UniqueIdentifier, project.Name, absoluteProjectPath.GetPathRelativeFrom(absolutionSolutionPath).Path.Remove(0, 3) /* removes the leading ..\ */, project.UniqueIdentifier.ToString().ToUpper());
                    buildSection += string.Format(projectBuildConfigurationsInSlnFormatString, project.UniqueIdentifier.ToString().ToUpper());
                }

                using (var fileStream = new StreamWriter(FilePath))
                {
                    string slnContents = string.Format(solutionFormatString, headerString, buildSection);
                    fileStream.Write(slnContents);
                    fileStream.Flush();
                }
            }
            foreach (var project in Projects)
            {
                project.Generate();
            }
        }
    }
}
