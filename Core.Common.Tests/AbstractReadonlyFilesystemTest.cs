using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.FileSystem.Test
{
  public abstract class AbstractReadonlyFileSystemTest
  {
    protected IReadonlyFileSystem uut;
    [TestInitialize]
    public void InitReadonlyFileSystemTest()
    {
      uut = CreateReadonlyFileSystem();
      try
      {
        DeleteDir("testdir");
      }
      catch (Exception ) { }
      CreateDir("testdir");

    }
    protected abstract void CreateFile(string path, string content);
    protected abstract void DeleteDir(string path);
    protected abstract void CreateDir(string path);

    protected abstract IReadonlyFileSystem CreateReadonlyFileSystem();


    [TestMethod]
    public void ShouldReturnTrueIfFileExists()
    {
      CreateFile("testdir/file.txt", "test");
      var result = uut.IsFile("testdir/file.txt");
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ShouldReturnFalseIfFileDoesNotExist()
    {
      var result = uut.IsFile("testdir/file.txt");
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ShouldReturnTruefIfDirectoryExists()
    {
      CreateDir("testdir/testdir2");
      var result = uut.IsDirectory("testdir/testdir2");
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ShouldReturnFalseIfDirectoryDoesNotExist()
    {
      var result = uut.IsDirectory("testdir/testdir2");

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ShouldReturnFalseIfPathExistsButIsNotADrectory()
    {
      CreateFile("testdir/test", "");
      var result = uut.IsDirectory("testdir/test");
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ShouldReturnFalseIfPathExistsButIsNotAFile()
    {
      CreateDir("testdir/test");
      var result = uut.IsFile("testdir/test");
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ShouldReturnAllFilesystemEntries()
    {
      CreateDir("testdir/testdirect");
      CreateFile("testdir/testfile.txt", "");
      var entries = uut.GetEntries("testdir");
      Assert.IsNotNull(entries);
      Assert.AreEqual(2, entries.Count());
      Assert.IsTrue(entries.Any(entry => entry.Contains("testdirect")));
      Assert.IsTrue(entries.Any(entry => entry.Contains("testfile.txt")));
    }

    [TestMethod]
    public void ShouldOpenFileToReadCorrectly()
    {
      CreateFile("testdir/testfile.txt", "ShouldOpenFileToReadCorrectly");

      using (var reader = new StreamReader(uut.OpenRead("testdir/testfile.txt")))
      {
        var result = reader.ReadToEnd();
        Assert.AreEqual("ShouldOpenFileToReadCorrectly", result);
      }
    }
    public abstract string NormalizePathForFs(string path);
    [TestMethod]
    public virtual void ShouldNormalizePathCorrectly()
    {
      var path = "unnormalized/path";
      var expected = NormalizePathForFs(path);
      var actual = uut.NormalizePath(path);
      Assert.AreEqual(expected, actual);

    }

    [TestMethod]
    public virtual void ShouldReturnCacheKey()
    {
      CreateFile("somefile.txt", "content");
      var key = uut.GetCacheKey("somefile.txt");
      Assert.IsFalse(string.IsNullOrEmpty(key));
      
    }
    [TestMethod]
    public virtual void CacheKeyShouldChangeWhenFileChanges()
    {
      CreateFile("somefile.txt", "content");
      var key = uut.GetCacheKey("somefile.txt");
      Thread.Sleep(19);
      CreateFile("somefile.txt", "content2");
      var newKey = uut.GetCacheKey("somefile.txt");
      Assert.AreNotEqual(key, newKey);
    }
  }



}
