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
        }

        public Guid UniqueIdentifier { get; private set; }

        private List<string> files = new List<string>();

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
                
            }
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

        private const string ThemeProjectSkeleton = @"<?xml version='1.0' encoding='utf-8'?>
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

        private const string sampleUIProjectItems =
    @"<ApplicationDefinition Include=""App.xaml"">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </ApplicationDefinition>
    <Page Include=""MainWindow.xaml"">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Compile Include=""App.xaml.cs"">
      <DependentUpon>App.xaml</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include=""MainWindow.xaml.cs"">
      <DependentUpon>MainWindow.xaml</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""Properties\AssemblyInfo.cs"">
      <SubType>Code</SubType>
    </Compile>
    <Compile Include=""Properties\Resources.Designer.cs"">
      <AutoGen>True</AutoGen>
      <DesignTime>True</DesignTime>
      <DependentUpon>Resources.resx</DependentUpon>
    </Compile>
    <Compile Include=""Properties\Settings.Designer.cs"">
      <AutoGen>True</AutoGen>
      <DependentUpon>Settings.settings</DependentUpon>
      <DesignTimeSharedInput>True</DesignTimeSharedInput>
    </Compile>
    <EmbeddedResource Include=""Properties\Resources.resx"">
      <Generator>ResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.cs</LastGenOutput>
    </EmbeddedResource>
    <None Include=""Properties\Settings.settings"">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.cs</LastGenOutput>
    </None>
    <AppDesigner Include=""Properties\"" />";


    }

    public enum ProjectType
    {
        Library,
        WindowsExecutable
    }
}
