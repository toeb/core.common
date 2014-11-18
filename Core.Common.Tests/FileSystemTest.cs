using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using System.IO;
namespace Core.FileSystem.Test
{

  public abstract class FileSystemTest
  {
    private IFileSystem uut;
    [TestInitialize]
    public void Init()
    {
      uut = CreateFileSystem();
    }

    protected abstract IFileSystem CreateFileSystem();


    [TestMethod]
    public void ShouldCreateDirectoryAndReturnTrueIfItIsADirectory()
    {
      var dirname = "testdirectory";
      Assert.IsFalse(uut.IsDirectory(dirname));
      uut.CreateDirectory(dirname);
      Assert.IsTrue(uut.IsDirectory(dirname));
    }
    [TestMethod]
    public void ShouldCreateAFileAndReturnTrueThatItExists()
    {
      var filename = "file.txt";
      Assert.IsFalse(uut.IsFile(filename));

      uut.CreateFile(filename).Close();
      Assert.IsTrue(uut.IsFile(filename));

    }

    [TestMethod]
    public void ShouldWriteAndReadToFile()
    {
      var filename = "file.txt";
      using (var writer = new StreamWriter(uut.CreateFile(filename)))
      {
        writer.Write("hello");
      }
      using (var reader = new StreamReader(uut.OpenRead(filename)))
      {
        var text = reader.ReadToEnd();
        Assert.AreEqual("hello", text);
      }
    }
    [TestMethod]
    public void ShouldCreateSubdirectories()
    {
      uut.CreateDirectory("test1/test2/test3");
      Assert.AreEqual(1, uut.GetEntries(".").Count());
      Assert.AreEqual(1, uut.GetEntries("test1").Count());
      Assert.AreEqual(1, uut.GetEntries("test1/test2").Count());
      Assert.AreEqual(0, uut.GetEntries("test1/test2/test3").Count());
    }

    [TestMethod]
    public void ShouldCreateFolderStructure()
    {
      uut.CreateDirectory("test1");
      uut.CreateDirectory("test2");
      uut.CreateDirectory("test1/test2");
      uut.CreateDirectory("test3/test1/test2");
      uut.CreateDirectory("test3/test2");

      var entries = uut.GetEntries(".");
      Assert.AreEqual(3, entries.Count());
      Assert.IsTrue(entries.ContainsAll("~/test1", "~/test2", "~/test3"));
      Assert.AreEqual(1, uut.GetEntries("test1").Count());
      Assert.AreEqual(0, uut.GetEntries("test2").Count());
      Assert.AreEqual(2, uut.GetEntries("test3").Count());

    }
    [TestMethod]
    public void ShouldNotDeleteDirectoryIfItIsNotEmpty()
    {
      uut.CreateDirectory("test1");
      uut.CreateFile("test1/test2.txt");
      try
      {
        uut.DeleteDirectory("test1", false);
        Assert.Fail("shoul have thrown");
      }
      catch (Exception e)
      {

      }
    }
    [TestMethod]
    public void ShouldDeleteDirectory()
    {
      uut.CreateDirectory("test1");
      uut.Delete("test1");
      Assert.AreEqual(0, uut.GetEntries(".").Count());
    }
    [TestMethod]
    public void ShouldDeleteDirectoryRecursively()
    {
      uut.CreateDirectory("test1/test2");
      uut.DeleteDirectory("test1", true);
      Assert.AreEqual(0, uut.GetEntries(".").Count());
    }

    [TestMethod]
    public void ShouldCreateASimpleTextFile()
    {
      uut.WriteTextFile("test.txt", "text content");
      var content = uut.ReadTextFile("test.txt");
      Assert.AreEqual("text content", content);
    }


  }
}
