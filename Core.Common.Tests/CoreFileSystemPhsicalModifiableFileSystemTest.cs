using Core.Trying;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.FileSystem.Test
{
  [TestClass]
  public class CoreFileSystemPhsicalModifiableFileSystemTest : AbstractModifiableFileSystemTest
  {

    protected override IReadonlyFileSystem CreateReadonlyFileSystem()
    {
      return new PhysicalReadonlyFileSystem();
    }

    protected override IFileSystem CreateFileSystem()
    {
      return new PhysicalModifiableFileSystem();
    }
  }
}
