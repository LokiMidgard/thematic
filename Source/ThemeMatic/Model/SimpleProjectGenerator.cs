using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Markup;
using NDepend.Helpers.FileDirectoryPath;
using ThemeMatic.Model.VisualStudio;

namespace ThemeMatic.Model
{
    // I'm going to call this the 'simple' project generator because it will do simple string.format() etc
    // to generate the project. In the future a more complicated T4 project generator or something like that
    // might come along
    public class SimpleProjectGenerator : IProjectGenerator
    {
        private readonly string resourceRootPath;
        private const string relativePathFromCurrentExecutableToWpfThemes = @"Resources\Themes\Wpf\";
        private const string relativePathFromCurrentExecutableToSampleUiFolder = @"Resources\UIStarterProject";
        private Guid themeProjectGuid;
        private Guid applicationUiProjectGuid;

        public SimpleProjectGenerator(string resourceRootPath)
        {
            if (String.IsNullOrEmpty(resourceRootPath))
            {
                throw  new ArgumentException("resource root path was not correctly specified");
            }
            if (!Directory.Exists(resourceRootPath))
            {
                throw new ArgumentException("resource root path does not exist");
            }
            if (!Directory.Exists(relativePathFromCurrentExecutableToWpfThemes))
            {
                throw new DirectoryNotFoundException("WPF themes required to generate this project could not be found.");
            }
            if (!Directory.Exists(relativePathFromCurrentExecutableToSampleUiFolder))
            {
                throw new DirectoryNotFoundException("Sample project required to generate themes could not be found.");
            }

            this.resourceRootPath = resourceRootPath;
        }

        public void Generate(Design design)
        {
            if (design == null)
            {
                throw new ArgumentNullException("design");
            }
            var sln = new Solution() {Directory = design.OutputLocation, Name = design.ProjectName};
            var themeProject = new Project() {ProjectType = ProjectType.Library, Name = design.ThemeAssemblyName, Directory = Path.Combine(design.OutputLocation, design.ThemeAssemblyName)};
            sln.Projects.Add(themeProject);
            var sampleApplication = new Project() { ProjectType = ProjectType.WinExe, Name = design.ProjectName, Directory = Path.Combine(design.OutputLocation, design.ProjectName)};
            sln.Projects.Add(sampleApplication);

            AddFilesToThemeProject(themeProject, design);
            AddFilesToSampleApplication(sampleApplication);

            sln.Generate();
        }

        private void AddFilesToSampleApplication(Project sampleApplication)
        {
            var sampleAppSourcePath = new DirectoryInfo(relativePathFromCurrentExecutableToSampleUiFolder);
            var absoluteSampleApplicationRoot = new DirectoryPathAbsolute(sampleAppSourcePath.FullName);
            AddAllFilesInPathToProject(absoluteSampleApplicationRoot, absoluteSampleApplicationRoot, sampleApplication);
        }

        private void AddFilesToThemeProject(Project themeProject, Design design)
        {
            var absoluteThemeFileRoot = new DirectoryPathAbsolute(AppDomain.CurrentDomain.BaseDirectory);
            var startThemeCopyDirectory =
                new DirectoryInfo(Path.Combine(relativePathFromCurrentExecutableToWpfThemes, design.Theme.Name));
            AddAllFilesInPathToProject(absoluteThemeFileRoot, new DirectoryPathAbsolute(startThemeCopyDirectory.FullName), themeProject);
            string baseResourceDictionaryName = Path.Combine(relativePathFromCurrentExecutableToWpfThemes, design.Theme.Name + ".xaml");
            themeProject.AddFile(new FileInfo(baseResourceDictionaryName).FullName, ".\\" + baseResourceDictionaryName);
        }

        private void AddAllFilesInPathToProject(DirectoryPathAbsolute absoluteSampleApplicationRoot, DirectoryPathAbsolute currentFolder, Project project)
        {
            System.Diagnostics.Debug.WriteLine("Copying contents of folder " + currentFolder.ToString());
            foreach (var file in currentFolder.ChildrenFilesPath)
            {
                project.AddFile(file.FileInfo.FullName, file.GetPathRelativeFrom(absoluteSampleApplicationRoot).Path);
            }
            foreach (var subdirectory in currentFolder.ChildrenDirectoriesPath)
            {
                AddAllFilesInPathToProject(absoluteSampleApplicationRoot, subdirectory, project);
            }
        }
    }
}
