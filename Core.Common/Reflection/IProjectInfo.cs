using System.Reflection;

namespace Core
{
  public interface IProjectInfo
  {
    string AssemblyName { get; }
    Assembly Assembly { get; }
    string ProjectDir { get; }
  }
}
