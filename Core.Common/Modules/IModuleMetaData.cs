
namespace Core.Modules
{
  public interface IModuleMetaData
  {
    string ModuleName
    {
      get;
    }

    string ModuleVersion
    {
      get;
    }

    string ModuleGuid
    {
      get;
    }
    int InitializationPriority { get;  }
    bool AutoActivate { get; }
  }
}
