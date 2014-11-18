using System;
using System.IO;

namespace Core.Values
{
  public class DelegateFileSource : AbstractFileSource
  {
    public DelegateFileSource(TransformValueDelegate<Stream, object> readFile, WatchableFile watchableFile, SourceInfo info)
      : base(watchableFile, info)
    {
      if (readFile == null) throw new ArgumentNullException("readFile");
      ReadFileDelegate = readFile;
    }

    protected override object ReadFile(Stream stream)
    {
      return ReadFileDelegate(stream);
    }

    public TransformValueDelegate<Stream, object> ReadFileDelegate { get; private set; }
  }
}
