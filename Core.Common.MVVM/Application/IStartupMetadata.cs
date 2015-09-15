
using System;
using System.ComponentModel.Composition;
namespace Core.Common.MVVM
{
  public interface IApplicationViewMetadata
  {
    bool IsStartup { get; }
  }


  [AttributeUsage(AttributeTargets.Class), MetadataAttribute]
  public class StartupViewAttribute : ExportAttribute, IApplicationViewMetadata
  {
    public StartupViewAttribute():base()
    {
      IsStartup = true;
    }

    public bool IsStartup
    {
      get;
      private set;
    }
  }
}
