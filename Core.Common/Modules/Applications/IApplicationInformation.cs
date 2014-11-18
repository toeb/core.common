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
  public interface IApplicationInformation : IIdentifiable<Guid>
  {

    Version ApplicationVersion { get; }
    string ApplicationName { get; }
    Type ApplicationType { get; }
  }
}
