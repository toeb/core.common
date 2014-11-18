using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test
{
  [TestClass]
  public class TracingTests
  {
    [TestMethod]
    public void ShouldTraceToOutputWindow() {
      var listeners = Trace.Listeners;
      Trace.WriteLine("hello output");
      Debug.WriteLine("muuh");
    }
  }
}
