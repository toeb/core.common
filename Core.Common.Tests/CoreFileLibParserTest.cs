using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Extensions;
namespace Core.Test
{

  [TestClass]
  public class CoreFileLibParserTest
  {

    [TestMethod]
    public void ShouldNormalizeFileLibName()
    {
      var lib = FileLib.Parse("JqUery-1.0.3.13.Min.IntelliSense.JS", true);
      Assert.AreEqual("jquery", lib.LibName);
      Assert.IsTrue(lib.Tags.ContainsAll("min", "intellisense"));
      Assert.AreEqual("js", lib.Extension);
    }

    [TestMethod]
    public void ShouldMatchFileLib()
    {
      var lib = FileLib.Parse("jquery-1.0.313.131.min.js");
      Assert.AreEqual("jquery", lib.LibName);
      Assert.AreEqual(new Version(1, 0, 313, 131), lib.GetVersion());
      Assert.IsTrue(lib.Tags.ContainsAll("min"));
      Assert.IsFalse(lib.Tags.Contains("js"));
      Assert.AreEqual("js", lib.Extension);
    }
    [TestMethod]
    public void ShouldMatchMultipleTags()
    {
      var lib = FileLib.Parse("lib.tag1.tag2.tag3.ext");
      Assert.AreEqual("lib", lib.LibName);
      Assert.AreEqual("ext", lib.Extension);
      Assert.IsTrue(lib.Tags.ContainsAll("tag1", "tag2", "tag3"));
    }
    [TestMethod]
    public void ShouldMatchSimpleFileLib()
    {
      var lib = FileLib.Parse("jquery-1.0.min.js");
      Assert.AreEqual("jquery", lib.LibName);
      Assert.AreEqual("1", lib.Major);
      Assert.AreEqual("0", lib.Minor);
      Assert.AreEqual("", lib.Revision);
      Assert.AreEqual("", lib.Build);
      Assert.IsTrue(lib.Tags.Contains("min"));
      Assert.AreEqual("js", lib.Extension);
    }
    [TestMethod]
    public void ShouldMatchSimpelFile()
    {
      string fn = "jquery.js";
      var lib = FileLib.Parse(fn);
      Assert.AreEqual("jquery", lib.LibName);
      Assert.AreEqual("js", lib.Extension);
      Assert.AreEqual(0, lib.Tags.Count());
      Assert.AreEqual(null, lib.GetVersion());

    }
  }
}
