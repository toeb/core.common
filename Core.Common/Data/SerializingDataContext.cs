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
using System.IO;

namespace Core.Common.Data
{
  public abstract class SerializingDataContext : SimpleDataContext
  {

    public string BaseDir { get; set; }

    public SerializingDataContext()
    {
      BaseDir = Path.GetFullPath("data");
    }
    protected override System.Threading.Tasks.Task DoSaveAsync()
    {
      return Task.Factory.StartNew(() =>
      {
        var entries = Entries.Cast<SimpleEntry>().ToArray();
        foreach (var entry in entries)
        {
          entry.ChangeDate = DateTime.Now;
          if (entry.State == EntityState.Modified || entry.State == EntityState.Attached)
          {
            entry.State = EntityState.Unmodified;
          }
          entry.State = EntityState.Unmodified;
          entry.Hash = GetHash(entry.Value);

        }
        var result = Serialize(entries.Where(entry => entry.State == EntityState.Unmodified).ToArray());
        if (!Directory.Exists(BaseDir)) Directory.CreateDirectory(BaseDir);
        File.WriteAllText(BaseDir + "/data.json", result);

      });
    }

    protected abstract string Serialize(IEnumerable<IEntry> entries);
    protected abstract IEnumerable<SimpleEntry> Deserialize(string txt);

    void UpdateEntry(IEntry entry)
    {

      var existingEntry = GetEntryById(entry.Id) as SimpleEntry;

      //var references = Reflection.Bfs(entry.Value).ToArray();
      //foreach (var reference in references) Entry(reference);
      
      if (existingEntry == null)
      {
        // not loaded yet 
        // from datastore
        existingEntry = Entry(entry.Value) as SimpleEntry;
        existingEntry.Id = entry.Id;

      }else if (existingEntry.Hash.Equals(entry.Hash))
      {
        // no change
      }
      else
      {
        throw new NotImplementedException();
        // changed
      }

      existingEntry.State = EntityState.Unmodified;
    }
    private void DoRefresh()
    {
      if (!File.Exists(BaseDir + "/data.json")) return;

      var txt = File.ReadAllText(BaseDir + "/data.json");
      var entries = Deserialize(txt);

      foreach (var entry in entries)
      {
        UpdateEntry(entry);
      }
    }
    protected override Task DoRefreshAsync()
    {
      return Task.Factory.StartNew(DoRefresh);
    }

  }
}
