using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.TestingUtilities;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Core.FileSystem.Test
{
  [TestClass]
  public class CoreFileSystemPhysicalFileSystemTests : TestBase
  {
    [TestMethod]
    [Timeout(1000)]
    public void FileExistsPerformace()
    {
      // also test virtual path provider
    }

    [TestMethod]
    public void PhysicalFileSystem()
    {
      var uut = new PhysicalFileSystem();

    }

    [TestMethod]
    public void Exists()
    {
      var uut = new PhysicalFileSystem();
      var exists = uut.Exists(".");
      Assert.IsTrue(exists);
    }

    [TestMethod]
    public void GetEntries()
    {
      var uut = new PhysicalFileSystem();
      if (Directory.Exists("temp")) Directory.Delete("temp", true);
      Directory.CreateDirectory("temp");
      File.Create("temp/a.txt");
      File.Create("temp/b.txt");
      File.Create("temp/c.txt");
      Directory.CreateDirectory("temp/temp2");
      var entries = uut.GetEntries("temp");
      Assert.IsTrue(
        entries.Select(e => e.Contains("a.txt")).Any() &&
        entries.Select(e => e.Contains("b.txt")).Any() &&
        entries.Select(e => e.Contains("c.txt")).Any() &&
        entries.Select(e => e.Contains("temp2")).Any()
        );
    }
  }
}
