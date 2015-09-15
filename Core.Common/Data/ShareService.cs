using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Data
{

  /// <summary>
  /// Used to share data accross decoupled components
  /// </summary>
  [Export(typeof(IShareService))]
  public class ShareService : IShareService
  {
    private IDictionary<Tuple<string, Type>, object> collections = new Dictionary<Tuple<string, Type>, object>();

    protected ICollection<T> MakeCollection<T>()
    {
      return new ObservableCollection<T>();
    }
    static MethodInfo method = null;
    public ICollection<T> Collection<T>(string stringkey) where T : class
    {
      var key = Tuple.Create(stringkey, typeof(T));
      if (collections.ContainsKey(key)) return collections[key] as ICollection<T>;
      if (method == null)
      {
        method = typeof(ShareService).GetMethod("MakeCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
      }
      var collection = method.Invoke(this, new object[0]) as ICollection<T>;
      collections[key] = collection;
      return collection;

    }
  }
}
