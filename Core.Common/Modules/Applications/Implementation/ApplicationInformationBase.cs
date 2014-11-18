using Core.Collections;
using Core.Graph;
using Core.Graph.Directed;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Threading.Tasks;
using Core.FileSystem;
using Core.Resources;

namespace Core.Modules.Applications
{
  public class ApplicationInformation : IApplicationInformation
  {
    public ApplicationInformation(Type type, string name, Guid? id, Version version)
    {
      if (type == null) throw new ArgumentNullException("type");
      Id = type.GUID;
      ApplicationName = string.IsNullOrEmpty(name) ? type.FullName : name;
      if (id.HasValue) Id = id.Value;
      ApplicationVersion = version ?? new Version(1,0,0,0);

    }

    public Version ApplicationVersion
    {
      get;
      set;
    }

    public string ApplicationName
    {
      get;
      set;
    }

    public Guid Id
    {
      get;
      set;
    }


    public Type ApplicationType
    {
      get;
      set;
    }
  }
}
