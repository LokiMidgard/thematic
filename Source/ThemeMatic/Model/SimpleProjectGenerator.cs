using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Markup;

namespace ThemeMatic.Model
{
    // I'm going to call this the 'simple' project generator because it will do simple string.format() etc
    // to generate the project. In the future a more complicated T4 project generator or something like that
    // might come along
    public class SimpleProjectGenerator : IProjectGenerator
    {
        private readonly string resourceRootPath;
        private const string themePathFragment = @"Resources\Themes\Wpf\";

        public SimpleProjectGenerator(string resourceRootPath)
        {
            if (String.IsNullOrEmpty(resourceRootPath))
            {
                throw  new ArgumentException("resource root path was not correctly specified");
            }
            if (!File.Exists(resourceRootPath))
            {
                throw new ArgumentException("resource root path does not exist");
            }
            this.resourceRootPath = resourceRootPath;
        }

        public void Generate(Design design)
        {
            if (design == null)
            {
                throw new ArgumentNullException("design");
            }
            string folder = CreateFolderStructure(design);
            WriteResourceDictionaries(design, folder);
            WriteProject(design);
        }

        private void WriteProject(Design design)
        {
            // TODO
        }

        private void WriteResourceDictionaries(Design design, string wpfFolderName)
        {
            File.Copy(Path.Combine(resourceRootPath, themePathFragment, design.Theme.Name + ".xaml"), Path.Combine(wpfFolderName, design.Theme.Name + ".xaml"));
            CopyAll(new DirectoryInfo(Path.Combine(themePathFragment, design.Theme.Name)), new DirectoryInfo(Path.Combine(wpfFolderName, design.Theme.Name)));
        }

        private string CreateFolderStructure(Design design)
        {
            string baseDirectory = Path.Combine(design.OutputLocation, design.ProjectName);
            string themeProjectDirectory = Path.Combine(baseDirectory, design.ThemeAssemblyName);
            CreateIfNotExists(baseDirectory);
            CreateIfNotExists(themeProjectDirectory);

            // create \Resources\Themes\Wpf\<theme name>"
            string resources = Path.Combine(themeProjectDirectory, "Resources");
            CreateIfNotExists(resources);

            string themes = Path.Combine(resources, "Themes");
            CreateIfNotExists(themes);

            string wpfDirectoryName = Path.Combine(themes, "Wpf");
            CreateIfNotExists(wpfDirectoryName);

            return wpfDirectoryName;
        }

        private void CreateIfNotExists(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        // copied straight form the MSDN...the shame
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
