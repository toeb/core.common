using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects.Test
{
  [TestClass]
  public class CoreManagedObjectsMergeTests
  {
    [TestMethod]
    public void CreateMissingPropertiesMergeStrategy()
    {
      var uut = ManagedObjectMergeStrategies.CreateMissing;
      var a = ManagedObject.Extensible(new { a = 4, b = "st" });
      var b = ManagedObject.Extensible();

      Assert.IsTrue(uut.CanMerge(a, b));
      var result = uut.Merge(a, b);
      Assert.AreEqual(b, result);
      Assert.AreEqual(2, b.Properties.Count());
      Assert.AreEqual(4, b.GetProperty("a").Value);
      Assert.AreEqual("st", b.GetProperty("b").Value);
    }

    [TestMethod]
    public void OverwriteExistingPropertiesMergeStrategy()
    {
      var uut = ManagedObjectMergeStrategies.OverwriteExisting;

      var a = ManagedObject.Extensible(new { a = 4, b = "st" });
      var b = ManagedObject.Extensible();
      b.RequireProperty("a");

      Assert.IsTrue(uut.CanMerge(a, b));
      var result = uut.Merge(a, b);
      Assert.AreEqual(b, result);
      Assert.AreEqual(1, b.Properties.Count());
      Assert.AreEqual(4, b.GetProperty("a").Value);
      Assert.IsFalse(b.HasProperty("b"));
    }
  }
}
