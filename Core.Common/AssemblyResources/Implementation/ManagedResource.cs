using Core.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resources
{
  public class ManagedResource : IManagedResource
  {
    public static ManagedResource[] None = new ManagedResource[0];
    public ManagedResource(string key, Assembly assembly)
    {
      this.Id = key;
      Assembly = assembly;
    }
    public Assembly Assembly
    {
      get;
      private set;
    }

    public string Id
    {
      get;
      private set;
    }
  }
}
