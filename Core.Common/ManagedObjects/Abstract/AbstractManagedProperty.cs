
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
  /// <summary>
  /// default abstract base for IManagedProperty
  /// </summary>
  [DebuggerDisplay("{Info.Name}: '{Value}'")]
  public abstract class AbstractManagedProperty : AbstractValue, IManagedProperty
  {
    public AbstractManagedProperty(ManagedPropertyInfo info)
      : base(info)
    {
      Info = info;
    }

    public AbstractManagedProperty(string name, bool isReadable, bool isWritable, Type valueType)
      : this(new ManagedPropertyInfo(name,isReadable,isWritable,valueType,false))
    {

    }

    /// <summary>
    /// Convenience Accessor to Managed Property Info
    /// </summary>
    protected new ManagedPropertyInfo Info { get; private set; }


    public IPropertyInfo PropertyInfo
    {
      get { return Info; }
    }
  }
}
