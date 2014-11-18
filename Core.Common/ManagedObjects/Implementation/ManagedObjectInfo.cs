using Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

namespace Core.ManagedObjects
{
  public class ManagedObjectInfo : ValueInfo, IManagedObjectInfo
  {
    public ManagedObjectInfo(bool readable,bool writable, Type type, bool onlyExact):base(readable,writable,type,onlyExact){}
    public new static ManagedObjectInfo MakeDefault()
    {
      return new ManagedObjectInfo(false, false, null, false);
    }
  }
}
