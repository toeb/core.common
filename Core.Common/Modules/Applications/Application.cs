using Core.FileSystem;
using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Linq;
namespace Core.Modules.Applications
{
  public static class Application
  {
    /// <summary>
    /// creates the default application
    /// </summary>
    /// <returns></returns>

    public static ModuleApplication Create()
    {
      return Create<ModuleApplication>();
    }
    /// <summary>
    /// create the default application with the specified container as the parentcontainer
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static IApplication Create(CompositionContainer parent)
    {
      return Create<ModuleApplication>(parent);
    }


    public static T Create<T>(CompositionContainer parent) where T : IApplication
    {
      return (T)Create(typeof(T), parent);
    }

    public static T Create<T>() where T : IApplication
    {
      return (T)Create(typeof(T));
    }

    public static T Create<T>(Action<CompositionContainer> configContainer) where T : IApplication
    {
      return (T)Create(typeof(T),configContainer);
    }
    public static IApplication Create(Type applicationType, Action<CompositionContainer> configContainer)
    {

      CompositionContainer globalContainer = new CompositionContainer(
        new TypeCatalog(
        typeof(FileSystemService)
        )
        );
      globalContainer.ComposeExportedValue<IErrorService>(ErrorService.Instance);
      globalContainer.ComposeExportedValue<IReflectionService>(ReflectionService.Instance);
      configContainer(globalContainer);

      return Create(applicationType, globalContainer);
    }

    public static IApplication Create(Type applicationType)
    {
      CompositionContainer globalContainer = new CompositionContainer(
        new TypeCatalog(
        typeof(FileSystemService)
        )
        );

      globalContainer.ComposeExportedValue<IErrorService>(ErrorService.Instance);
      globalContainer.ComposeExportedValue<IReflectionService>(ReflectionService.Instance);

      return Create(applicationType, globalContainer);
    }
    public static IApplication Create(Type applicationType, CompositionContainer parentContainer)
    {
      if (parentContainer == null) throw new ArgumentNullException("parentContainer");
      CompositionContainer applicationContainer = new CompositionContainer(new TypeCatalog(applicationType), parentContainer);
      applicationContainer.ComposeExportedValue<CompositionContainer>(applicationContainer);
      applicationContainer.ComposeExportedValue<CompositionContainer>("ApplicationCatalog", applicationContainer);

      var exports = applicationContainer.GetExports(applicationType, null, null);
      var export = exports.Single();
      applicationContainer.ComposeExportedValue(export.Value);
      return export.Value as IApplication;
    }


  }


}
