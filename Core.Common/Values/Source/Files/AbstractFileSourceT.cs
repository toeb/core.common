using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  public abstract class AbstractFileSource<T> : AbstractFileSource
  {
    public AbstractFileSource(WatchableFile file, SourceInfo info) : base(file, info) { }

    protected abstract T TypedReadFile(Stream stream);
    protected override object FileMissingValue
    {
      get
      {
        return default(T);
      }
    }
    protected override object ReadFile(Stream stream)
    {
      return TypedReadFile(stream);
    }
  }
}
