
namespace Core.Converters
{
  public class AssignableTypeConverter : IExtendedConverter
  {

    public ConversionResponse Convert(ConversionRequest request)
    {
      if (!request.TargetType.IsAssignableFrom(request.SourceType)) return request.Error("cannot converter");
      if(request.Simulate)return request.SimulateSuccess();
      return request.Success(request.Source);
    }
  }
}
