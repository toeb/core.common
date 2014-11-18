using Core;
using Core.Strings;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Formatters
{
  public static class IFormatterExtensions
  {

    public static bool CanRead(this IReader reader, Type type)
    {
      return reader.CanRead(new ReadContext() { Type = type });
    }
    public static bool CanWrite(this IWriter writer, Type type)
    {
      return writer.CanWrite(new WriteContext() { Type = type });
    }
    public static object Read(this IReader formatter, string str, Type type, ReadContext context)
    {
      var stream = str.AsStream(Encoding.UTF8);
      var result = formatter.Read(stream, type, null, context);
      stream.Dispose();
      return result;
    }
    public static object Read(this IReader reader, string str, Type type)
    {
      return reader.Read(str, type, new ReadContext());
    }
    public static T Read<T>(this IReader reader, string str)
    {
      return (T)(reader.Read(str, typeof(T)));
    }
    public static string WriteObject(this IWriter writer, object value)
    {
      if (value == null) throw new ArgumentNullException("value");
      var result = Streams.WriteToString(stream => writer.Write(stream, value.GetType(), value, new WriteContext()));
      return result;
    }

    public static string WriteText(this IWriter writer, WriteContext context)
    {
      var result = Streams.WriteToString(stream => writer.Write(stream, context.Type, context.Value, context));
      return result;
    }

    public static void Write(this IWriter formatter, Stream stream, Type type, object value, WriteContext context)
    {
      context.OutputStream = stream;
      context.Type = type;
      context.Value = value;
      formatter.Write(context);
    }

    public static object Read(this IReader formatter, Stream stream, Type type, object value, ReadContext context)
    {
      context.InputStream = stream;
      context.Type = type;
      context.ExistingValue = value;
      return formatter.Read(context);
    }

  }
}
