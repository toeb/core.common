
namespace Core.Converters
{

  public interface IConverter<TFrom, TTo> : IConverter
  {
    TTo Convert(TFrom f);
  }
}
