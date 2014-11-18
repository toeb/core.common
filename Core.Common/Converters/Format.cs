using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions.Reflection;
namespace Core.Converters
{
  [DebuggerDisplay("{Name}.{TypeName}")]
  public class Format : IFormat
  {
    public Format(Type type, string name)
    {
      if (type == null) throw new ArgumentNullException("type");
      if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");
      Name = name;
      Type = type;
    }
    public string Name { get; set; }
    public Type Type { get; set; }
    public string TypeName
    {
      get
      {
        return Type.Name;
      }
    }
    public override string ToString()
    {
      return string.Format("{0}.{1}", Name, Type.Name);
    }
    public override bool Equals(object obj)
    {
      return base.Equals(obj) || obj is Format && (obj as Format == this);
    }
    public static bool operator ==(Format a, Format b)
    {
      return a.Name == b.Name && a.Type == b.Type;
    }
    public static bool operator !=(Format a, Format b)
    {
      return !(a == b);
    }
    public bool IsAssignableFrom(IFormat other)
    {
      if (other.Name != this.Name) return false;
      if (this.Type.IsSuperclassOfOrSameClass(other.Type)) return true;
      return false;
    }
  }
}
