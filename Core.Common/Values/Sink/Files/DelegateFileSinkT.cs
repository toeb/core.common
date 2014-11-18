using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{

  public class DelegateFileSink<T> : AbstractFileSink<T>, ISink<T>
  {
    public DelegateFileSink(string path, ToStreamDelegate<T> toStream)
      : base(path)
    {
      if (toStream == null) throw new ArgumentNullException("toStream");
      WriteFileDelegate = toStream;
    }
    public DelegateFileSink(string path, ToStreamDelegate<T> toStream, SinkInfo info)
      : base(path,info)
    {
      if (toStream == null) throw new ArgumentNullException("toStream");
      WriteFileDelegate = toStream;
    }

    protected override void TypedFileWrite(Stream stream, T value)
    {
      WriteFileDelegate(value, stream);
    }

    public ToStreamDelegate<T> WriteFileDelegate { get; private set; }

    public new T Value
    {
      set { base.Value = value; }
    }


  }
}
