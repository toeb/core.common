using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{


  public class DelegateFileSource<T> : DelegateFileSource, ISource<T>
  {
    public DelegateFileSource(FromStreamDelegate<T> readFile, WatchableFile file) :
      this(readFile, file, new SourceInfo(true, typeof(T), false))
    {

    }
    public DelegateFileSource(FromStreamDelegate<T> readFile, WatchableFile file, SourceInfo info) :
      base(stream => readFile(stream), file, info)
    {

    }

    public new T Value
    {
      get { return (T)base.Value; }
    }


  }

}
