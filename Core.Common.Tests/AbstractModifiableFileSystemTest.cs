using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem.Test
{

  public abstract class AbstractModifiableFileSystemTest
  {
    private IFileSystem uut;
    private IReadonlyFileSystem readonlyFs;
    [TestInitialize]
    public void InitAbstractModifiableFileSystemTest()
    {
      uut = CreateFileSystem();
      readonlyFs = CreateReadonlyFileSystem();

      try
      {
        uut.DeleteDirectory("testdir", true);

      }
      catch (Exception e)
      {
      }
      uut.CreateDirectory("testdir");

    }

    protected abstract IReadonlyFileSystem CreateReadonlyFileSystem();
    protected abstract IFileSystem CreateFileSystem();

    [TestMethod]
    public void ShouldCreateAFile()
    {
      Assert.IsFalse(readonlyFs.IsFile("testdir/filename.txt"));
      uut.CreateFile("testdir/filename.txt").Close();
      Assert.IsTrue(readonlyFs.IsFile("testdir/filename.txt"));
    }

    [TestMethod]
    public void ShouldDeleteFile()
    {
      uut.CreateFile("testdir/filename.txt").Close();
      Assert.IsTrue(readonlyFs.IsFile("testdir/filename.txt"));
      uut.Delete("testdir/filename.txt");
      Assert.IsFalse(readonlyFs.IsFile("testdir/filename.txt"));
    }

    [TestMethod]
    public void ShouldWriteFileContents()
    {
      using (var writer = new StreamWriter(uut.CreateFile("testdir/filename.txt")))
      {
        writer.Write("ShouldWriteFileContents");

      }
      var content = readonlyFs.ReadFileToEnd("testdir/filename.txt");
      Assert.AreEqual("ShouldWriteFileContents", content);
    }

    [TestMethod]
    public void ShouldCreateADirectory()
    {
      Assert.IsFalse(readonlyFs.IsDirectory("testdir/testdirect"));
      uut.CreateDirectory("testdir/testdirect");
      Assert.IsTrue(readonlyFs.IsDirectory("testdir/testdirect"));
    }

    [TestMethod]
    public void ShouldDeleteADirectory()
    {
      uut.CreateDirectory("testdir/testdirdelete");
      Assert.IsTrue(readonlyFs.IsDirectory("testdir/testdirdelete"));
      uut.DeleteDirectory("testdir/testdirdelete", true);
      Assert.IsFalse(readonlyFs.IsDirectory("testdir/testdirdelete"));
    }

    [TestMethod]
    public void ShouldWriteTextFile()
    {
      uut.WriteTextFile("test.txt", "texttext");
      Assert.IsTrue(uut.IsFile("test.txt"));
      var content = uut.ReadFileToEnd("test.txt");
      Assert.AreEqual("texttext", content);

    }

  }


}
