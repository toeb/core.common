using System;
using System.Diagnostics;

namespace Core.Modules.Applications
{
  [DebuggerDisplay("{ModuleName} {Version}")]
  public class ModuleInformaton : NotifyPropertyChangedBase, IModuleInformation
  {
    public override string ToString()
    {
      return string.Format("{0} {1}",ModuleName,Version);
    }

    public ModuleInformaton(Type type) : this(type, null, null, null) { }
    public ModuleInformaton(Type type,string moduleName, Guid? id, Version version)
    {
      InitializationPriority = 10;
      if (string.IsNullOrEmpty(moduleName)) moduleName = type.FullName;
      if (!id.HasValue || id.Value == Guid.Empty) id = type.GUID;
      if (version == null) version = new Version(1, 0, 0, 0);
      ModuleName = moduleName;
      Id = id.Value;
      Version = version;
      Type = type;
    }
    public string ModuleName
    {
      get;
      private set;
    }

    public Version Version
    {
      get;
      private set;
    }

    public Guid Id
    {
      get;
      private set;
    }

    public Type Type
    {
      get;
      private set;
    }


    public int InitializationPriority
    {
      get;
      set;
    }


    public bool AutoActivate
    {
      get;
      set;
    }
  }
}
