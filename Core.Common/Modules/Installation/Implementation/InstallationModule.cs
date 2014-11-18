using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Core.Extensions;
using Core.FileSystem;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;

namespace Core.Modules.Installation
{


  [Export(typeof(IInstallationService))]
  [Module]
  public class InstallationModule : NotifyPropertyChangedBase, IInstallationService
  {
    private readonly ISet<IInstaller> installers = new HashSet<IInstaller>();
    [ImportMany(AllowRecomposition = true)]
    public IEnumerable<IInstaller> Installers
    {
      get { return installers; }
      set
      {
        ChangeEnumerableCallback(installers, value, AddInstaller, RemoveInstaller);
        //ChangeEnumerableSingleItemCallback(ref installers, value, InstallerAdded, InstallerRemoved);
      }
    }

    public bool AddInstaller(IInstaller installer)
    {
      if (!installers.Add(installer)) return false;
      InstallerAdded(installer);
      return true;
    }

    public bool RemoveInstaller(IInstaller installer)
    {
      if (!installers.Remove(installer)) return false;
      InstallerRemoved(installer);
      return true;
    }

    private void InstallerRemoved(IInstaller removedElement)
    {

    }


    private IEnumerable<string> GetMissingRequirements(IEnumerable<IInstaller> installers, InstallationInstance instance)
    {
      var metRequirements = installers.SelectMany(installer => installer.CanInstall(instance)).Distinct();
      var neededRequirements = instance.InstallationInformation.InstallationRequirements.Distinct();
      var missingRequirements = neededRequirements.Except(metRequirements);
      return missingRequirements;
    }
    private bool CanInstall(IEnumerable<IInstaller> installers, InstallationInstance instance)
    {
      return !GetMissingRequirements(installers, instance).Any();

    }
    private void InstallerAdded(IInstaller installer)
    {
      var installers = Installers.Except(installer);
      foreach (var installation in Installations)
      {
        bool installableBefore = CanInstall(installers, installation);
        bool installableAfter = CanInstall(Installers, installation);


        if (!installableBefore && installableAfter)
        {
          NotifyInstallableOfInstallationCapability(installation);
        }
      }
    }

    private void NotifyInstallableOfInstallationCapability(InstallationInstance installation)
    {
      var del = installation.Installable.GetExportedDelegate(CanInstallCallbackAttribute.ContractName);
      if (del == null) return;
      del.DynamicInvoke();
    }


    [ImportingConstructor]
    InstallationModule([Import(InstallationContracts.InstallationFileSystem, AllowDefault=true)] IRelativeFileSystem fileSystem)
    {
      if (fileSystem == null) fileSystem = new RelativeFileSystem(Environment.CurrentDirectory);
      FileSystem = fileSystem;
    }





    public IRelativeFileSystem FileSystem
    {
      get;
      private set;
    }

    private IEnumerable<object> installables = new object[0];
    [ImportMany(InstallableAttribute.InstallableContractName, AllowRecomposition = true)]
    public IEnumerable<object> Installables { get { return installables; } set { ChangeEnumerableSingleItemCallback(ref installables, value, InstallablesAdded, InstallablesRemoved); } }


    Dictionary<object, InstallationInstance> installationInstances = new Dictionary<object, InstallationInstance>();

    private void InstallablesRemoved(object removedElement)
    {
      installationInstances.Remove(removedElement);
    }
    private void InstallablesAdded(object addedElement)
    {
      RequireInstallation(addedElement);
    }

    public bool HasInstallation(object installation)
    {
      return installationInstances.ContainsKey(installation);
    }

    public IEnumerable<InstallationInstance> Installations
    {
      get
      {
        return installationInstances.Values;
      }
    }
    public IEnumerable<InstallationInstance> InstallableInstallations
    {
      get
      {
        return Installations.Where(installation => CanInstall(installation));
      }
    }

    [Import]
    CompositionContainer Container { get; set; }

    protected virtual InstallationInstance CreateInstallationInstance(object installable)
    {
      var type = installable.GetType();
      var attribute = Installable(type);
      if (attribute == null) throw new ArgumentException("object is not installable - it needs to be marked with the Installable Attribute");

      var installation =
        new InstallationInstance()
        {
          InstallationService = this,
          InstallationInformation = attribute,
          Installable = installable,
          IsInstalled = false
        };

      installationInstances[installable] = installation;

      SetInstallableInstallationInstance(installation);
      CheckInstall(installation);
      if (CanInstall(installation)) NotifyInstallableOfInstallationCapability(installation);
      return installation;
    }
    public InstallationInstance RequireInstallation(object installable)
    {
      if (HasInstallation(installable)) return GetInstallation(installable);
      var installation = CreateInstallationInstance(installable);
      return installation;
    }

    public InstallationInstance GetInstallation(object installable)
    {
      if (!HasInstallation(installable)) return null;
      return installationInstances[installable];
    }

    IFileSystemService FileSystemService { get; set; }

    public void Install(InstallationInstance instance)
    {
      CheckInstall(instance);
      if (!CanInstall(instance)) throw new InvalidOperationException("instance cannot be installed / installer missing fort requirement");
      if (instance.IsInstalled) throw new InvalidOperationException("instance is already installed");
      CreateInstallationDirectory(instance);

      Installers.Where(installer => installer.CanInstall(instance).Any()).Do(installer => installer.Install(instance));


      instance.IsInstalled = true;
      CallbackInstallableInstalled(instance);
    }

    public bool CheckInstall(InstallationInstance instance)
    {
      if (!CanInstall(instance)) return false;
      //check directory is created
      var directory = GenerateInstallationDirectory(instance);
      var installed = true;
      installed &= FileSystem.IsDirectory(directory);
      if (installed) instance.InstallationDirectory = directory;
      if (installed) instance.InstallationFileSystem = FileSystem.ScopeTo(directory);
      if (installed) Installers.Where(installer => installer.CanInstall(instance).Any()).Do(installer => installed &= installer.CheckInstall(instance));
      if (instance.IsInstalled != installed)
      {
        if (installed)
        {
          CallbackInstallableInstalled(instance);
          SetInstallationFileSystem(instance);
        }
        else
        {
          CallbackInstallableUninstalled(instance);
          UnsetInstallationFileSystem(instance);

        }
      }
      instance.IsInstalled = installed;
      if (!installed)
      {
        instance.InstallationDirectory = null;
        instance.InstallationFileSystem = null;
      }

      return installed;
    }
    public void Uninstall(InstallationInstance instance)
    {
      if (!instance.IsInstalled) throw new InvalidOperationException("instance is not installed");

      instance.InstallationFileSystem.DeleteDirectory(".", true);
      instance.InstallationFileSystem = null;
      instance.InstallationDirectory = null;
      instance.IsInstalled = false;
      UnsetInstallationFileSystem(instance);
      CallbackInstallableUninstalled(instance);
    }

    private void CreateInstallationDirectory(InstallationInstance instance)
    {
      var relativeDir = GenerateInstallationDirectory(instance);
      var fs = FileSystem.ScopeTo(relativeDir);
      fs.CreateDirectory("");
      instance.InstallationDirectory = relativeDir;
      instance.InstallationFileSystem = fs;

      SetInstallationFileSystem(instance);

    }

    public static string GenerateInstallationDirectory(InstallationInstance instance)
    {
      return GenerateInstallationDirectory(Installable(instance));
    }
    public static string GenerateInstallationDirectory(InstallableAttribute attribute)
    {
      var dir = string.Format("~/{0}/{1}/", attribute.InstallationName, attribute.InstallationVersion);
      return dir;
    }


    public static InstallableAttribute Installable(Type type)
    {
      var installable = type.GetCustomAttribute<InstallableAttribute>() ?? new InstallableAttribute();
      if (string.IsNullOrEmpty(installable.InstallationName)) installable.InstallationName = type.FullName;
      if (string.IsNullOrEmpty(installable.InstallationVersion)) installable.InstallationVersion = new Version(1, 0, 0, 0).ToString();
      if (installable.InstallationRequirements == null) installable.InstallationRequirements = new string[0];
      return installable;

    }
    public static InstallableAttribute Installable(InstallationInstance instance)
    {
      var type = instance.Installable.GetType();
      return Installable(type);
    }

    void SetInstallableInstallationInstance(InstallationInstance installation)
    {
      if (installation.Installable == null) throw new ArgumentNullException();
      var type = installation.Installable.GetType();
      var prop = type.PropertiesWith<ImportInstallationInstanceAttribute>().SingleOrDefault();
      if (prop == null) return;
      prop.Item1.SetValue(installation.Installable, installation);
    }
    void UnsetInstallaableInstallationInstance(InstallationInstance installation)
    {
      if (installation.Installable == null) throw new ArgumentNullException();
      var type = installation.Installable.GetType();
      var prop = type.PropertiesWith<ImportInstallationInstanceAttribute>().SingleOrDefault();
      if (prop == null) return;
      prop.Item1.SetValue(installation.Installable, null);
    }
    void CallbackInstallableInstalled(InstallationInstance installation)
    {
      if (installation.Installable == null) throw new ArgumentNullException();
      var del = installation.Installable.GetExportedDelegate(InstalledCallbackAttribute.ContractName);
      if (del == null) return;
      del.DynamicInvoke();
    }
    void CallbackInstallableUninstalled(InstallationInstance installation)
    {

      if (installation.Installable == null) throw new ArgumentNullException();
      var del = installation.Installable.GetExportedDelegate(UninstalledCallbackAttribute.ContractName);
      if (del == null) return;
      del.DynamicInvoke();

    }
    void SetInstallationFileSystem(InstallationInstance installation)
    {
      if (installation.Installable == null) throw new ArgumentNullException();
      var type = installation.Installable.GetType();
      var prop = type.PropertiesWith<ImportInstallationFileSystemAttribute>().SingleOrDefault();
      if (prop == null) return;
      prop.Item1.SetValue(installation.Installable, installation.InstallationFileSystem);
    }
    void UnsetInstallationFileSystem(InstallationInstance installation)
    {
      if (installation.Installable == null) throw new ArgumentNullException();
      var type = installation.Installable.GetType();
      var prop = type.PropertiesWith<ImportInstallationFileSystemAttribute>().SingleOrDefault();
      if (prop == null) return;
      prop.Item1.SetValue(installation.Installable, null);
    }


    public IEnumerable<string> GetMissingRequirements(InstallationInstance instance)
    {
      var requirements = instance.InstallationInformation.InstallationRequirements.Distinct();
      var metRequirements = GetMetRequirements(instance);
      return requirements.Except(metRequirements);
    }
    public IEnumerable<string> GetMetRequirements(InstallationInstance instance)
    {
      var metRequirements = Installers.SelectMany(installer => installer.CanInstall(instance)).Distinct();
      return metRequirements;
    }

    public bool CanInstall(InstallationInstance installation)
    {
      var missingRequirements = GetMissingRequirements(installation);
      bool result = !missingRequirements.Any();
      return result;
    }
  }
}
