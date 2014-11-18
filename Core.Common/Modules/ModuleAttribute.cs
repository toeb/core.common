using System.ComponentModel.Composition;

namespace Core.Modules
{
  [MetadataAttribute]
  public class ModuleAttribute : ExportAttribute, IModuleMetaData
  {
    public ModuleAttribute() { InitializationPriority = 10; AutoActivate = true; AutoDiscover = true; }

    /// <summary>
    /// set to false if you do not want thsi module to be automatically discovered
    /// </summary>
    public bool AutoDiscover
    {
      get;
      set;
    }
    /// <summary>
    /// set to false if you do not want the module to be automatically activated
    /// </summary>
    public bool AutoActivate
    {
      get;
      set;
    }

    public string ModuleName
    {
      get;
      private set;
    }

    public string ModuleVersion
    {
      get;
      private set;
    }

    public string ModuleGuid
    {
      get;
      private set;
    }
    public int InitializationPriority { get; set; }
  }
}
