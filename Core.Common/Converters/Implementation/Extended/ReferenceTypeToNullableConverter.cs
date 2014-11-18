
using System;
namespace Core.Converters
{
  public class ReferenceTypeToNullableConverter : IExtendedConverter
  {

    public bool CanConvert(ConversionRequest request)
    {
      var nullableType = request.TargetType;
      var referenceType = request.SourceType;


      if (!nullableType.IsNullable() || referenceType.IsValueType) return false;
      var nullableValueType = nullableType.GetNullableTypeArgument();
      var from = referenceType;
      var to = nullableValueType;
      var converter = request.Find(from, to);
      return converter != null;
    }

    public ConversionResponse Convert(ConversionRequest request)
    {

      var nullableType = request.TargetType;
      var referenceType = request.SourceType;

      if (!nullableType.IsNullable()) return request.Error("target type is not nullable");
      if (referenceType.IsValueType) return request.Error("source type is not a reference type");


      var from = request.Source;
      var to = request.ExistingValue;
      var nullableValueType = nullableType.GetNullableTypeArgument();

      var subRequest = request.Derive(from, referenceType, to, nullableValueType);
      var converter = request.Find(subRequest);

      if (converter == null) return request.Error("could not find converter");
      if (request.Simulate) return request.SimulateSuccess();

      if (from == null) return request.Success(Activator.CreateInstance(nullableType));


      var subResponse = converter.Convert(subRequest);

      return request.Success(subResponse);
      request.ExistingValue = System.Activator.CreateInstance(nullableType, subRequest.ExistingValue);




    }
  }
}
