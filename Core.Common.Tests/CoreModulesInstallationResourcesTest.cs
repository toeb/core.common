using Core.FileSystem;
using Core.Modules.Applications;
using Core.Resources;
using Core.TestingUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Installation.AssemblyResources.Test
{
  [TestClass]
  public class CoreModulesInstallationResourcesTest : TypedTestBase<AssemblyResourcesInstaller>
  {

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
    public void TestResourcesAreProvided()
    {
      var uut = app.ActivateModuleType<TestModule>();
      var installation = installer.RequireInstallation(uut);
      installer.Install(installation);
      Assert.IsNotNull(uut.ResourceInstallation);
      Assert.AreEqual(1, uut.ResourceInstallation.Resources.Count());
      Assert.IsTrue(uut.ResourceInstallation.FileSystem.Exists("test1.txt"));

    }

    public ModuleApplication app { get; set; }

    public InstallationModule installer { get; set; }
  }

  [Module]
  [Installable("AssemblyResources")]
  class TestModule
  {
    [ImportInstalledResources]
    public IAssemblyResourceInstallation ResourceInstallation { get; set; }


    [InstalledCallback]
    public void Installed()
    {
    }

  }
}
