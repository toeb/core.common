using Core.FileSystem.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem.Test
{

  [TestClass]
  public class CoreFileSystemMemoryFileSystemTest : FileSystemTest
  {
    private IFileSystem uut;
    [TestInitialize]
    public void Initialize()
    {
      uut = new MemoryFileSystem();
    }

    [TestMethod]
    public void MemoryFileStream()
    {
      var entry = new MemoryFileSystemEntry();
      using (var stream = new MemoryFileSystemEntryStream(entry))
      {
        stream.WriteByte(22);
      }
      Assert.AreEqual(22, entry.Data[0]);
    }

    protected override IFileSystem CreateFileSystem()
    {
      return new MemoryFileSystem();
    }
  }
}
