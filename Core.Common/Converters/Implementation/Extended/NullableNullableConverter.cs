using System;
using System.Reflection;

namespace Core.Converters
{
  public class NullableNullableConverter : IExtendedConverter
  {
    public bool CanConvert(ConversionRequest request)
    {


      var from = request.SourceFormat.Type.GetNullableTypeArgument();
      var to = request.TargetFormat.Type.GetNullableTypeArgument();

      var converter = request.Find(from, to);
      return converter != null;
    }

    PropertyInfo hasValue = typeof(Nullable<>).GetProperty("HasValue");
    PropertyInfo value = typeof(Nullable<>).GetProperty("Value");


    public ConversionResponse Convert(ConversionRequest request)
    {
      var fromNullable = request.SourceType;
      var toNullable = request.TargetType;

      if (!fromNullable.IsNullable()) return request.Error("source type needs to be nullable");
      if (!toNullable.IsNullable()) return request.Error("target type needs to be nullable");


      var fromType = request.SourceType.GetNullableTypeArgument();
      var toType = request.TargetType.GetNullableTypeArgument();


      var from = request.Source;
      var to = request.ExistingValue;

      var hasValue = fromNullable.GetProperty("HasValue");
      var valueFrom = fromNullable.GetProperty("Value");


      if (from == null || !((bool)hasValue.GetValue(from)))
        return request.Simulate ? request.SimulateSuccess() : request.Success(Activator.CreateInstance(toNullable));
      
        var value = valueFrom.GetValue(from);
      
      var subRequest = request.Derive(value, fromType, null, toType);

      var converter = request.Find(subRequest);

      if (converter == null) return request.Error("no converter found");

      var subResponse = converter.Convert(subRequest);
      if(subResponse.HasError)return request.Error(subResponse, "failed to convert values");
      var result = Activator.CreateInstance(toNullable, subResponse.Result);
      return request.Success(result);
    }

  }
}
