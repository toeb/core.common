using Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{
  [DebuggerDisplay("{Name}: {ValueType}")]
  public class ManagedPropertyInfo : ValueInfo, IPropertyInfo
  {
    public override string ToString()
    {
      return string.Format("{0}: {ValueType} Id: {Id}", Name,ValueType);
    }
    public ManagedPropertyInfo(IPropertyInfo info) : this(info.Name,info.IsReadable,info.IsWriteable,info.ValueType,info.OnlyExactType ) { }
    public ManagedPropertyInfo(PropertyInfo info)
      :this(info.Name,info.CanRead,info.CanWrite,info.PropertyType,false)
    {

    }
    public ManagedPropertyInfo(string name, bool isReadable, bool isWritable, Type valueType, bool exactType) : base(isReadable, isWritable, valueType, exactType) { this.Name = name; }
    private string name;
    public string Name
    {
      get { return name; }
      set { ChangeIfDifferentAndCallback(ref name, value, NameChanging, NameChanged, NameName); }
    }
    private static readonly string NameName = "Name";
    protected virtual void NameChanged(string oldValue, string newValue) { }
    protected virtual void NameChanging(string oldValue, string newValue) { }


    
    public string Id
    {
      get { return Name + ":" + ValueType.Namespace.Normalize() + ValueType.Name; }
    }
  }
}
