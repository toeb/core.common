using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
namespace Core.Formatters
{
  public abstract class AbstractReader<T> : AbstractReader
  {
    protected abstract object ReadObject(T existingValue, ReadContext context);
    protected abstract T CreateTypedValue(ReadContext context);

    protected bool IsForType(Type type)
    {
      return typeof(T).IsAssignableFrom(type);
    }

    protected override bool CanReadType(Type type)
    {
      return IsForType(type);
    }

    protected sealed override  object ReadObject(ReadContext context)
    {
      if (context.ExistingValue != null && !(context.ExistingValue is T)) throw new InvalidOperationException("existing value is not correct type");
      var result = ReadObject((T)context.ExistingValue, context);
      return result;
    }

    protected sealed override object CreateValue(ReadContext context)
    {
      return CreateTypedValue(context);
    }
  }



  public abstract class AbstractWriter<T> : AbstractWriter
  {
    protected abstract void WriteObject(T value, WriteContext context);
    
    protected override bool CanWriteType(Type type)
    {
      return IsForType(type);
    }

    protected bool IsForType(Type type)
    {
      return typeof(T).IsAssignableFrom(type);
    }

    protected sealed override  void WriteObject(WriteContext context)
    {
      var value = context.Value;
      if (value == null) value = default(T);
      if (value !=null &&!(value is T)) context.DataError("value is not correct type");
      WriteObject((T)value, context);
    }
  }
}
