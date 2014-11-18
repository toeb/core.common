using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Json
{

  class MyContractResolver :
    IContractResolver
  {
    private IContractResolver contractResolver;

    public MyContractResolver(IContractResolver contractResolver)
    {
      // TODO: Complete member initialization
      this.contractResolver = contractResolver;
    }

    public JsonContract ResolveContract(Type type)
    {
      var contract = contractResolver.ResolveContract(type);
      return contract;
    }
  }
  class MyReferenceResolver : IReferenceResolver
  {
    private IReferenceResolver referenceResolver;

    public MyReferenceResolver(IReferenceResolver referenceResolver)
    {
      // TODO: Complete member initialization
      this.referenceResolver = referenceResolver;
    }


    public void AddReference(object context, string reference, object value)
    {
      referenceResolver.AddReference(context, reference, value);
    }

    public string GetReference(object context, object value)
    {
      var reference =  referenceResolver.GetReference(context, value);
      return reference;
    }

    public bool IsReferenced(object context, object value)
    {
      var isReferenced = referenceResolver.IsReferenced(context, value);
      return isReferenced;
    }

    public object ResolveReference(object context, string reference)
    {
      var resolvedReference  =referenceResolver.ResolveReference(context, reference);
      return resolvedReference;
    }
  }
  public class EntitySerializer : JsonSerializer
  {
    public EntitySerializer()
    {
      var serializer = this;

      /*
      Http.Configuration.Formatters.JsonFormatter.Indent = true;
      Http.Configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
      Http.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
      Http.Configuration.Formatters.JsonFormatter.SerializerSettings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
      Http.Configuration.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
      Http.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new Resolver();
      Http.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
       * */
      serializer.Converters.Add(new StringEnumConverter() { CamelCaseText = true });
      serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
      serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
      serializer.TypeNameHandling = TypeNameHandling.Objects;
      serializer.ContractResolver = new ProxyContractResolver(serializer.ContractResolver);
      //serializer.Binder = new MyBinder();
      serializer.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
      serializer.ReferenceResolver = new MyReferenceResolver(serializer.ReferenceResolver);
      serializer.Formatting = Formatting.Indented;

    }
  }
  class ProxyContractResolver : IContractResolver
  {
    IContractResolver resolver;
    public ProxyContractResolver(IContractResolver resolver)
    {
      this.resolver = resolver;
    }
    public JsonContract ResolveContract(Type type)
    {
      if (type.Namespace == "System.Data.Entity.DynamicProxies")
      {
        return resolver.ResolveContract(type.BaseType);
      }
      return resolver.ResolveContract(type);
    }
  }
}
