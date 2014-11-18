using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
  }
}
