using Core.Modules.Applications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using Core.TestingUtilities;
using Core.FileSystem;
using System.IO;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;
namespace Core.Modules.Installation.Tests
{

  [TestClass]
  public class CoreModulesInstallationTests : TypedTestBase<InstallationModule>
  {
    ModuleApplication app;
    protected override InstallationModule CreateUut()
    {
      if (Directory.Exists("test")) Directory.Delete("test", true);
      app = Application.Create<ModuleApplication>();

      app.Import<IRelativeFileSystem>(new RelativeFileSystem("test"), InstallationContracts.InstallationFileSystem);
      var uut = app.ActivateModuleType<InstallationModule>();
      return uut;
    }
    [TestMethod]
    public void CreateInstallationModule()
    {
      Assert.AreEqual(0, uut.Installables.Count());
    }
    [TestMethod]
    public void AddInstallableModule()
    {
      app.ActivateModuleType<Installable1>();
      Assert.AreEqual(1, uut.Installables.Count());
    }

    [TestMethod]
    public void InstallableInstanceIsCreate()
    {
      var installable = app.ActivateModuleType<Installable1>();
      var ii = uut.GetInstallation(installable);
      Assert.IsNotNull(ii);
      Assert.AreEqual(installable, ii.Installable);
      Assert.IsFalse(ii.IsInstalled);
      Assert.IsNull(ii.InstallationFileSystem);
    }

    [TestMethod]
    public void Installation()
    {
      var installable = app.ActivateModuleType<Installable1>();
      var installation = uut.GetInstallation(installable);

      uut.Install(uut.GetInstallation(installable));
      Assert.IsTrue(installation.IsInstalled);
      Assert.IsNotNull(installation.InstallationDirectory);
      Assert.IsTrue(Directory.Exists(installation.InstallationFileSystem.ToAbsolutePath(".")));
    }
    [TestMethod]
    public void Deinstallation()
    {
      var installable = app.ActivateModuleType<Installable1>();
      var installation = uut.GetInstallation(installable);

      uut.Install(installation);
      var dir = installation.InstallationFileSystem.ToAbsolutePath(".");
      Assert.IsTrue(Directory.Exists(dir));
      uut.Uninstall(installation);

      Assert.IsFalse(installation.IsInstalled);

      Assert.IsNull(installation.InstallationDirectory);

      Assert.IsFalse(Directory.Exists(dir));

    }

    [TestMethod]
    public void CheckInstallAfterInstallableDiscovered()
    {
      var installable = app.ActivateModuleType<Installable1>();
      var installation = uut.GetInstallation(installable);
      var dir = InstallationModule.GenerateInstallationDirectory(installation).Replace("~", "./test");
      Directory.CreateDirectory(dir);
      Assert.IsFalse(installation.IsInstalled);
      uut.CheckInstall(installation);
      Assert.IsTrue(installation.IsInstalled);
    }


    [TestMethod]
    public void CheckInstallRemovedAfterInstallableDiscovered()
    {
      var installable = app.ActivateModuleType<Installable1>();
      var installation = uut.GetInstallation(installable);
      uut.Install(installation);
      Assert.IsTrue(installation.IsInstalled);
      var dir = installation.InstallationDirectory.Replace("~", "./test");
      Directory.Delete(dir, true);
      uut.CheckInstall(installation);
      Assert.IsFalse(installation.IsInstalled);
    }

    [TestMethod]
    public void CheckInstallWhileInstallableDiscovered()
    {
      var dir = InstallationModule.GenerateInstallationDirectory(InstallationModule.Installable(typeof(Installable1)))
        .Replace("~", "./test");
      Directory.CreateDirectory(dir);
      var installable = app.ActivateModuleType<Installable1>();
      var installation = uut.GetInstallation(installable);
      Assert.IsTrue(installation.IsInstalled);
    }
    [TestMethod]
    public void InstallationRequirementsAreTransferred()
    {
      var m = app.ActivateModuleType<TestInstallerInstallable>();
      var instance = uut.GetInstallation(m);
      Assert.IsTrue(instance.InstallationInformation.InstallationRequirements.Contains("Test"));
    }

    [TestMethod]
    public void CanInstallTestInstaller()
    {
      app.ActivateModuleType<TestInstaller>();
    }
    [TestMethod]
    public void InstallerAdded()
    {
      Assert.AreEqual(0, uut.Installers.Count());
      app.ActivateModuleType<TestInstaller>();
      Assert.AreEqual(1, uut.Installers.Count());

    }



    [TestMethod]
    public void CheckInstallTestInstallerNotIntalled()
    {
      app.ActivateModuleType<TestInstaller>();
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      var installation = uut.GetInstallation(installable);

      var isInstalled = uut.CheckInstall(installation);
      Assert.IsFalse(isInstalled);
    }


    [TestMethod]
    public void CheckInstallTestInstallerIsInstalled()
    {
      var dir = InstallationModule.GenerateInstallationDirectory(InstallationModule.Installable(typeof(TestInstallerInstallable)));
      var installationDir = Path.GetFullPath(dir.Replace("~", "test"));
      Directory.CreateDirectory(installationDir);
      File.Create(Path.Combine(installationDir, "test.txt")).Close();

      app.ActivateModuleType<TestInstaller>();
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      var installation = uut.GetInstallation(installable);

      var isInstalled = uut.CheckInstall(installation);
      Assert.IsTrue(isInstalled);
    }



    [TestMethod]
    public void CheckInstallTestInstallerIsInstalledButNotTestInstaller()
    {

      var dir = InstallationModule.GenerateInstallationDirectory(InstallationModule.Installable(typeof(TestInstallerInstallable)));
      var installationDir = Path.GetFullPath(dir.Replace("~", "test"));
      Directory.CreateDirectory(installationDir);
      // this would staisfy testinstaller
      //File.Create(Path.Combine(installationDir, "test.txt")).Close();

      app.ActivateModuleType<TestInstaller>();
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      var installation = uut.GetInstallation(installable);

      var isInstalled = uut.CheckInstall(installation);
      Assert.IsFalse(isInstalled);
    }


    [TestMethod]
    public void InstallTestInstaller()
    {
      app.ActivateModuleType<TestInstaller>();
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      var installation = uut.GetInstallation(installable);
      uut.Install(installation);
      var result = installation.InstallationFileSystem.Exists("test.txt");
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MissingRequirements()
    {
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      var installation = uut.GetInstallation(installable);
      var result = uut.GetMissingRequirements(installation);

      Assert.AreEqual(1, result.Count());
      Assert.IsTrue(result.Contains("Test"));
    }

    [TestMethod]
    public void CannotInstallBecauseOfMissingRequirement()
    {
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      var installation = uut.GetInstallation(installable);
      var result = uut.CanInstall(installation);
      Assert.IsFalse(result);
    }


    [TestMethod]
    public void CanInstall()
    {
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      app.ActivateModuleType<TestInstaller>();
      var installation = uut.GetInstallation(installable);
      var result = uut.CanInstall(installation);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ObjectNotifiedOfInstallCapabilityAfterMissingINstallerIsAdded()
    {
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      var installation = uut.RequireInstallation(installable);

      Assert.IsFalse(installable.caninstallCalled);
      app.ActivateModuleType<TestInstaller>();
      Assert.IsTrue(installable.caninstallCalled);
    }

    [TestMethod]
    public void InstallableGetsInstallationFileSystem()
    {

      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      app.ActivateModuleType<TestInstaller>();
      Assert.IsNull(installable.FS);
      uut.Install(uut.GetInstallation(installable));
      Assert.IsNotNull(installable.FS);
      uut.Uninstall(uut.GetInstallation(installable));
      Assert.IsNull(installable.FS);

    }


    [TestMethod]
    public void InstallableGetsInstallableInstance()
    {
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      app.ActivateModuleType<TestInstaller>();
      var instance = installable.Ini;
      Assert.IsNotNull(instance);
      Assert.AreEqual(uut.GetInstallation(installable), instance);
    }
    [TestMethod]
    public void InstallableIsCalledBackWhenInstalled()
    {
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      app.ActivateModuleType<TestInstaller>();
      var installation = uut.GetInstallation(installable);

      Assert.IsFalse(installable.installedCalled);
      uut.Install(installation);
      Assert.IsTrue(installable.installedCalled);

    }
    [TestMethod]
    public void InstallableIsCalledBackWhenUninstalled()
    {

      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      app.ActivateModuleType<TestInstaller>();
      var installation = uut.GetInstallation(installable);

      uut.Install(installation);
      Assert.IsFalse(installable.uninstalledCalled);
      uut.Uninstall(installation);
      Assert.IsTrue(installable.uninstalledCalled);
    }


    [TestMethod]
    public void CanInstallCallbackIsCalledWhenModuleIsAddedAndDirectlyInstallable()
    {
      TestInstallerInstallable.installCalled = 0;
      var installer = app.ActivateModuleType<TestInstaller>();
      Assert.AreEqual(0, TestInstallerInstallable.installCalled);
      var installable = app.ActivateModuleType<TestInstallerInstallable>();
      Assert.AreEqual(1, TestInstallerInstallable.installCalled);

    }

    [TestMethod]
    public void NonModuleInstallableInstallation()
    {
      var simple = new MySimpleInstallable();
      uut.RequireInstallation(simple);
      Assert.IsFalse(simple.canInstallCalled);
      app.ActivateModuleType<TestInstaller>();
      Assert.IsTrue(simple.canInstallCalled);
    }

    [TestMethod]
    public void NonModuleInstallableInstallationCanInstallCalledWhenInstallable()
    {
      var simple = new MySimpleInstallable();
      app.ActivateModuleType<TestInstaller>();
      uut.RequireInstallation(simple);
      Assert.IsTrue(simple.canInstallCalled);
    }
    [TestMethod]
    public void InstallShouldBePossibleInCanInstallCallback()
    {
      var unit = new InstallInInstallableCallbackTestClass();
      unit.canInstallCalled = () => { 
        var inst = uut.RequireInstallation(unit);
        Assert.AreEqual(inst, unit.Ini);
        uut.Install(inst);
      };
      uut.RequireInstallation(unit);
    }
    [TestMethod]
    public void InstallationInstanceIsSetBeforeInstallCallback()
    {
      var unit = new InstallInInstallableCallbackTestClass();      
      bool canInstallCalled = false;
      unit.canInstallCalled = () => {canInstallCalled=true; Assert.IsNotNull(unit.Ini); };
      uut.RequireInstallation(unit);
      Assert.IsTrue(canInstallCalled);
    }
    /* these tests are not needed becaouse the functionality already exists with INstallationCallback
    [TestMethod]
    public void InstallationDiscoveredCalledAfterInstall()
    {
      var unit = new InstallationDiscoveredInstallable();
      var installation = uut.RequireInstallation(unit);
      Assert.IsFalse(unit.installationDiscoveredCalled);
      uut.Install(installation);
      Assert.IsTrue(unit.installationDiscoveredCalled);
    }


    [TestMethod]
    public void InstallationDiscoveredCalledIfInstallationWasFound()
    {
      var dir = InstallationModule.GenerateInstallationDirectory(InstallationModule.Installable(typeof(InstallationDiscoveredInstallable))).Replace("~", "./test");
      Directory.CreateDirectory(dir);
      var unit = new InstallationDiscoveredInstallable();
      Assert.IsFalse(unit.installationDiscoveredCalled);
      var installation = uut.RequireInstallation(unit);
      Assert.IsTrue(unit.installationDiscoveredCalled);
    }*/

  }

  [Installable]
  class InstallationDiscoveredInstallable
  {
    public bool installationDiscoveredCalled = false;
    [OnInstallationDiscovered]
    void discovered()
    {
      installationDiscoveredCalled = true;
    }
  }

  [Installable]
  class InstallInInstallableCallbackTestClass
  {
    public Action canInstallCalled;
    [CanInstallCallback]
    void canInstall() { canInstallCalled(); }

    private InstallationInstance ini;
    [ImportInstallationInstance]
    public InstallationInstance Ini { get { return ini; } set { ini = value; } }
  }
  [Installable(InstallationRequirements=new string[]{"Test"})]
  class MySimpleInstallable
  {
    [CanInstallCallback]
    void canInstall() { canInstallCalled = true; }


    public bool canInstallCalled { get; set; }
  }

  [Module]
  [Installable]
  class Installable1
  {


  }
  

  [Module]
  [Installable(InstallationRequirements = new[] { "Test" })]
  class TestInstallerInstallable
  {
    public bool caninstallCalled = false;
    public static int installCalled = 0;
    [CanInstallCallback]
    void install()
    {
      installCalled++;
      caninstallCalled = true;
    }

    public bool installedCalled = false;
    public bool uninstalledCalled = false;
    [InstalledCallback]
    public void Installed()
    {
      installedCalled = true;
    }

    [UninstalledCallbackAttribute]
    public void Uninstalled()
    {
      uninstalledCalled = true;
    }

    [ImportInstallationInstance]
    public InstallationInstance Ini { get; set; }

    [ImportInstallationFileSystem]
    public IRelativeFileSystem FS
    {
      get;
      set;
    }
  }

  [Module]
  [Installer]
  class TestInstaller : IInstaller
  {

    public IEnumerable<string> CanInstall(InstallationInstance instance)
    {
      return instance.InstallationInformation.InstallationRequirements.Where(req => req == "Test").ToArray();
    }

    public bool CheckInstall(InstallationInstance instance)
    {
      if (instance.InstallationFileSystem == null) return false;

      return instance.InstallationFileSystem.Exists("test.txt");
    }

    public void Install(InstallationInstance instance)
    {
      instance.InstallationFileSystem.CreateFile("test.txt").Dispose();
    }

    public void Uninstall(InstallationInstance instance)
    {
      if (instance.InstallationFileSystem.Exists("test.txt")) instance.InstallationFileSystem.Delete("test.txt");

    }
  }


}
