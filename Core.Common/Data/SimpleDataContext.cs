using Core.Common.Reflect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Crypto;

namespace Core.Common.Data
{
  public class SimpleDataContext : AbstractDataContext
  {
    private IDispatcher dispatcher;
    [Import(AllowDefault = true)]
    IDispatcher Dispatcher { get { return dispatcher ?? (dispatcher = new SimpleDispatcher()); } set { dispatcher = value; } }

    protected override ICollection<T> MakeSet<T>()
    {
      return new SimpleDataContextCollection<T>(Dispatcher, this);
    }


    protected override object GenerateId(object item)
    {
      return Guid.NewGuid();
    }
    protected override IEntry CreateEntry(object item)
    {

      var entry = new SimpleEntry(this);
      entry.Value = item;
      entry.Id = GenerateId(item);
      entry.ChangeDate = DateTime.Now;
      entry.Hash = item.ComputeHash();
      return entry;
    }
    protected override object GetHash(object entity)
    {
      return entity.ComputeHash();
    }

    protected override Task DoSaveAsync()
    {
      return Task.Factory.StartNew(() =>
      {
        foreach (var entry in Entries)
        {
          entry.State = EntityState.Unmodified;
        }
      });
    }

    protected override Task DoRefreshAsync()
    {
      return Task.Factory.StartNew(() => { });
    }

    protected override IEnumerable<object> GetReferencedEntities(object entity)
    {
      var graph = Core.Common.Reflect.Reflection.Bfs(entity).Where(e => e != null).ToArray();
      return graph;
    }
  }
}
