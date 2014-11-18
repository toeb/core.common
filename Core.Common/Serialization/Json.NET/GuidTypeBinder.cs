using System;
using Newtonsoft.Json.Serialization;

namespace Core.Serialization.Json
{
  public class GuidBinder : DefaultSerializationBinder
  {

    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
      var assembly = serializedType.Assembly;
      //var guid = assembly.GetGuid();
      var id = serializedType.GUID;
      typeName = id.ToString();
      assemblyName = string.Empty;
      //base.BindToName(serializedType, out assemblyName, out typeName);
    }
    public override Type BindToType(string assemblyName, string typeName)
    {

      Guid id = Guid.Parse(typeName);
      var type = ReflectionService.Instance.GetById(id);
      //var result = base.BindToType(assemblyName, typeName);
      return type;
    }
  }
}