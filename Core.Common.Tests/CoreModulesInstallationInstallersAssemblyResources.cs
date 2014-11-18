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
using Core.Resources;
using Core.Modules.Installation.AssemblyResources;

namespace Core.Modules.Installation.Tests
{
  [Module]
  [Installable(InstallationRequirements = new[] { "AssemblyResources" })]
  class TestAssemblyResourcesInstallable
  {

  }
  
  [TestClass]
  public class CoreModulesInstallationInstallersAssemblyResources : TypedTestBase<AssemblyResourcesInstaller>
  {

    public ModuleApplication app { get; set; }

    public InstallationModule installer { get; set; }
    protected override AssemblyResourcesInstaller CreateUut()
    {
      if (Directory.Exists("test")) Directory.Delete("test", true);

      app = Application.Create<ModuleApplication>();
      app.Import<IRelativeFileSystem>(new RelativeFileSystem("test"), InstallationContracts.InstallationFileSystem);
      app.ActivateModuleType<ResourcesModule>();
      installer = app.ActivateModuleType<InstallationModule>();
      var module = app.ActivateModuleType<AssemblyResourcesInstallerModule>();
      return module.AssemblyResourcesInstaller;
    }

    [TestMethod]
    public void CreateAssemblyResourcesInstaller()
    {
      Assert.IsNotNull(uut);
      Assert.IsTrue(installer.Installers.Contains(uut));
    }

    [TestMethod]
    public void CanInstall()
    {
      var installable = app.ActivateModuleType<TestAssemblyResourcesInstallable>();
      var installation = installer.GetInstallation(installable);
      Assert.IsNotNull(installation);
      Assert.IsTrue(installer.CanInstall(installation));
    }

    [TestMethod]
    public void MeetsInstallationRequirements()
    {
      var installable = app.ActivateModuleType<TestAssemblyResourcesInstallable>();
      var installation = installer.GetInstallation(installable);
      var reqs = installer.GetMissingRequirements(installation);
      Assert.IsFalse(reqs.Any());
    }

    [TestMethod]
    public void InstallsResources()
    {
      var installable = app.ActivateModuleType<TestAssemblyResourcesInstallable>();
      var installation = installer.GetInstallation(installable);

      installer.Install(installation);
      Assert.IsTrue(installation.InstallationFileSystem.Exists("AssemblyResources/Test.html"));
    }

    [TestMethod]
    public void UninstallResources()
    {
      var installable = app.ActivateModuleType<TestAssemblyResourcesInstallable>();
      var installation = installer.GetInstallation(installable);

      installer.Install(installation);
      var fs = installation.InstallationFileSystem;
      installer.Uninstall(installation);
      Assert.IsFalse(fs.Exists("AssemblyResources/Test.html"));
      Assert.IsFalse(fs.Exists("AssemblyResources"));
    }


  }
}
