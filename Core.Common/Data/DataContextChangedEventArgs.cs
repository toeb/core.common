using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common.Data
{
  public class DataContextChangedEventArgs : EventArgs
  {
    public DataContextChangedEventArgs(IEntry entry)
    {
      this.Entry = entry;
    }

    public IEntry Entry { get; set; }
  }
}
