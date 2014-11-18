using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Formatters
{
  public abstract class AbstractWriter : IWriter
  {
    protected abstract bool CanWriteType(Type type);
    protected abstract void WriteObject(WriteContext context);
    public bool CanWrite(WriteContext ctx)
    {
      return CanWriteType(ctx.Type);
    }
    public void Write(WriteContext context)
    {
      context.CurrentWriter = this;
      WriteObject(context);
    }

    
  }
}
