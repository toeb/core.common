using Core.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization
{

  public class ConfigurationJsonConverter : MappedConverter
  {
    public override bool UseProperty(PropertyInfo info)
    {
      return info.HasAttribute<ConfigurableAttribute>();
    }
    public override bool CanConvert(Type objectType)
    {
      return !objectType.IsValueType;

    }
    protected override void HandleUncomputedProperties(IEnumerable<PropertyInfo> uncomputedProperties, object obj)
    {
      foreach (var prop in uncomputedProperties)
      {

        var defaultAttribute = prop.GetCustomAttribute<DefaultValueAttribute>();
        if (defaultAttribute == null) continue;
        prop.SetValue(obj, defaultAttribute.Value);
      }
    }
    protected override void Annotate(JsonWriter writer, PropertyInfo property, object value, JsonSerializer serializer)
    {

      var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
      var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();


      string comment = "";
      if (displayAttribute != null)
      {
        comment += displayAttribute.Description + ". ";
      }

      if (defaultValueAttribute != null)
      {
        StringWriter tmp = new StringWriter();

        serializer.Serialize(tmp, value);
        comment += "default value is '" + tmp.ToString() + "'. ";
      }
      if (string.IsNullOrEmpty(comment)) return;
      writer.WriteComment(comment);
    }
  }
}
