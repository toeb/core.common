using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Reflect;
using Core.Graph;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Evaluation;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Core.Common.CodeGeneration
{
  
  

  public class FileGenerationContext : GenerationContext
  {

  }

  public class GenerationContext
  {
    public ICollection<IGenerator> Generators { get; set; }
  }
  public interface IGenerator
  {
    void Generate(GenerationContext context);
  }

  class CustomLogger : ILogger
  {
    public string Parameters
    {
      get; set;
    }

    public LoggerVerbosity Verbosity
    {
      get;

      set;
    }

    public void Initialize(IEventSource eventSource)
    {
    //  eventSource.AnyEventRaised += EventSource_AnyEventRaised;
      eventSource.ErrorRaised += EventSource_ErrorRaised;
    }

    private void EventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
    {
      Debug.WriteLine(e.Message);
    }

    private void EventSource_AnyEventRaised(object sender, BuildEventArgs e)
    {
      
      Debug.WriteLine(e.Message);
    }

    public void Shutdown()
    {
    }
  }


  public class ProjectContext
  {
    public ProjectContext(string projectFile)
    {
      ProjectFile = projectFile;
      OutputPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

      if (!Directory.Exists(OutputPath))
      {
        Directory.CreateDirectory(OutputPath);
      }

      LibrarySearchPaths = new List<string>();
      ProjectCollection = new ProjectCollection();
      ProjectCollection.SetGlobalProperty("Configuration", "Release");
      ProjectCollection.SetGlobalProperty("Platform", "Any CPU");
      ProjectCollection.SetGlobalProperty("OutputPath", OutputPath);
      Project = ProjectCollection.LoadProject(this.ProjectFile, "4.0");

    }
    public string OutputPath { get; private set; }
    public string ProjectFile { get; set; }

    public string TargetPath { get; private set; }
    public bool BuildResult { get; private set; }
    public IList<string> LibrarySearchPaths { get; private set; }
    public Microsoft.Build.Evaluation.Project Project { get; private set; }
    public ProjectCollection ProjectCollection { get; private set; }

    public bool BuildProject()
    {

      ProjectCollection.RegisterLogger(new CustomLogger());

      var projectReferences = new HashSet<string>();
      var projectReferenceMap = new Dictionary<string, object>();


      var success = Project.Build();

      this.TargetPath = Path.Combine(Project.DirectoryPath, Project.GetProperty("TargetPath").EvaluatedValue);
      BuildResult = success;
      LibrarySearchPaths.Add(OutputPath);
      return success;
    }

    public AppDomain ReflectionDomain { get; private set; }


    public void LoadReflectionDomain()
    {
      
      ReflectionDomain = AppDomain.CurrentDomain; //AppDomain.CreateDomain("reflectionDomain");
      ReflectionDomain.ReflectionOnlyAssemblyResolve += ResolveAssemblyInDomain;      
      Assembly.ReflectionOnlyLoadFrom(TargetPath);
    }
    public AssemblyContext CreateAssemblyContext()
    {
        if(ReflectionDomain == null)
        {
          LoadReflectionDomain();        
        }
        var assembly = ReflectionDomain.ReflectionOnlyGetAssemblies().Single(asm => asm.Location == TargetPath);
        var assemblyContext = new AssemblyContext(this, assembly);
        return assemblyContext;
    }

    private Assembly ResolveAssemblyInDomain(object sender, ResolveEventArgs args)
    {
      foreach (var searchPath in LibrarySearchPaths)
      {
        var libName = Regex.Match(args.Name, "[^,]+").Value;
        var file = Directory.EnumerateFiles(searchPath, libName + ".dll").SingleOrDefault();

        if(file!=null)
        {
          return Assembly.ReflectionOnlyLoadFrom(file);
        }
        

      }

      return null;
    }



    public IEnumerable<TypeContext> TypeContexts { get { return typeContexts; } }
    internal void AddTypeContexts(IEnumerable<TypeContext> typeContexts)
    {
      foreach(var ctx in typeContexts)
      {
        this.typeContexts.Add(ctx);
      }
    }
    private List<TypeContext> typeContexts = new List<TypeContext>();
  }

  public class AssemblyContext
  {
    public Assembly Assembly { get; private set; }
    public ProjectContext ProjectContext { get; private set; }

    public AssemblyContext(ProjectContext project,  Assembly assembly)
    {
      Assembly = assembly;
      this.ProjectContext = project;

      typeContexts = GetIdlTypes().Select(t => new TypeContext(this, t)).ToList();
      ProjectContext.AddTypeContexts(typeContexts);
    }

    
    public IEnumerable<Type> GetIdlTypes()
    {
      return Assembly.GetTypes().Where(t => t.IsPublic && (t.IsInterface || t.IsEnum )&& !t.IsGenericTypeDefinition && !t.IsGenericType).ToArray();
    }
    private List<TypeContext> typeContexts;
    public IEnumerable<TypeContext> TypeContexts
    {
      get
      {
        return typeContexts;
      }
    }
  }

  public class TypeContext
  {

    public TypeContext(AssemblyContext assemblyContext, Type t)
    {
      AssemblyContext = assemblyContext;
      Type = t;
    }
    public Type Type { get; private set; }
    public AssemblyContext AssemblyContext { get; private set; }
  }

  [TestClass]
  public class Class1
  {
    [TestMethod]
    [TestCategory("IDL")]
    public void Testit()
    {
      var context = new ProjectContext(System.IO.Path.GetFullPath("../../TestSolution/IdlLib2/IdlLib2.csproj"));


      context.BuildProject();

      context.LoadReflectionDomain();

      var assemblyContext  =context.CreateAssemblyContext();


      var typeContexts = context.TypeContexts.ToArray();




    }
  }
}
