using Core;
using System;
using System.IO;

namespace Core.Formatters
{
  public class FormattingContext :ScopeBase
  {
    public Type Type { get { return Get<Type>(); } set { Set(value); } }
  
    public virtual void DataError(string msg){throw new Exception(msg); }
    public virtual void SyntaxError(string msg) { throw new Exception(msg); }
  }
}
