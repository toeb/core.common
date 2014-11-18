
namespace Core.Converters
{
  public class NullableToValueTypeConverter : IExtendedConverter
  {

    public ConversionResponse Convert(ConversionRequest request)
    {


      if (!request.SourceType.IsNullable()) return request.Error("unexpected type: source type is not nulable");
      if (!request.TargetType.IsValueType) return request.Error("unexpected type: target type is not a value type");
      if (request.TargetType.IsNullable()) return request.Error("unexpected type: target type may not be a nullable type");
      var nullableValueType = request.SourceType.GetNullableTypeArgument();



      if (request.Source == null) return request.Error("cannot convert nullable without value to value type");



      var nullable = request.SourceType.GetNullableTypeArgument();
      var getValue = request.SourceType.GetProperty("Value");
      var value = getValue.GetValue(request.Source);


      var subRequest = request.Derive(value, nullableValueType, null, request.TargetType);
      
      var converter = request.Find(subRequest);
      if (converter == null) return request.Error("could not find converter");
      

      var subResponse = converter.Convert(subRequest);

      if (subResponse.HasError) return request.Error(subResponse, "failed to convert");

      return request.Success(subResponse);

    }
  }
}
