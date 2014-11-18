
using System.Linq;
using System.Reflection;

namespace Core
{
  public static class IProjectInfoExtensions
  {
    /// <summary>
    /// returns the project info 
    /// 
    /// works when this pre build event is added to the project (and the generated file is compiled):
    /// <code>echo namespace $(ProjectName){ public  static class  ProjectInfo{ public static readonly string ProjectDir = @"$(ProjectDir)";  }} > $(ProjectDir)\ProjectInfo.cs</code>
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IProjectInfo GetProjectInfo(this Assembly assembly)
    {
      var info = new ProjectInfo() { Assembly = assembly };
      var infoType = assembly.DefinedTypes.SingleOrDefault(t => t.Name == "ProjectInfo");
      if (infoType == null) return info;
      var field = infoType.GetField("ProjectDir");
      if (field == null) return info;
      var value = field.GetValue(null);
      info.ProjectDir = value as string;
      return info;
    }


  }
}
