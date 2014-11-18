
namespace Core.Converters
{
  public class IntCharConverter : AbstractTwoWayExtendedConverter<int, char>
  {

    protected override char Convert(int from, ConversionRequest request)
    {
      return (char)from;
    }

    protected override int Convert(char from, ConversionRequest request)
    {
      return (int)from;
    }
  }
}
