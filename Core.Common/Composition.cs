using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

  /// <summary>
  /// The to be central class of every application 
  /// Controls the Root composition container
  /// </summary>
  public static class Composition
  {
    private static AggregateCatalog catalog = null;
    private static CompositionContainer container = null;

    static Composition()
    {
    }

    /// <summary>
    /// Initializes the the Rootcatalog to the application catalog
    /// </summary>
    public static void InitializeDefault()
    {
      RootCatalog.Catalogs.Add(new ApplicationCatalog());
    }

    /// <summary>
    /// Root Catalog contains top level services and objects
    /// </summary>
    public static AggregateCatalog RootCatalog
    {
      get
      {
        if (catalog == null)
        {
          catalog = new AggregateCatalog();
        }
        return catalog;
      }
    }
    /// <summary>
    /// the root container
    /// 
    /// </summary>
    public static CompositionContainer RootContainer
    {
      get
      {
        if (container == null)
        {
          container = new CompositionContainer(RootCatalog);

          container.ComposeExportedValue(container);
          container.ComposeExportedValue("RootContainer", RootContainer);
          container.ComposeExportedValue("RootCatalog", RootCatalog);
        }
        return container;
      }
    }
    /// <summary>
    /// simple method which forward to root composition container
    /// other objects may use this if the do not want to reference System.ComponentModel.Composition
    /// </summary>
    /// <param name="contract"></param>
    /// <returns></returns>
    public static T GetExportedValue<T>(string contract = null)
    {
      if (string.IsNullOrEmpty(contract))
      {
        return RootContainer.GetExportedValue<T>();
      }
      return RootContainer.GetExportedValue<T>(contract);
    }

  }
}
