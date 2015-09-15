using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class Streams
  {
    public static string ReadTextToEnd(this Stream stream)
    {
      var reader = new StreamReader(stream);
      var result = reader.ReadToEnd();
      return result;
    }

    public static void WriteText(this Stream stream, string text)
    {
      var writer = new StreamWriter(stream);
      writer.Write(text);
      writer.Flush();
    }
    public static void WriteChar(this Stream stream, char @char)
    {
      var writer = new StreamWriter(stream);
      writer.Write(@char);
      writer.Flush();
    }
    public static string WriteToString(Action<Stream> writeAction)
    {
      var stream = new MemoryStream();
      writeAction(stream);
      stream.Seek(0, SeekOrigin.Begin);
      return stream.ReadTextToEnd();
    }
    public static Task<string> WriteToString(Func<Stream, Task> asyncWriteAction)
    {
      var stream = new MemoryStream();
      var task = asyncWriteAction(stream);
      var result = task.ContinueWith(t =>
      {
        stream.Seek(0, SeekOrigin.Begin);
        return stream.ReadTextToEnd();
      });

      return result;
      
    }
  }
}
