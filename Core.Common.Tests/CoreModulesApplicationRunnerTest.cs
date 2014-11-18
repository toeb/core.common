using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Modules.Applications;
using Core.Modules.Applications.Runner;
using System.ComponentModel.Composition;

namespace Core.Modules.ApplicationRunner.Test
{
  [Export]
  class MyApplication:ApplicationBase
  {
    MyApplication():base(typeof(MyApplication)){}
  }
  [TestClass]
  public class CoreModulesApplicationRunnerTest
  {

    [TestMethod]
    public void ActivateApplicationRunner()
    {

      var uut = Application.Create<MyApplication>();
      uut.DiscoverModules();
      IApplicationRunner controller = uut.ActivateModuleType<ApplicationRunnerModule>();
      bool stoppingEvent = false;
      bool startingEvent = false;
      controller.Stopping += () => stoppingEvent = true;
      controller.Starting += () => startingEvent = true;

      controller.Start();

      Assert.IsTrue(startingEvent);
      Assert.IsFalse(stoppingEvent);
      controller.Stop();
      Assert.IsTrue(stoppingEvent);
    }

  }
}
