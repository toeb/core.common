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
  public class CoreFileSystemPhysicalReadonlyFileSystemTest : AbstractReadonlyFileSystemTest
  {
    protected override void CreateFile(string path, string content = "")
    {
      path = uut.NormalizePath(path);
      using (var writer = new StreamWriter(File.OpenWrite(path)))
      {
        writer.Write(content);
      }
    }
    protected override void CreateDir(string path)
    {
      path = uut.NormalizePath(path);
      Directory.CreateDirectory(path);
    }

    protected override IReadonlyFileSystem CreateReadonlyFileSystem()
    {
      return new PhysicalReadonlyFileSystem();
    }

    protected override void DeleteDir(string path)
    {

      Directory.Delete(path, true);
    }

    public override string NormalizePathForFs(string path)
    {
      return Path.GetFullPath(path);
    }
  }
}
