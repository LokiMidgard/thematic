using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
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
        private Design design;

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
            
            // The order that the items are added to the solution affects which one it chooses as the startup project. We want the sample app to be the startup project
            var sampleApplication = new Project() { ProjectType = ProjectType.WinExe, Name = design.ProjectName, Directory = Path.Combine(design.OutputLocation, design.ProjectName)};
            sln.Projects.Add(sampleApplication);

            var themeProject = new Project() { ProjectType = ProjectType.Library, Name = design.ThemeAssemblyName, Directory = Path.Combine(design.OutputLocation, design.ThemeAssemblyName) };
            sln.Projects.Add(themeProject);

            AddFilesToThemeProject(themeProject, design);
            AddFilesToSampleApplication(sampleApplication, design);
            sampleApplication.References.Add(themeProject);

            sln.Generate();
        }

        private void AddFilesToSampleApplication(Project sampleApplication, Design design)
        {
            var sampleAppSourcePath = new DirectoryInfo(relativePathFromCurrentExecutableToSampleUiFolder);
            var absoluteSampleApplicationRoot = new DirectoryPathAbsolute(sampleAppSourcePath.FullName);
            AddAllFilesInPathToProject(absoluteSampleApplicationRoot, absoluteSampleApplicationRoot, sampleApplication, str => str.Replace("TestClientApplication", design.ProjectName));
            try
            {
                sampleApplication.UpdateFileContents(@".\App.xaml.cs", str => string.Format(str, design.ThemeAssemblyName, design.Theme.Name));                
            }
            catch (FormatException)
            {
                // FormatException can be thrown if the sample UI project is being re-generated (and thus can be ignored) - TODO consider adding a flag to the Project class to determine if it has already been generated
            }
        }

        private void AddFilesToThemeProject(Project themeProject, Design design)
        {
            var absoluteThemeFileRoot = new DirectoryPathAbsolute(AppDomain.CurrentDomain.BaseDirectory);
            var startThemeCopyDirectory =
                new DirectoryInfo(Path.Combine(relativePathFromCurrentExecutableToWpfThemes, design.Theme.Name));
            AddAllFilesInPathToProject(absoluteThemeFileRoot, new DirectoryPathAbsolute(startThemeCopyDirectory.FullName), themeProject, null);
            string baseResourceDictionaryName = Path.Combine(relativePathFromCurrentExecutableToWpfThemes, design.Theme.Name + ".xaml");
            themeProject.AddFile(new FileInfo(baseResourceDictionaryName).FullName, ".\\" + baseResourceDictionaryName);

            themeProject.UpdateFileContents(@".\Resources\Themes\Wpf\" + design.Theme.Name + ".xaml", new ColorUpdater(design).UpdateThemeColors);
        }



        private void AddAllFilesInPathToProject(DirectoryPathAbsolute absoluteSampleApplicationRoot, DirectoryPathAbsolute currentFolder, Project project, Func<string, string> postCopyUpdateFileContentsMethod)
        {
            System.Diagnostics.Debug.WriteLine("Copying contents of folder " + currentFolder.ToString());
            foreach (var file in currentFolder.ChildrenFilesPath)
            {
                project.AddFile(file.FileInfo.FullName, file.GetPathRelativeFrom(absoluteSampleApplicationRoot).Path, postCopyUpdateFileContentsMethod);
            }
            foreach (var subdirectory in currentFolder.ChildrenDirectoriesPath)
            {
                AddAllFilesInPathToProject(absoluteSampleApplicationRoot, subdirectory, project, postCopyUpdateFileContentsMethod);
            }
        }
    }

    public class ColorUpdater
    {
        private readonly Design design;

        public ColorUpdater(Design design)
        {
            this.design = design;
        }

        public string UpdateThemeColors(string input)
        {
            var doc = new XmlDocument();
            doc.LoadXml(input);
            var nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("x",  XamlNamespaceUri);
            nsManager.AddNamespace("d", WpfNamespaceUri);
            
            // update the colors
            UpdateColor(doc, nsManager, design.ColorScheme.Primary.Name, design.ColorScheme.Primary);
            UpdateColor(doc, nsManager, design.ColorScheme.Secondary.Name, design.ColorScheme.Secondary);
            UpdateColor(doc, nsManager, design.ColorScheme.Chrome.Name, design.ColorScheme.Chrome);
            UpdateColor(doc, nsManager, design.ColorScheme.ChromeAlternate.Name, design.ColorScheme.ChromeAlternate);
            UpdateColor(doc, nsManager, design.ColorScheme.Disabled.Name, design.ColorScheme.Disabled);
            UpdateColor(doc, nsManager, design.ColorScheme.Foreground.Name, design.ColorScheme.Foreground);
            UpdateColor(doc, nsManager, design.ColorScheme.Background.Name, design.ColorScheme.Background);
            
            var sb = new StringBuilder();
            doc.WriteTo(new XmlTextWriter(new StringWriter(sb)));
            return sb.ToString();
        }

        private const string WpfNamespaceUri = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        private const string XamlNamespaceUri = "http://schemas.microsoft.com/winfx/2006/xaml";

        private void UpdateColor(XmlDocument doc, XmlNamespaceManager nsManager, string name, DesignColor designColor)
        {
            UpdateItem(doc, nsManager, name + "Base", designColor.Base);
            UpdateItem(doc, nsManager, name + "LightAccent1", designColor.LightAccent1);
            UpdateItem(doc, nsManager, name + "LightAccent2", designColor.LightAccent2);
            UpdateItem(doc, nsManager, name + "DarkAccent1", designColor.DarkAccent1);
            UpdateItem(doc, nsManager, name + "DarkAccent2", designColor.DarkAccent2);
        }

        private void UpdateItem(XmlDocument doc, XmlNamespaceManager nsManager, string name, Color color)
        {
            string query = "//*/d:Color[attribute::x:Key=\"" + name + "\"]";
            System.Diagnostics.Debug.WriteLine(query);
            var node = doc.SelectSingleNode(query, nsManager);
            if (node == null)
            {
                throw new InvalidOperationException("Unable to find color node " + name);
            }
            SetColorAttribute(doc, node, "R", color.R);
            SetColorAttribute(doc, node, "G", color.G);
            SetColorAttribute(doc, node, "B", color.B);
            SetColorAttribute(doc, node, "A", color.A);
        }

        private void SetColorAttribute(XmlDocument doc, XmlNode node, string RgbaName, byte value)
        {
            var attribute = doc.CreateAttribute(null, RgbaName, doc.NamespaceURI);
            attribute.Value = value.ToString();
            node.Attributes.Append(attribute);
        }
    }
}
