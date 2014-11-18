using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{

  public abstract class AbstractFileSink<T> : AbstractFileSink
  {
    protected AbstractFileSink(string path)
      : this(path, new SinkInfo(true, typeof(T), false))
    {
      if (string.IsNullOrEmpty(path)) throw new ArgumentException("path must be a valid pathname", "path");
    }


    protected AbstractFileSink(string path, SinkInfo info)
      : base(path, info)
    {
      if (string.IsNullOrEmpty(path)) throw new ArgumentException("path must be a valid pathname", "path");
    }

    protected abstract void TypedFileWrite(Stream stream, T value);
    protected override void WriteFile(Stream stream, object value)
    {
      TypedFileWrite(stream, (T)value);
    }
  }


}
