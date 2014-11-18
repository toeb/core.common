using System;

namespace Core.Converters
{
  public interface IConverter
  {
    string Format { get; }
    bool CanConvert(Type from, Type to);
    object Convert(object from);
  }
}
