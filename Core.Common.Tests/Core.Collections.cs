using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Core.Collections;
using System.Linq;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace Core.Collections
{
  [TestClass]
  public class CoreCollectionsEditableView
  {

    [TestMethod]
    public void AddSuccess()
    {
      ICollection<int> items = new List<int>() { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      var view = items.EditableView(i => i < 5);
      Assert.IsTrue(view.Add(0));
      Assert.IsTrue(items.Contains(0));
    }
    [TestMethod]
    public void AddFail1()
    {
      ICollection<int> items = new List<int>() { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      var view = items.EditableView(i => i < 5);
      Assert.IsFalse(view.Add(15));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddFail2()
    {
      ICollection<int> items = new List<int>() { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      var view = (ICollection<int>)items.EditableView(i => i < 5);
      view.Add(52);
    }


  }
}
