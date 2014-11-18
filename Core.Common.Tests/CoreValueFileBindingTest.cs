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
  public class CoreValueFileBindingTest :TestBase
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
    [TestMethod]
    public void Setup()
    {
      var uut =  new DelegateFileValue<string>(FileName, stream =>
      {
        var reader = new StreamReader(stream);
        return reader.ReadToEnd();
      }, (stream, value) =>
      {
        var writer = new StreamWriter(stream);
        writer.Write(value);
      });
      string val = "";
      var uut2 = new DelegateValue(()=>{return val;},(v)=>val = v as string);


    }
  }
}
