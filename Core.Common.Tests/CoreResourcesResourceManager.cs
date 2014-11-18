using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.TestingUtilities;
using System.Reflection;
using System.Linq;
using Core.Extensions;
using System.IO;
namespace Core.Resources.Test
{

  //[TestClass]
  //public class CoreResourcesResourceManager : TypedTestBase<IAssemblyResourceService>
  //{
  //  private Assembly TestAssembly { get { return typeof(Playground.Resources.TestAssembly.ProjectInfo).Assembly; } }

  //  [TestMethod]
  //  public void CreateResourceManager()
  //  {
  //    Assert.IsNotNull(uut);
  //  }
  //  [TestMethod]
  //  public void GetResourceManager()
  //  {

  //    var resources = uut.GetResources(TestAssembly);
  //    Assert.IsNotNull(resources);
  //    Assert.AreEqual(TestAssembly, resources.Assembly);
  //  }

  //  [TestMethod]
  //  public void GetResourceCount()
  //  {
  //    var resources = uut.GetResources(TestAssembly);
  //    var count = resources.Resources.Count();
  //    Assert.IsTrue( count > 0);
  //  }
  //  [TestMethod]
  //  public void CheckFileResources()
  //  {
  //    var resources = uut.GetResources(TestAssembly);
  //    var names = resources.Resources.OfType<IFileResource>().Select(fr => fr.FileName.ToLower()).ToArray();
  //    Assert.IsTrue(names.ContainsAll("embeddedresource.html", "normalresource.html"));
  //  }
  //  [TestMethod]
  //  public void CheckStringResource()
  //  {
  //    var resources = uut.GetResources(TestAssembly);
  //    var values = resources.Resources.OfType<IStringResource>().Select(sr=>sr.Value).ToArray();
  //    Assert.IsTrue(values.ContainsAll("Hello", "Bye"));
  //  }

  //  [TestMethod]
  //  public void CheckLocalPath()
  //  {
  //    var resources = uut.GetResources(TestAssembly);

  //    var files = resources.Resources
  //      .OfType<IFileResource>()
  //      .Where(file => file.LocalPath != null)
  //      .Select(file=> file.LocalPath);

  //    foreach (var file in files) {
  //      Assert.IsTrue(File.Exists(file));
  //    }

  //  }
  //  [TestMethod]
  //  public void FileContentEqual()
  //  {

  //    var resources = uut.GetResources(TestAssembly);

  //    var files = resources.Resources
  //      .OfType<IFileResource>()
  //      .Where(file => file.LocalPath != null);

  //    foreach (var file in files)
  //    {
  //      var resourceContent = new StreamReader(file.OpenStream()).ReadToEnd();
  //      var fileContent = File.ReadAllText(file.LocalPath);
  //      Assert.AreEqual(fileContent, resourceContent);
  //    }
  //  }

  //  [TestMethod]
  //  public void IgnoreFileExtensionsPeriods()
  //  {

  //    var resources = uut.GetResources(TestAssembly);
  //    var files = resources.Resources
  //      .OfType<IFileResource>()
  //      .Where(file => file.LocalPath != null);

  //    files.Single(f => f.FileName == "test.min.js");
  //    files.Single(f => f.FileName == "test.min.js.map");
      
  //  }



  //}
}
