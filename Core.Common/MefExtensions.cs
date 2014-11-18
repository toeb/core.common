using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{

  public static class MefExtensions
  {
    /// <summary>
    /// gets an exported value of a specific object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="contractName"></param>
    /// <returns></returns>
    public static Delegate GetExportedDelegate(this object self, string contractName)
    {
      var batch = new CompositionBatch();
      var part = batch.AddPart(self);

      var exportDefinition = part.ExportDefinitions.SingleOrDefault(ed => ed.ContractName == contractName);
      if (exportDefinition == null) return null;
      var exportedValue = part.GetExportedValue(exportDefinition) as ExportedDelegate;
      var del = exportedValue.CreateDelegate(typeof(Delegate));
      return del;
    }

    public static IEnumerable<Delegate> GetExportedDelegates(this object self, string contractName)
    {
      var batch = new CompositionBatch();
      var part = batch.AddPart(self);

      var exportDefinitions = part.ExportDefinitions.Where(ed => ed.ContractName == contractName);
      var exportedValues = exportDefinitions.Select(def => part.GetExportedValue(def) as ExportedDelegate);
      var dels = exportedValues.Select(del => del.CreateDelegate(typeof(Delegate)));
      return dels;

    }

    public static object GetExportedValue(this CompositionContainer container, Type type)
    {
      var exports = container.GetExports(type, null, null);
      var export = exports.SingleOrDefault();
      return export.Value;
    }

    public static Lazy<object, object> GetSingleExport(this CompositionContainer container, Type type)
    {
      var exports = container.GetExports(type, null, null);
      var export = exports.SingleOrDefault();
      return export;
    }

  }
}
