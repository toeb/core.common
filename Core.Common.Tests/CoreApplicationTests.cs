using Core.FileSystem;
using Core.TestingUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Extensions;
using System.Threading;
using System.ComponentModel.Composition.Hosting;
using Core.Modules;
using Core.Modules.Applications;
namespace Core.Applications.Test
{
  [Module]
  class DeactivationErrorModule : IModule
  {
    public void Activate()
    {
    }

    public void Deactivate()
    {
      throw new Exception();
    }
  }

  [Module]
  class ActivationErrorModule : IModule
  {
    public void Activate()
    {
      throw new Exception();

    }


    public void Deactivate()
    {
    }
  }

  [TestClass]
  public class CoreModulesApplicationsTests : TestBase
  {
    [TestMethod]
    public void ActivateException()
    {
      var uut = Application.Create<MyApplication>();
      uut.AddModule(typeof(ActivationErrorModule));
      var instance = uut.GetModule(typeof(ActivationErrorModule));
      try
      {
        uut.ActivateModule(instance);
        Assert.Fail();
      }
      catch
      {
        Assert.IsFalse(instance.IsActive);
        Assert.IsNull(instance.Module);
      }
    }
    [TestMethod]
    public void DeactivateException()
    {
      var uut = Application.Create<MyApplication>();
      var module = uut.ActivateModuleType(typeof(DeactivationErrorModule));

      try
      {
        uut.DeactivateModule(module);
        Assert.Fail();
      }
      catch
      {
        Assert.IsTrue(module.IsActive);
        Assert.IsNotNull(module.Module);
      }
    }
    [TestMethod]
    public void ApplicationContainerIsAvailable()
    {
      var uut = Application.Create<MyApplication>();
      var container = uut
        .Container
        .GetExportedValue<CompositionContainer>("ApplicationContainer");

      Assert.AreEqual(container, uut.Container);
    }

    protected override void RunAlways()
    {
      var fs = Composition.RootContainer.GetExport<PhysicalFileSystem>().Value;
      var path = "TestAppDir";
      if (fs.Exists(path))
      {
        fs.DeleteDirectory(path, true);
      }

      fs.TryCreateDirectory(path);


    }
    [TestMethod]
    public void ApplicationExportsItself()
    {
      var uut = Application.Create<MyApplication>();

      var app1 = uut.Container.GetExportedValue<IApplication>();
      var app2 = uut.Container.GetExportedValue<IApplication>("Application");

      Assert.AreEqual(uut, app1);
      Assert.AreEqual(uut, app2);
    }


    [TestMethod]
    public void CreateApplication()
    {
      var uut = Application.Create<MyApplication>();
      Assert.AreEqual(0, uut.Modules.Count());
      Assert.IsNotNull(uut.ApplicationInfo);
      Assert.AreNotEqual(Guid.Empty, uut.ApplicationInfo.Id);
      Assert.IsNotNull(uut.ApplicationInfo.ApplicationVersion);
      Assert.AreEqual(uut.GetType().FullName, uut.ApplicationInfo.ApplicationName);
    }

    [TestMethod]
    public void DiscoverModules()
    {
      var uut = Application.Create<MyApplication>();
      uut.DiscoverModules();
      Assert.IsTrue(uut.Modules.Count() > 0);
      Assert.IsTrue(uut.Modules.Select(instance => instance.ModuleInfo.Type).ContainsAll(typeof(TestModule1), typeof(TestModule2)));
    }


    [TestMethod]
    public void ActivateModule()
    {
      var uut = Application.Create<MyApplication>();
      uut.DiscoverModules();
      Assert.IsTrue(!uut.Modules.Any(module => module.IsActive));

      var m = uut.GetModule(typeof(TestModule1));
      uut.ActivateModule(m);
      Assert.IsTrue(m.IsActive);
      Assert.IsNotNull(m.Module);

    }

    [TestMethod]
    public void DeactivateModule()
    {
      var uut = Application.Create<MyApplication>();
      uut.DiscoverModules();
      var m = uut.GetModule(typeof(TestModule1));
      uut.ActivateModule(m);
      uut.DeactivateModule(m);
      Assert.AreEqual(0, uut.Modules.Count(module => module.IsActive));
      Assert.IsFalse(m.IsActive);
      Assert.IsNull(m.Module);
    }

    [TestMethod]
    public void ModuleInstanceIsCorrect()
    {
      var uut = Application.Create<MyApplication>();
      uut.DiscoverModules();
      var module = uut.GetModule(typeof(MyModule1));
      uut.ActivateModule(module);
      Assert.IsNotNull(module.Module);
      Assert.AreEqual(typeof(MyModule1), module.Module.GetType());

    }


    [TestMethod]
    public void ActivatedModuleExportsToApplication()
    {
      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module1 = uut.GetModule(typeof(MyModule1));
      Assert.AreEqual(0, uut.Container.GetExports<string>("Export1").Count());
      uut.ActivateModule(module1);
      Assert.AreEqual(1, uut.Container.GetExports<string>("Export1").Count());

    }


    [TestMethod]
    public void DeactivatedModuleExportsRemoved()
    {
      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module1 = uut.GetModule(typeof(MyModule1));
      uut.ActivateModule(module1);
      uut.DeactivateModule(module1);
      Assert.AreEqual(0, uut.Container.GetExports<string>("Export1").Count());

    }
    [TestMethod]
    public void RemovingModuleRemovesRecomposableExportsFromDependentModules()
    {
      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module1 = uut.FindModuleByPartialName("MyModule1");
      var module2 = uut.FindModuleByPartialName("MyModule2");

      uut.ActivateModule(module1);
      uut.ActivateModule(module2);
      var m2 = module2.Module as MyModule2;

      Assert.AreEqual("hello", m2.Val);
      uut.DeactivateModule(module1);
      Assert.AreEqual(null, m2.Val);

    }
    /*
    [TestMethod]
    public void RemoveModuleWithNeededExport()
    {
      Assert.Inconclusive("currently not supported because needed imports do not work");

      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module1 = uut.FindModuleByPartialName("MyModule1");
      var module3 = uut.FindModuleByPartialName("MyModule3");

      uut.ActivateModule(module1);
      uut.ActivateModule(module3);
      var m3 = module3.Module as MyModule3;
      Assert.AreEqual("hello", m3.Val);
      uut.DeactivateModule(module1);


      Assert.AreEqual(null, m3.Val);
    }
    */

    [TestMethod]
    public void ModuleIsNotInstanciatedBeforeActivation()
    {
      MyModule1.disposeCount = 0;
      MyModule1.instanceCount = 0;

      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module1 = uut.FindModuleByPartialName("MyModule1");
      Assert.AreEqual(0, MyModule1.instanceCount);
      Assert.AreEqual(0, MyModule1.disposeCount);
    }



    [TestMethod]
    public void ModuleIsInstanciatedOnceAfterActivation()
    {
      MyModule1.disposeCount = 0;
      MyModule1.instanceCount = 0;

      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module1 = uut.GetModule(typeof(MyModule1));
      uut.ActivateModule(module1);
      Assert.AreEqual(1, MyModule1.instanceCount);
      Assert.AreEqual(0, MyModule1.disposeCount);
    }




    [TestMethod]
    public void ModueIsDisposedAfterDeactivation()
    {
      // Assert.Inconclusive();
      MyModule1.disposeCount = 0;
      MyModule1.instanceCount = 0;

      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module1 = uut.FindModuleByPartialName("MyModule1");
      uut.ActivateModule(module1);
      uut.DeactivateModule(module1);

      Assert.AreEqual(1, MyModule1.disposeCount);
    }


    [TestMethod]
    public void ModuleActivateCallbackIsCalledNonInherited()
    {
      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();

      var module = uut.FindModuleByPartialName("NonInheritedModule");

      uut.ActivateModule(module);

      var instance = module.Module as NonInheritedModule;
      Assert.IsTrue(instance.activateCalled);
      Assert.IsFalse(instance.deactivateCalled);

    }

    [TestMethod]
    public void ModuleActivateCallbackIsCalledInherited()
    {
      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();

      var module = uut.FindModuleByPartialName("OnlyInheritedModule");

      uut.ActivateModule(module);

      var instance = module.Module as OnlyInheritedModule;
      Assert.IsTrue(instance.activateCalled);
      Assert.IsFalse(instance.deactivateCalled);

    }


    [TestMethod]
    public void ModuleDeactivateCallbackIsCalledNonInherited()
    {

      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();

      var module = uut.FindModuleByPartialName("NonInheritedModule");

      uut.ActivateModule(module);
      var instance = module.Module as NonInheritedModule;
      uut.DeactivateModule(module);
      Assert.IsTrue(instance.activateCalled);
      Assert.IsTrue(instance.deactivateCalled);
    }


    [TestMethod]
    public void NonInheritedModuleIsFound()
    {

      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module = uut.FindModuleByPartialName("NonInheritedModule");
      Assert.AreEqual(typeof(NonInheritedModule), module.ModuleInfo.Type);
    }

    [TestMethod]
    public void InheritedModuleIsFound()
    {

      var uut = Application.Create<MyApp2>();
      uut.DiscoverModules();
      var module = uut.FindModuleByPartialName("OnlyInheritedModule");
      Assert.AreEqual(typeof(OnlyInheritedModule), module.ModuleInfo.Type);
    }


    [TestMethod]
    public void ActivationCallbackOnlyCalledOncePerModule()
    {
      var uut = Application.Create<ModuleApplication>();
      var m1 = uut.ActivateModuleType<MultiActivationCallback1>();
      Assert.AreEqual(1, m1.activated);
      var m2 = uut.ActivateModuleType<MultiActivationCallback2>();
      Assert.AreEqual(1, m1.activated);
      Assert.AreEqual(1, m2.activated);
    }
    [TestMethod]
    public void DeactivationCallbackOnlyCalledOncePerModule()
    {
      var uut = Application.Create<ModuleApplication>();
      var m1 = uut.ActivateModuleType<MultiActivationCallback1>();
      var m2 = uut.ActivateModuleType<MultiActivationCallback2>();

      var mi1 = uut.GetModule(typeof(MultiActivationCallback1));
      var mi2 = uut.GetModule(typeof(MultiActivationCallback2));

      uut.DeactivateModule(mi1);

      Assert.AreEqual(1, m1.deactivated);
      Assert.AreEqual(0, m2.deactivated);

      uut.DeactivateModule(mi2);
      Assert.AreEqual(1, m1.deactivated);
      Assert.AreEqual(1, m2.deactivated);
    }
    [TestMethod]
    public void ModuleInstanceIsSet()
    {
      var uut = Application.Create();
      var mod = uut.ActivateModuleType<ModuleInstanceModule>();
      Assert.IsNotNull(mod.Instance);
      Assert.AreEqual(mod.Instance, uut.GetModule(typeof(ModuleInstanceModule)));

    }
    [TestMethod]
    public void ModuleInstanceIsUnSet()
    {
      var uut = Application.Create();
      var mod = uut.ActivateModuleType<ModuleInstanceModule>();
      uut.DeactivateModule(mod.Instance);
      Assert.IsNull(mod.Instance);

    }

    [TestMethod]
    public void ModuleContainerIsSet()
    {

      var uut = Application.Create();
      var mod = uut.ActivateModuleType<ModuleInstanceModule>();
      Assert.IsNotNull(mod.Container);

    }
    [TestMethod]
    public void ModuleContainerIsUnset()
    {

      var uut = Application.Create();
      var mod = uut.ActivateModuleType<ModuleInstanceModule>();
      uut.DeactivateModule(mod.Instance);
      Assert.IsNull(mod.Container);

    }
  }



  [Module]
  class ModuleInstanceModule
  {
    [ModuleContainer]
   public CompositionContainer Container { get; set; }
    [ModuleInstance]
    public IModuleInstance Instance { get; set; }
  }

  [Module]
  class MultiActivationCallback1
  {
    public int deactivated = 0;
    public int activated = 0;
    [ActivationCallback]
    void t1() { activated++; }
    [DeactivationCallback]
    void t2() { deactivated++; }
  }
  [Module]
  class MultiActivationCallback2
  {
    public int deactivated=0;
    public int activated = 0;
    [ActivationCallback]
    void t1() { activated++; }
    [DeactivationCallback]
    void t2() { deactivated++; }
  }

  [Module]
  class NonInheritedModule
  {
    public bool activateCalled = false;
    public bool deactivateCalled = false;
    [ActivationCallback]
    void activate() { activateCalled = true; }


    [DeactivationCallback]
    void deactivate() { deactivateCalled = true; }
  }
  [Module]
  public class OnlyInheritedModule : IModule
  {


    public bool activateCalled = false;
    public bool deactivateCalled = false;
    public void Activate()
    {
      activateCalled = true;
    }

    public void Deactivate()
    {
      deactivateCalled = true;
    }
  }

  [Module]
  class MyModule1 : IDisposable
  {


    public static int disposeCount = 0;
    public void Dispose()
    {
      disposeCount++;
    }
    public static int instanceCount = 0;
    ~MyModule1() { instanceCount--; }
    MyModule1() { instanceCount++; }
    [Export("Export1")]
    string Val { get { return "hello"; } }
  }

  [Module]
  class MyModule2
  {

    string v = "";
    [Import("Export1", AllowDefault = true, AllowRecomposition = true)]
    public string Val
    {
      get { return v; }
      set
      {
        v = value;

      }
    }

  }


  [Export]
  class MyApplication : ApplicationBase
  {


    public MyApplication() : base(typeof(MyApplication)) { }
  }

  [Module]
  class TestModule1
  {


  }
  [Module]
  class TestModule2
  {


  }

  [Module]
  class MyModule3
  {

    string v = "";
    //[Import("Export3")]  //currently an error beause application cannot instanciate without dependency
    public string Val
    {
      get { return v; }
      set
      {
        v = value;

      }
    }

  }

  [Export]
  class MyApp2 : ApplicationBase
  {

    MyApp2() : base(typeof(MyApp2)) { }
  }
}
