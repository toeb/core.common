using Core.Values;
using System;
using System.Collections.Generic;

namespace Core.ManagedObjects
{
  public interface IManagedObjectValue : IManagedObject, IValue
  {

    /// <summary>
    /// should retunr info object for this ManagedObject
    /// </summary>
    IManagedObjectInfo ObjectInfo { get; }
  }
}
