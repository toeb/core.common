using Core;
using System;
using System.IO;
using Core.Strings;

namespace Core.Formatters
{

  public class WriteContext : FormattingContext
  {
    public Stream OutputStream { get { return Get<Stream>(); } set { Set(value); } }
    public object Value { get { return Get<object>(); } set { Set(value); } }

    public IWriter CurrentWriter { get { return Get<IWriter>(); } internal set { Set(value); } }

  }


}
