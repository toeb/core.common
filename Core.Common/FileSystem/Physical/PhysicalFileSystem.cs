using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{
  /// <summary>
  /// Physical Filesystem wraps the normal .net file system
  /// </summary>
  [Export]
  [Export(typeof(IFileSystem))]
  public class PhysicalFileSystem : PhysicalModifiableFileSystem
  {
    

  }
}
