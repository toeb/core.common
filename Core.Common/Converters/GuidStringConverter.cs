using System;
using System.ComponentModel.Composition;

namespace Core.Converters
{
  /// <summary>
  /// converts string<->Guid no  specific format is used
  /// </summary>
  [Export]
  [Export(typeof(IConverter))]
  [Export(typeof(IConverter<Guid, string>))]
  [Export(typeof(IConverter<string, Guid>))]
  public class GuidStringConverter : IConverter<Guid, string>, IConverter<string, Guid>
  {
    public Guid Convert(string f)
    {
      Guid result;
      if (!Guid.TryParse(f, out result)) return Guid.Empty;
      return result;
    }
    public string Convert(Guid f)
    {
      return f.ToString("N");
    }
    public string Format
    {
      get { return "no format"; }
    }

    public bool CanConvert(Type from, Type to)
    {
      return from == typeof(Guid) && to == typeof(string) || to == typeof(Guid) && from == typeof(string);
    }

    object IConverter.Convert(object from)
    {
      if (from == null) throw new ArgumentNullException("from");
      if (from is string) return Convert(from as string);
      if (from is Guid) return Convert((Guid)from);
      throw new ArgumentException("argument is neither of type string nor Guid", "from");
    }
  }
}