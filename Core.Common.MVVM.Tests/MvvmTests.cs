using Core.Common;
using Core.Common.MVVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Core
{



  public class SamplePropertyObjectClass : BasePropertyObject
  {
    public SamplePropertyObjectClass()
    {

    }

    [StringLength(5, ErrorMessage = "Max. 5 Chars")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "only numbers allowed")]
    public string MyProperty
    {
      get { return Get<string>(); }
      set { Set(value); }
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please Specify MyFlag")]
    public string MyFlag { get { return Get<string>(); } set { Set(value); } }



  }




  [TestClass]
  public class MvvmTests
  {


    [TestMethod]
    public void TestMethod4()
    {
      var uut = new SamplePropertyObjectClass();
      uut.MyProperty = "mukkkkkkkkkk";
      Assert.IsTrue(uut.HasErrors);

      var errors = uut.GetErrors("MyProperty").Cast<string>().ToArray();
      var error = ((IDataErrorInfo)uut)["MyProperty"];
      var message = ((IDataErrorInfo)uut).Error;
      uut.MyFlag = null;
      uut.MyProperty = "555555";
      Assert.IsTrue(uut.HasErrors);
      uut.MyProperty = "55";
      Assert.IsTrue(uut.HasErrors);
      uut.MyFlag = "asd";
      Assert.IsFalse(uut.HasErrors);
    }

    [TestMethod]
    public void TestMethod3()
    {
      var uut = new SamplePropertyObjectClass();
      var ok = false;
      uut.PropertyChanged += (sender, args) => ok = true;
      uut.MyProperty = "hello";
      Assert.IsTrue(ok);
    }

    [TestMethod]
    public void TestMethod2()
    {
      var uut = new SamplePropertyObjectClass();
      uut.MyProperty = "hello";

      Assert.IsTrue(uut["MyProperty"] as string == "hello");
    }
    [TestMethod]
    public void TestMethod1()
    {
      var uut = new PropertyObject(new DefaultObjectOperations());

      uut["hello"] = 1;
      uut["byby"] = 2;

      var res = uut["hello"];

    }
  }

}
