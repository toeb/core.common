
namespace Core.Converters
{
  public class ValueTypeToNullableConverter : IExtendedConverter
  {

    public ConversionResponse Convert(ConversionRequest request)
    {

      if (!request.SourceType.IsValueType) return request.Error("source type is not a value type");
      if (request.SourceType.IsNullable()) return request.Error("source type may be a nullable");

      if (!request.TargetType.IsNullable()) return request.Error("target type needs to be a nullable");

      var nullableValueType = request.TargetType.GetNullableTypeArgument();


      var subRequest = request.Derive(request.Source, nullableValueType, request.SourceType);
      subRequest.Simulate = true;
      var converter = subRequest.Find(request.SourceType, request.TargetType);
      if (converter == null) return subRequest.Error("no converter found for which can convert type to nullable value's type");

      if (request.Simulate) return request.SimulateSuccess();
      var result = converter.Convert(subRequest);
      if (result.HasError) return request.Error("could not convert");
      return request.Success(System.Activator.CreateInstance(request.TargetType, subRequest.ExistingValue));
    }
  }
}
