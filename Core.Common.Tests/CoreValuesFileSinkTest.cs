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
  public class CoreValuesFileSinkTest : TypedTestBase<AbstractFileSink>
  {
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


    protected override AbstractFileSink CreateUut()
    {
      return new DelegateFileSink<string>(FileName, (value, stream) =>
      {
        var writer = new StreamWriter(stream);
        writer.Write(value);
        writer.Flush();
      });
    }


    [TestMethod]
    public void Create()
    {
      Assert.AreEqual(Path.GetFullPath(FileName), uut.Path);
    }

    [TestMethod]
    public void WriteValue()
    {
      uut.Value = "hello world";
      var result =  DelegateExtensions.TryRepeatException(() => File.ReadAllText(FileName));
      Assert.AreEqual("hello world", result);
    }


  }
}
