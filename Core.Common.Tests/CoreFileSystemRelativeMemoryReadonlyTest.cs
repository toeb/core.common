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
  public class CoreFileSystemRelativeMemoryReadonlyTest : AbstractReadonlyFileSystemTest
  {
    private MemoryFileSystem mfs;

    protected override void CreateFile(string path, string content)
    {
      mfs.WriteTextFile("testdir/" + path, content);
    }

    protected override void DeleteDir(string path)
    {

      mfs.DeleteDirectory("testdir/" + path, true);
    }

    protected override void CreateDir(string path)
    {
      mfs.CreateDirectory("testdir/" + path);
    }

    protected override IReadonlyFileSystem CreateReadonlyFileSystem()
    {
      mfs = new MemoryFileSystem();
      return mfs.ScopeTo("testdir");
    }

    public override string NormalizePathForFs(string path)
    {
      return FileSystem.RelativePathUtility.Normalize(path);
    }
  }
}
