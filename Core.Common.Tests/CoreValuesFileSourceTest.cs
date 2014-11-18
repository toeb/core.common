using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.TestingUtilities;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Core.Merge;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Core.Values
{
  [TestClass]
  public class CoreValuesFileSourceTest : TypedTestBase<AbstractFileSource>
  {
    private string FileName { get { return "CoreValuesFileSourceTest.test"; } }
    protected override void RunAlways()
    {

      Action a = () =>
      {
        if (File.Exists(FileName))
          File.Delete(FileName);
      };
      a.TryRepeatException();
    }
    protected override AbstractFileSource CreateUut()
    {
      var uut = new DelegateFileSource<string>(stream =>
      {
        var reader = new StreamReader(stream);
        return reader.ReadToEnd();
      }, FileName);
      return uut;
    }
    [TestMethod]
    public void CreateFileSource()
    {

      Assert.AreEqual(Path.GetFullPath(FileName), uut.Path);
      Assert.IsTrue(uut.WatchingEnabled);
      Assert.IsNull(uut.Value);
    }
    [TestMethod]
    public void FileCreatedValueChanged()
    {
      var are = new AutoResetEvent(false);
      uut.ValueChanged += (sender, args) => { are.Set(); };

      File.Create(FileName).Close();

      var result = are.WaitOne(1000);
      Assert.IsTrue(result);

    }
    [TestMethod]
    public void FileChangedValueChanged()
    {

      File.Create(FileName).Close();
      var are = new AutoResetEvent(false);

      uut.ValueChanged += (sender, args) => { are.Set(); };

      File.WriteAllText(FileName, "hello");

      var result = are.WaitOne(1000);
      Assert.IsTrue(result);

    }


    [TestMethod]
    public void FileDeletedValueChanged()
    {

      File.Create(FileName).Close();
      var are = new AutoResetEvent(false);

      uut.ValueChanged += (sender, args) => { are.Set(); };

      Action a = () => File.Delete(FileName);
      a.TryRepeatException();

      var result = are.WaitOne(1000);
      Assert.IsTrue(result);

    }

    [TestMethod]
    public void GetFileValue()
    {
      File.WriteAllText(FileName, "hello world");
      Assert.AreEqual("hello world", uut.Value);
    }
  }


  





}
