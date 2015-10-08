using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Reflect;
using Core.Graph;
namespace Core.Test
{



  [TestClass]
  public class CoreReflectionExtensionsTest
  {

    class A { }
    class B : A { }
    class F : A { }
    class G : F{}
    class C : B { }
    class D : C { }
    class E { }
    [TestMethod]
    public void CommonAncestor()
    {
      Assert.AreEqual(typeof(object), typeof(A).GetCommonAncestorWith(typeof(E)));
      Assert.AreEqual(typeof(A), typeof(D).GetCommonAncestorWith(typeof(F)));
      Assert.AreEqual(typeof(B), typeof(B).GetCommonAncestorWith(typeof(C)));
     
    }


    [TestMethod]
    [TestCategory("Reflection")]
    public void ImplementedInterfaces()
    {
      Assert.AreEqual(typeof(T1).GetTypeSpecificInterfaces().Count(), 0);
      Assert.AreEqual(typeof(T2).GetTypeSpecificInterfaces().Count(), 2);
      Assert.AreEqual(typeof(T4).GetTypeSpecificInterfaces().Count(), 1);

    }

    [TestMethod]
    [TestCategory("Reflection")]
    public void GetBaseTypesInDependencyOrder()
    {

      var rsult = new[] { typeof(T1), typeof(T4),typeof(T2) }.DfsOrder(t => t.GetDirectBaseTypes()).ToArray(); 

    }

  }




  interface T1
  {

  }
  interface T2 : T3, T1
  {

  }
  interface T3
  {

  }
  interface T4 : T2, T3
  {

  }


  public static class Impl
  {
    
  }
}
