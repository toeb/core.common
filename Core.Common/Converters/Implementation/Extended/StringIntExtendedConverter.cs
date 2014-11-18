
namespace Core.Converters
{
  public class StringIntExtendedConverter : AbstractTwoWayExtendedConverter<string,int>
  {

    protected override int Convert(string from, ConversionRequest request)
    {
      return int.Parse(from);
    }

    protected override string Convert(int from, ConversionRequest request)
    {
      return from.ToString();
    }
  }
}
