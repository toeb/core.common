using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem.Test
{
  [TestClass]
  public class CoreFileSystemRelativeMemoryFileSystemTest
  {
    private IRelativeFileSystem uut;
    private IRelativeFileSystem uut2;
    [TestInitialize]
    public void Init()
    {
      uut = new MemoryFileSystem().ScopeTo("testdir1").ScopeTo("testdir2");
      uut2 = new PhysicalFileSystem().ScopeTo("C:\\test").ScopeTo("testdir");
    }

    [TestMethod]
    public void ShouldNormalizePathCorrectly()
    {
      var normalized = uut.NormalizePath("test/path/text.txt");
      Assert.AreEqual("~/test/path/text.txt", normalized);
    }

    [TestMethod]
    public void ShouldReturnCorrectAbsolutePath()
    {
      var absolute = uut.ToAbsolutePath("test/path");
      Assert.AreEqual("~/testdir1/testdir2/test/path", absolute);

    }

    [TestMethod]
    public void ShouldTransformToPhysicalAbsolutePath()
    {
      var absolute = uut2.ToAbsolutePath("test/path");
      Assert.AreEqual("C:\\test\\testdir\\test\\path", absolute);
    }

    [TestMethod]
    public void ShouldTransformPhysicalPathToVirtualPath()
    {
      var virt = uut2.AbsoluteToVirtualPath("C:\\test\\testdir\\test\\path");

      Assert.AreEqual("~/test/path", virt);
    }
    [TestMethod]
    public void ShouldTransformAbsolutePathToRelativePath()
    {
      var relative = uut.AbsoluteToVirtualPath("~/testdir1/testdir2/test/path");
      Assert.AreEqual("~/test/path", relative);
    }
    [TestMethod]
    public void ShouldTransformToAbsoluteRelativePath()
    {
      var absRelative = uut.AbsoluteToVirtualAbsolutePath("~/testdir1/testdir2/test/path");
      Assert.AreEqual("~/testdir2/test/path", absRelative);
    }
    [TestMethod]
    public void ShouldTranformToAbsoluteVirtualPath()
    {
      var absoluteVirtual = uut.ToVirtualAbsolutePath("test/path");
      Assert.AreEqual("~/testdir2/test/path", absoluteVirtual);
    }

    [TestMethod]
    public void ShouldReturnRootFileSystem()
    {
      var mfs = new MemoryFileSystem();
      var uut = mfs.ScopeTo("a").ScopeTo("b").ScopeTo("c");
      Assert.AreEqual(mfs, uut.Root);
    }

    [TestMethod]
    public void ShouldReturnCorrectRootPathForMfs()
    {
      var mfs = new MemoryFileSystem();
      var uut = mfs.ScopeTo(".");
      Assert.AreEqual("~/", uut.RootPath);
    }

    [TestMethod]
    public void ShouldReturnCorrectRootPathForPfs()
    {
      var pfs = new PhysicalFileSystem();
      var uut = pfs.ScopeTo("C:\\");
      Assert.AreEqual("C:\\", uut.RootPath);
    }


    [TestMethod]
    public void ShouldReturnCoorectRootPathForMfsWithDeepFolderStructure()
    {
      var mfs = new MemoryFileSystem();
      var uut = mfs.ScopeTo(".").ScopeTo("a").ScopeTo("b").ScopeTo("c").ScopeTo("d/e/f/g");
      Assert.AreEqual("~/", uut.RootPath);
    }

    [TestMethod]
    public void ShouldReturnCoorectRootPathForPfsWithDeepFolderStructure()
    {
      var mfs = new PhysicalFileSystem();
      var uut = mfs.ScopeTo("C:\\").ScopeTo("a").ScopeTo("b").ScopeTo("c").ScopeTo("d/e/f/g");
      Assert.AreEqual("C:\\", uut.RootPath);
    }


    [TestMethod]
    public void ShouldReturnCorrectAbsoluteVirtualPathForMfsWithDeepFolderStructure()
    {
      var mfs = new MemoryFileSystem();
      var uut = mfs.ScopeTo(".").ScopeTo("a").ScopeTo("b").ScopeTo("c").ScopeTo("d/e/f/g");
      Assert.AreEqual("~/a/b/c/d/e/f/g", uut.VirtualAbsolutePath);
    }


    [TestMethod]
    public void ShouldReturnCorrectAbsoluteVirtualPathForPfsWithDeepFolderStructure()
    {
      var pfs = new PhysicalFileSystem();
      var uut = pfs.ScopeTo("C:\\").ScopeTo("a").ScopeTo("b").ScopeTo("c").ScopeTo("d/e/f/g");
      Assert.AreEqual("~/a/b/c/d/e/f/g", uut.VirtualAbsolutePath);
    }

    [TestMethod]
    public void ShouldReturnCorrectRelativePath()
    {
      var mfs = new MemoryFileSystem();
      var uut = mfs.ScopeTo(".").ScopeTo("test");
      Assert.AreEqual("~/test", uut.RelativePath);
    }

    [TestMethod]
    public void ShouldReturnVirtualRoot()
    {
      var mfs = new MemoryFileSystem();
      var vroot = mfs.ScopeTo(".");
      var uut = vroot.ScopeTo("test").ScopeTo("test2");
      Assert.AreEqual(vroot, vroot.VirtualRoot);
      Assert.AreEqual(vroot, uut.VirtualRoot);
    }

    [TestMethod]
    public void ShouldCreateCorrectlyFormattedAbsolutePath()
    {
      var pfs = new PhysicalFileSystem();
      var uut = pfs.ScopeTo("C:\\").ScopeTo("test").ScopeTo("test2");
      Assert.AreEqual("C:\\test\\test2\\test", uut.ToAbsolutePath("test"));
    }

    [TestMethod]
    public void ShouldScopeToAbsoluteVirtualPath2()
    {
      var pfs = new PhysicalFileSystem();
      var uut = new RelativeFileSystem(pfs, ".");
      var uut2 = uut.ScopeTo("hello");
      var uut3 = uut2.ScopeTo("muha/alter");

      Assert.AreEqual("~/hello/muha/alter/", uut3.ToVirtualAbsolutePath(""));


    }
  }
}
