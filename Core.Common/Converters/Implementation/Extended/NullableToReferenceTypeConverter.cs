
namespace Core.Converters
{
  public class NullableToReferenceTypeConverter : IExtendedConverter
  {
    public ConversionResponse Convert(ConversionRequest request)
    {
      var nullableType = request.SourceFormat.Type;
      var referenceType = request.TargetFormat.Type;

      if (!nullableType.IsNullable()) return request.Error("unexpected type: requires source type to be a nullable");
      if (referenceType.IsValueType) return request.Error("unexpected type: requires target type to be a reference type");


      var nullableValueType = nullableType.GetNullableTypeArgument();

      var from = request.Source;
      var to = request.ExistingValue;

      var subRequest = request.Derive(from, nullableValueType, to, referenceType);

      var converter = request.Find(subRequest);
      if (converter == null) return request.Error("no converter found");

      if (request.Simulate) return request.SimulateSuccess();

      if (from == null) return request.Success((object)null);
      var subResponse = converter.Convert(subRequest);

      return request.Success(subResponse);
    }
  }
}
