using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TestingUtilities
{


  [TestClass]
  public abstract class TestBase
  {
    private static bool initialized = false;


    [TestInitialize]
    public void InitTestBase()
    {
      container = null;
      if (!initialized)
      {

        initialized = true;
        Composition.InitializeDefault();
        RunOnce();
      }
      RunAlways();
    }

    /// <summary>
    /// the composition container for the current test
    /// </summary>
    private CompositionContainer container;
    protected CompositionContainer Container { get { return container ?? Composition.RootContainer; } set { container = value; } }

    protected T GetUut<T>(string contract = null)
    {
      if (contract == null) return Container.GetExportedValue<T>();
      return Container.GetExportedValue<T>(contract);
    }


    protected virtual void RunOnce() { }
    protected virtual void RunAlways() { }

  }
}
