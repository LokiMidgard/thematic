using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NDepend.Helpers.FileDirectoryPath;

namespace ThemeMatic.Model.VisualStudio
{
    public class Project
    {
        public Project()
        {
            UniqueIdentifier = Guid.NewGuid();
            References = new List<Project>();
        }

        public Guid UniqueIdentifier { get; private set; }

        private List<string> files = new List<string>();
        public List<Project> References { get; private set; }

        public string Name { get; set; }

        private DirectoryPathAbsolute absoluteProjectRoot;
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
                absoluteProjectRoot = new DirectoryPathAbsolute(value);
                directory = value;
            }
        }

        public string FilePath 
        {
            get { return Path.Combine(Directory, Name + ".csproj"); }
        }

        public ProjectType ProjectType { get; set; }

        public void AddFile(string absoluteSourcePath, string relativeTargetPath)
        {
            if (!File.Exists(absoluteSourcePath))
            {
                throw new ArgumentException(string.Format("The file specified {0} does not exist.", absoluteSourcePath));
            }
            
            var filePathRelative = new FilePathRelative(relativeTargetPath);
            var absoluteTargetPath = filePathRelative.GetAbsolutePathFrom(absoluteProjectRoot);
            if (!absoluteTargetPath.ParentDirectoryPath.Exists)
            {
                CreateParentFolder(absoluteTargetPath.ParentDirectoryPath);
            }

            if (!File.Exists(absoluteTargetPath.Path))
            {
                File.Copy(absoluteSourcePath, absoluteTargetPath.Path);                
            }

            files.Add(relativeTargetPath);
        }

        public void UpdateFileContents(string relativeTargetPath, Func<string, string> updateMethod)
        {
            var filePathRelative = new FilePathRelative(relativeTargetPath);
            var absoluteTargetPath = filePathRelative.GetAbsolutePathFrom(absoluteProjectRoot);
            if (!File.Exists(absoluteTargetPath.Path))
            {
                throw new InvalidOperationException("The file you specified is not part of this project");
            }
            var currentFileContents = "";
            using (var streamReader = new StreamReader(absoluteTargetPath.Path))
            {
                currentFileContents = streamReader.ReadToEnd();
            }
            var updatedContents = updateMethod.Invoke(currentFileContents);
            using (var streamWriter = new StreamWriter(absoluteTargetPath.Path, false))
            {
                streamWriter.Write(updatedContents);
                streamWriter.Flush();
            }
        }

        private void CreateParentFolder(DirectoryPathAbsolute directory)
        {
            if (!directory.ParentDirectoryPath.Exists)
            {
                CreateParentFolder(directory.ParentDirectoryPath);
            }
            System.IO.Directory.CreateDirectory(directory.Path);
        }


        public void Generate()
        {
            if (!File.Exists(FilePath))
            {
                var projectContents = GetProjectContents();
                var projectReferences = GetProjectReferences();
                string projectFileContents = string.Format(ProjectFormatString, this.UniqueIdentifier, this.ProjectType, projectContents, this.Name, projectReferences);
                using (var fileWriter = new StreamWriter(this.FilePath))
                {
                    fileWriter.Write(projectFileContents);
                    fileWriter.Flush();
                }
            }
        }

        private string GetProjectReferences()
        {
            if (this.References.Count > 0)
            {
                var projectReferences = startItemGroup;
                var absoluteProjectFile = new FilePathAbsolute(this.FilePath);
                foreach (var reference in References)
                {
                    var referencedProjectFile = new FilePathAbsolute(reference.FilePath);
                    projectReferences += string.Format(ProjectReferenceFormatString,
                                                       referencedProjectFile.GetPathRelativeFrom(
                                                           absoluteProjectFile.ParentDirectoryPath).Path,
                                                       reference.UniqueIdentifier.ToString().ToUpper(), reference.Name);
                }
                projectReferences += endItemGroup;

                return projectReferences;
            }
            return "";
        }

        private const string ProjectReferenceFormatString = @"<ProjectReference Include=""{0}"">
      <Project>{{{1}}}</Project>
      <Name>{2}</Name>
    </ProjectReference>";

        private const string startItemGroup = "<ItemGroup>\n";
        private const string endItemGroup = "\n</ItemGroup>";

        private string GetProjectContents()
        {
            string projectContents = startItemGroup;
            foreach (var file in this.files)
            {
                projectContents += GetProjectNodeForFile(file);
            }
            projectContents += endItemGroup;
            return projectContents;
        }

        private string GetProjectNodeForFile(string file)
        {
            if (file.StartsWith(@".\"))
            {
                file = file.Remove(0, 2);
            }

            if (file == "App.xaml" || file == @".\App.xaml")
            {
                return @"	<ApplicationDefinition Include=""App.xaml"">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </ApplicationDefinition>";
            }
            else if (file.EndsWith(".xaml"))
            {
                return
                    string.Format(
                        @"    <Page Include=""{0}"">
      <SubType>Designer</SubType>
      <Generator>MSBuild:Compile</Generator>
    </Page>",
                        file);
            }
            else if (file.EndsWith(".xaml.cs"))
            {
                return
                    string.Format(
                        @"    <Compile Include=""{0}"">
      <DependentUpon>{1}</DependentUpon>
    </Compile>", file,
                        file.Substring(0, file.Length - ".cs".Length /* remove trailling .cs */));
            }
            else if (file.EndsWith(".cs"))
            {
                return string.Format("<Compile Include=\"{0}\" />", file);
            }
            return string.Empty;
        }



        private const string ProjectFormatString = @"<?xml version='1.0' encoding='utf-8'?>
<Project ToolsVersion='4.0' DefaultTargets='Build' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>8.0.30703</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{{{0}}}</ProjectGuid>
    <OutputType>{1}</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>ThemeTest</RootNamespace>
    <AssemblyName>{3}</AssemblyName>
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
{2}
{4}
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>
";
    }

    public enum ProjectType
    {
        Library,
        WinExe
    }
}
