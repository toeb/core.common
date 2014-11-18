using Core.TestingUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace Core.Test
{
  [TestClass]
  public class CoreWatchableFileTest : TypedTestBase<WatchableFile>
  {

    protected override WatchableFile CreateUut()
    {
      return new WatchableFile();
    }

    private string FileName { get { return "test.file"; } }
    protected override void RunAlways()
    {
      Action a = () =>
      {
        if (File.Exists(FileName))
          File.Delete(FileName);
      };
      a.TryRepeatException();
    }

    [TestMethod]
    public void CreateWatchableFile()
    {
      uut.Path = FileName;
      Assert.AreEqual(Path.GetFullPath(FileName), uut.Path);
      Assert.IsTrue(!File.Exists(FileName));
      Assert.IsTrue(uut.EventsEnabled);
    }
    [TestMethod]
    public void StartWatching()
    {
      bool called = false;
      uut.FileChanged += (sender) => { called = true; };
      uut.FileCreated += (sender) => { called = true; };
      uut.FileDeleted += (sender) => { called = true; };
      uut.Path = FileName;
      Assert.IsFalse(called);

    }
    [TestMethod]
    public void WatchFileChange()
    {
      var stream = File.Create(FileName);
      stream.Close();
      stream.Dispose();
      int changed = 0, created = 0, deleted = 0;
      var are = new AutoResetEvent(false);
      uut.FileChanged += sender => { changed++; are.Set(); };
      uut.FileCreated += sender => { created++; are.Set(); };
      uut.FileDeleted += sender => { deleted++; are.Set(); };

      uut.Path = FileName;

      File.WriteAllText(FileName, "hello");

      are.WaitOne(1000);

      Assert.AreEqual(0, created);
      Assert.IsTrue(1<= changed);
      Assert.AreEqual(0, deleted);
    }



    [TestMethod]
    public void WatchFileCreation()
    {
      int changed = 0, created = 0, deleted = 0;
      var are = new AutoResetEvent(false);
      uut.FileChanged += sender => { changed++; are.Set(); };
      uut.FileCreated += sender => { created++; are.Set(); };
      uut.FileDeleted += sender => { deleted++; are.Set(); };

      uut.Path = FileName;

      var stream = File.Create(FileName);
      stream.Close();
      stream.Dispose();


      are.WaitOne(1000);

      Assert.IsTrue(1 <= created);
      Assert.AreEqual(0, changed);
      Assert.AreEqual(0, deleted);
    }


    [TestMethod]
    public void WatchFileDeleted()
    {
      int changed = 0, created = 0, deleted = 0;
      var stream = File.Create(FileName);
      stream.Close();
      stream.Dispose();
      var are = new AutoResetEvent(false);
      uut.FileChanged += sender => { changed++; are.Set(); };
      uut.FileCreated += sender => { created++; are.Set(); };
      uut.FileDeleted += sender => { deleted++; are.Set(); };

      uut.Path = FileName;

      File.Delete(FileName);

       are.WaitOne(1000);

      Assert.AreEqual(0, created);
      Assert.AreEqual(0, changed);
      Assert.AreEqual(1, deleted);
    }
  }
}
