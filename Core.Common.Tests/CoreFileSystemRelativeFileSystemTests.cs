using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.TestingUtilities;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Core.FileSystem.Test
{

  [TestClass]
  public class CoreFileSystemRelativeFileSystemTests : TestBase
  {
    [TestMethod]
    public void Create()
    {
      var pfs = new PhysicalFileSystem();
      var uut = new RelativeFileSystem(pfs, ".");
      Assert.AreEqual(Path.GetFullPath("."), uut.AbsolutePath);
      Assert.AreEqual(pfs, uut.Parent);
    }
    [TestMethod]
    public void Issue1()
    {
      var fs = new PhysicalFileSystem();
      var path = fs.NormalizePath("../../../");
      var rfs = new RelativeFileSystem(fs, path);
      var entries = rfs.GetEntries(".").ToArray();

    }


    [TestMethod]
    public void NormalizePath()
    {
      var pfs = new PhysicalFileSystem();
      var uut = new RelativeFileSystem(pfs, ".");
      Assert.AreEqual("~/", uut.NormalizePath(""));
      Assert.AreEqual("~/", uut.NormalizePath("."));
      Assert.AreEqual("~/", uut.NormalizePath("./"));
      Assert.AreEqual("~/", uut.NormalizePath("/"));
      Assert.AreEqual("~/", uut.NormalizePath("~/"));
      Assert.AreEqual("~/test", uut.NormalizePath("test"));
      Assert.AreEqual("~/test", uut.NormalizePath("./test"));
      Assert.AreEqual("~/test/", uut.NormalizePath("./test/"));
      Assert.AreEqual("~/test/test2", uut.NormalizePath("test/test2"));

    }
    [TestMethod]
    public void Issue1b()
    {
      var fs = new PhysicalFileSystem();
      var path = fs.NormalizePath("../../../");

      var rfs = new RelativeFileSystem(fs, path);
      var result = rfs.VirtualAbsoluteToVirtualPath("~/.nuget");
    }
    [TestMethod]
    public void ToVirtualPath()
    {
      var pfs = new PhysicalFileSystem();
      var uut = new RelativeFileSystem(pfs, ".");
      var uut2 = uut.ScopeTo("hello");

      Assert.AreEqual("~/", uut.ToVirtualAbsolutePath(""));
      Assert.AreEqual("~/", uut2.ToVirtualPath(""));
      Assert.AreEqual("~/hello/", uut2.ToVirtualAbsolutePath(""));


    }
    [TestMethod]
    public void ToVirtualPath2()
    {
      var pfs = new PhysicalFileSystem();
      var uut = new RelativeFileSystem(pfs, ".");
      var uut2 = uut.ScopeTo("hello");
      var uut3 = uut2.ScopeTo("muha/alter");

      Assert.AreEqual("~/", uut3.ToVirtualPath(""));
      Assert.AreEqual("~/hello/muha/alter/", uut3.ToVirtualAbsolutePath(""));
      Assert.AreEqual("~/hello/muha/alter/lol", uut3.ToVirtualAbsolutePath("lol"));


    }

    [TestMethod]
    public void ToAbsolutePath()
    {
      var pfs = new PhysicalFileSystem();
      var uut = new RelativeFileSystem(pfs, ".");
      var uut2 = uut.ScopeTo("hello");
      var uut3 = uut2.ScopeTo("muha/alter");

      Assert.AreEqual(pfs.NormalizePath("."), uut.ToAbsolutePath(""));
      Assert.AreEqual(pfs.NormalizePath("hello"), uut2.ToAbsolutePath(""));
      Assert.AreEqual(pfs.NormalizePath("hello\\muha\\alter"), uut3.ToAbsolutePath(""));

    }

    [TestMethod]
    public void AbsoluteToVirtual()
    {

      var pfs = new PhysicalFileSystem();
      var uut = new RelativeFileSystem(pfs, ".");
      var uut2 = uut.ScopeTo("hello");
      var uut3 = uut2.ScopeTo("muha/alter");

      var path = Path.GetFullPath("hello\\muha\\alter\\test\\text.txt");
      var result = uut3.AbsoluteToVirtualPath(path);
      Assert.AreEqual("~/test/text.txt", result);
    }

    [TestMethod]
    public void CompositeFileSystemCreation()
    {
      var compositeFileSystem = new CompositeFileSystem();

      Assert.AreEqual(false, compositeFileSystem.Exists("."));
      Assert.AreEqual(0, compositeFileSystem.GetEntries(".").Count());
    }

    [TestMethod]
    public void CompositeFileSystem()
    {
      var cfs = new CompositeFileSystem();
      var baseFs = new RelativeFileSystem(".");
      cfs.AddFileSystem(new RelativeFileSystem(baseFs, "folder1"));
      cfs.AddFileSystem(new RelativeFileSystem(baseFs, "folder1/folder3"));
      cfs.AddFileSystem(new RelativeFileSystem(baseFs, "folder2"));

      Assert.IsTrue(cfs.Exists("test3.txt"));
      Assert.IsTrue(cfs.Exists("test1.txt"));
      Assert.IsTrue(cfs.Exists("test2.txt"));
    }

    [TestMethod]
    public void CompositeFileSystemBreakDownIssue()
    {
      var cfs = new CompositeFileSystem();
      var baseFs = new RelativeFileSystem(".");
      var innerFs = new RelativeFileSystem(baseFs, "folder1");
      cfs.AddFileSystem(innerFs);

      innerFs.Exists("test3.txt");
      //var fs = cfs.GetFileSystemFromPath("test3.txt");

    }


    [TestMethod]
    public void ShouldFindCorrectFileSystem()
    {
      var cfs = new CompositeFileSystem();
      var fs1 = new MemoryFileSystem();
      var fs2 = new MemoryFileSystem();
      cfs.AddFileSystem(fs1);
      cfs.AddFileSystem(fs2);
      fs1.WriteTextFile("file1.txt", "content");
      fs2.WriteTextFile("file1.txt", "content");
      fs2.WriteTextFile("file2.txt", "content");
      Assert.AreEqual(fs1, cfs.GetFileSystemFromPath("file1.txt"));
      Assert.AreEqual(fs2, cfs.GetFileSystemFromPath("file2.txt"));
    }
    [TestMethod]
    public void ShouldFindCorrectPhysicalFileSystem()
    {
      var cfs = new CompositeFileSystem();
      var fs1 = new MemoryFileSystem();
      var fs2 = new PhysicalFileSystem();
      cfs.AddFileSystem(fs1);
      cfs.AddFileSystem(fs2);
      fs1.WriteTextFile("file1.txt", "asd");
      fs2.WriteTextFile("file1.txt", "asd");
      fs2.WriteTextFile("file2.txt", "asd");

      Assert.AreEqual(fs1, cfs.GetFileSystemFromPath("file1.txt"));
      Assert.AreEqual(fs2, cfs.GetFileSystemFromPath("file2.txt"));


    }



    [TestMethod]
    public void ShouldFindCorrectRelativeFileSystem()
    {
      var cfs = new CompositeFileSystem();
      var fs1 = new PhysicalFileSystem().ScopeTo("test");
      cfs.AddFileSystem(fs1);
      try
      {
        fs1.CreateDirectory(".");
      }
      catch (Exception e) { }
      fs1.CreateFile("test.txt").Close();

      var fs = cfs.GetFileSystemFromPath("test.txt");
      Assert.AreEqual(fs1, fs);
    }

    [TestMethod]
    public void ShouldFindCorrectRelativeFileSystem2()
    {

      var cfs = new CompositeFileSystem();
      var fs1 = new PhysicalFileSystem().ScopeTo(".");
      cfs.AddFileSystem(fs1);
      try
      {
        fs1.CreateDirectory(".");
      }
      catch (Exception e) { }
      fs1.CreateFile("test.txt").Close();

      var fs = cfs.GetFileSystemFromPath("test.txt");
      Assert.AreEqual(fs1, fs);
    }

    [TestMethod]
    public void RelFsShouldContainPhsysicalFile()
    {
      var fs1 = new PhysicalFileSystem().ScopeTo("test");
      try
      {
        fs1.CreateDirectory(".");
      }
      catch (Exception e) { }
      fs1.CreateFile("test.txt").Close();

      Assert.IsTrue(fs1.Exists("test.txt"));
      Assert.IsTrue(fs1.IsFile("test.txt"));
      Assert.IsFalse(fs1.IsDirectory("test.txt"));

    }



  }
}
