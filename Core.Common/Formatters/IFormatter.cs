using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Formatters
{

  public interface IReader
  {
    bool CanRead(ReadContext context);
    object Read(ReadContext context);
  }
  public interface IWriter
  {
    bool CanWrite(WriteContext context);
    void Write(WriteContext context);
  }

  

}
