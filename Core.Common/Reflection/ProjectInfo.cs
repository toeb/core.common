using System.Reflection;

namespace Core
{
  public class ProjectInfo : IProjectInfo
  {



    public string ProjectDir
    {
      get;
      set;
    }

    public Assembly Assembly
    {
      get;
      set;
    }

    public string AssemblyName
    {
      get { return Assembly.GetName().Name; }
    }
  }
}
