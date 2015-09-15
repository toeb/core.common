using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem.Memory
{

  public class MemoryFileSystemEntryStream : MemoryStream
  {
    public MemoryFileSystemEntryStream(MemoryFileSystemEntry entry)
      : base()
    {
      this.Entry = entry;
    }
    public override void Flush()
    {
      base.Flush();
      Entry.Data = ToArray();
    }
    //public async override Task FlushAsync(System.Threading.CancellationToken cancellationToken)
    //{
    //  await base.FlushAsync(cancellationToken);
    //  Entry.Data = ToArray();
    //}

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (disposing)
      {
        Flush();
      }
    }

    public MemoryFileSystemEntry Entry { get; set; }
  }
}
