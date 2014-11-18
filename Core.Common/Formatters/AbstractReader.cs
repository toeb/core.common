using System;
using System.Threading.Tasks;

namespace Core.Formatters
{

  public abstract class AbstractReader : IReader
  {
    protected abstract bool CanReadType(Type type);
    protected abstract object ReadObject(ReadContext context);
    protected abstract object CreateValue(ReadContext context);
    protected virtual void BeforeRead(ReadContext context) { }
    protected virtual ReadContext TransformContext(ReadContext context) { return context; }
    protected virtual void AfterRead(object value, ReadContext context) { }


    public bool CanRead(ReadContext ctx)
    {
      return CanReadType(ctx.Type);
    }

    public object Read(ReadContext context)
    {
      context.CurrentReader = this;
      BeforeRead(context);
      context = TransformContext(context);
      if (context.ExistingValue == null)
      {
        context.ExistingValue = CreateValue(context);
      }
      var result =  ReadObject(context);
      AfterRead(result, context);
      return result;
    }
  }
  
}
