using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Extensions;

namespace Core.FileSystem.Test
{

  [TestClass]
  public class CoreFileSystemRelativeFileSystemTest : FileSystemTest
  {
    protected override IFileSystem CreateFileSystem()
    {
      if (Directory.Exists("relfs")) Directory.Delete("relfs", true);
      Directory.CreateDirectory("relfs");
      var fs = new RelativeFileSystem("relfs");
      return fs;

    }

  }
}
