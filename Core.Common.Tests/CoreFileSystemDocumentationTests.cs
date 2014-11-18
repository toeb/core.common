using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.TestingUtilities;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Core.FileSystem.Test
{
  [TestClass]
  public class CoreFileSystemDocumentationTests
  {
    [TestMethod]
    public void WritingAFile()
    {
      // create  a physical file system
      // (works just like the static System.IO classes)
      IFileSystem fs = new PhysicalFileSystem();
      using (var writer = new StreamWriter(fs.CreateFile("test.txt")))
      {
        writer.Write("hello!");
      }
      // file was created in current working directory!
      Assert.IsTrue(File.Exists("test.txt") && fs.Exists("test.txt"));
      // file contains the content
      Assert.AreEqual("hello!", File.ReadAllText("test.txt"));
    }

    [TestMethod]
    public void GettingARelativeView()
    {
      // create a physical file system
      IFileSystem fs = new PhysicalFileSystem()
        .ScopeTo("example_dir");// scope to a specific directory

      // directory contains:
      // subdir/file3.txt
      // file2.txt
      // file1.txt

      // return all entries of relative path "."
      var entries = fs.GetEntries(".");

      // all entries are returned as relative paths
      Assert.IsTrue(entries.Contains("~/file1.txt"));
      Assert.IsTrue(entries.Contains("~/file2.txt"));
      Assert.IsTrue(entries.Contains("~/subdir"));

    }

    [TestMethod]
    public void UsingAnInMemoryFileSystem()
    {
      // you can also use a in memory filesystem
      // which is usefull for caching and unit testing
      IFileSystem fs = new MemoryFileSystem();

      fs.WriteTextFile("test.txt", "content");

      Assert.IsTrue(fs.Exists("test.txt"));
      Assert.AreEqual("content", fs.ReadFileToEnd("test.txt"));
    }

    [TestMethod]
    public void UsingACompositeFileSystem()
    {
      // combine multiple filesystems?
      // not a problem!
      var cfs = new CompositeFileSystem();
      var pfs = new PhysicalFileSystem();
      var mfs = new MemoryFileSystem();
      mfs.TouchFile("file4.txt");
      // search order is order in which the filesystems are
      // added - so if example_dir/subdir contains a file with the same
      // filename as example_dir the file in example_dir will
      // be preferred
      cfs.AddFileSystem(pfs.ScopeTo("example_dir"));
      cfs.AddFileSystem(pfs.ScopeTo("example_dir/subdir"));
      cfs.AddFileSystem(mfs);

      var entries = cfs.GetEntries(".");
      Assert.IsTrue(entries.Contains("~/subdir"));
      Assert.IsTrue(entries.Contains("~/file1.txt"));
      Assert.IsTrue(entries.Contains("~/file2.txt"));
      Assert.IsTrue(entries.Contains("~/file3.txt"));
      Assert.IsTrue(entries.Contains("~/file4.txt"));

    }



  }
}
