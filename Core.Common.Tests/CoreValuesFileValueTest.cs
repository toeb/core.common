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
  public class CoreValuesFileValueTest : TypedTestBase<IFileValue<string>>
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
    protected override IFileValue<string> CreateUut()
    {

      return new DelegateFileValue<string>(FileName, stream =>
      {
        var reader = new StreamReader(stream);
        return reader.ReadToEnd();
      }, (stream, value) =>
      {
        var writer = new StreamWriter(stream);
        writer.Write(value);
      });
    }

    [TestMethod]
    public void ChangeValueFile()
    {
      string newValue = "";
      var are = new AutoResetEvent(false);
      uut.ValueChanged += (sender, args) => { newValue = uut.Value; are.Set(); };
      File.WriteAllText(FileName, "hello");
      are.WaitOne();

      Assert.AreEqual("hello", newValue);
    }

  }
}
