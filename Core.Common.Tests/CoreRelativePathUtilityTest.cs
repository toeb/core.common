using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.TestingUtilities;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Core.FileSystem.Test
{
  [TestClass]
  public class CoreRelativePathUtilityTest
  {
    [TestMethod]
    public void ShouldReturnTrueIfPathIsRelative()
    {

    }

    [TestMethod]
    public void ShouldReturnTrueIfPathIsValid()
    {

    }

    [TestMethod]
    public void ShouldNormalizePath()
    {
      Assert.AreEqual("~/", RelativePathUtility.Normalize(""));
      Assert.AreEqual("~/", RelativePathUtility.Normalize("."));
      Assert.AreEqual("~/", RelativePathUtility.Normalize("./"));
      Assert.AreEqual("~/", RelativePathUtility.Normalize("/"));
      Assert.AreEqual("~/", RelativePathUtility.Normalize("~/"));
      Assert.AreEqual("~/test", RelativePathUtility.Normalize("test"));
      Assert.AreEqual("~/test", RelativePathUtility.Normalize("./test"));
      Assert.AreEqual("~/test/", RelativePathUtility.Normalize("./test/"));
      Assert.AreEqual("~/test/test2", RelativePathUtility.Normalize("test/test2"));

    }
  }
}
