using Core;
using System;
using System.IO;

namespace Core.Formatters
{
  public class ReadContext : FormattingContext 
  {
    public Stream InputStream { get { return Get<Stream>(); } set { Set(value); } }
    public object ExistingValue { get { return Get<object>(); } set { Set(value); } }

    public IReader CurrentReader { get { return Get<IReader>(); } internal set { Set(value); } }
  }
}
