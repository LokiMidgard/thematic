using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Markup;
using NDepend.Helpers.FileDirectoryPath;

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
            if (!Directory.Exists(resourceRootPath))
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
            
            var resourceDictionaries = new List<string>();
            WriteResourceDictionaries(design, folder, resourceDictionaries);
            WriteProject(design, resourceDictionaries);
        }

        private void WriteProject(Design design, List<string> resourceDictionaries)
        {
            string projectFileName = Path.Combine(ThemeProjectDirectory(design), design.ThemeAssemblyName + ".csproj");
            if (!File.Exists(projectFileName))
            {
                List<string> relativePaths = GetRelativePathsForResourceDictionaries(design, resourceDictionaries);

                using (var fileStream = new StreamWriter(projectFileName, true))
                {
                    fileStream.Write(GenerateNewProjectFileContents(design, relativePaths));
                    fileStream.Flush();
                }
            }
        }

        private List<string> GetRelativePathsForResourceDictionaries(Design design, List<string> resourceDictionaries)
        {
            var projectRootDirectory = ThemeProjectDirectory(design);
            var absoluteProjectRoot = new DirectoryPathAbsolute(projectRootDirectory);

            var relativePaths = new List<string>();
            foreach (var path in resourceDictionaries)
            {
                var absoluteFile = new FilePathAbsolute(path);
                var relativeFile = absoluteFile.GetPathRelativeFrom(absoluteProjectRoot);
                relativePaths.Add(relativeFile.Path);
            }
            return relativePaths;
        }

        private void WriteResourceDictionaries(Design design, string wpfFolderName, List<string> resourceDictionariesCopied)
        {

            string targetBaseResourceDictionaryName = Path.Combine(wpfFolderName, design.Theme.Name + ".xaml");
            File.Copy(Path.Combine(resourceRootPath, themePathFragment, design.Theme.Name + ".xaml"), targetBaseResourceDictionaryName);
            resourceDictionariesCopied.Add(targetBaseResourceDictionaryName);

            CopyAll(new DirectoryInfo(Path.Combine(themePathFragment, design.Theme.Name)), new DirectoryInfo(Path.Combine(wpfFolderName, design.Theme.Name)), resourceDictionariesCopied);
        }

        public string BaseDirectory(Design design)
        {
            return Path.Combine(design.OutputLocation, design.ProjectName);
        }

        public string ThemeProjectDirectory(Design design)
        {
            return Path.Combine(BaseDirectory(design), design.ThemeAssemblyName);
        }

        private string CreateFolderStructure(Design design)
        {
            CreateIfNotExists(BaseDirectory(design));
            CreateIfNotExists(ThemeProjectDirectory(design));

            // create \Resources\Themes\Wpf\<theme name>"
            string resources = Path.Combine(ThemeProjectDirectory(design), "Resources");
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

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target, List<string> filesCopied)
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
            foreach (FileInfo fi in source.GetFiles().Where(a => a.Extension.ToLower() == ".xaml"))
            {
                string targetFileName = Path.Combine(target.ToString(), fi.Name);
                fi.CopyTo(targetFileName, true);
                filesCopied.Add(targetFileName);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir, filesCopied);
            }
        }

        public string GenerateNewProjectFileContents(Design design, List<string> relativePathsFromProjectRootToXamlFiles)
        {
            string xamlFileNodes = GenerateXamlFileNodeFragment(relativePathsFromProjectRootToXamlFiles);
            return string.Format(projectSkeleton, Guid.NewGuid(), xamlFileNodes);
        }

        private string GenerateXamlFileNodeFragment(List<string> relativePathsFromProjectRootToXamlFiles)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in relativePathsFromProjectRootToXamlFiles)
            {
                sb.Append(
                    string.Format(
                        @"
    <Page Include='{0}'>
      <SubType>Designer</SubType>
      <Generator>MSBuild:Compile</Generator>
    </Page>",
                        item));
            }
            return sb.ToString();
        }

        private const string projectSkeleton = @"<?xml version='1.0' encoding='utf-8'?>
<Project ToolsVersion='4.0' DefaultTargets='Build' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>8.0.30703</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{{{0}}}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>ThemeTest</RootNamespace>
    <AssemblyName>ThemeTest</AssemblyName>
    <TargetFrameworkVersion>v4.0</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
<ItemGroup>
    <Reference Include='PresentationCore' />
    <Reference Include='PresentationFramework' />
    <Reference Include='ReachFramework' />
    <Reference Include='System' />
    <Reference Include='System.Core' />
    <Reference Include='Microsoft.CSharp' />
    <Reference Include='System.Xaml' />
    <Reference Include='WindowsBase' />
    <Reference Include='PresentationUI, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL'>
      <SpecificVersion>False</SpecificVersion>
      <HintPath>C:\Windows\Microsoft.NET\Framework\v3.0\WPF\PresentationUI.dll</HintPath>
    </Reference>
  </ItemGroup>
  <ItemGroup>
    {1}
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>
";
    }
}
