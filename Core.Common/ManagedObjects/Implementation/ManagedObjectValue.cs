using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.ManagedObjects
{
  public class ManagedObjectValue : AssignableManagedObject
  {
    private object instance;
    private static readonly string InstanceName = "Instance";
    public object Instance
    {
      get { return GetInstance(); }
      set { SetInstance(value, CombinationPolicy.Overwrite); }
    }

    public void SetInstance(object instance, CombinationPolicy policy)
    {
      ChangeIfDifferentAndCallback(ref this.instance, instance, InstanceChanging, InstanceChanged, InstanceName);
    }
    public object GetInstance()
    {
      return instance;
    }

    private void InstanceChanged(object oldValue, object newValue)
    {

    }
    private void InstanceChanging(object oldValue, object newValue) { }

    public ManagedObjectValue()
      : base(new ManagedObjectInfo(true, true, typeof(object), false))
    {

    }

    protected override void ConsumeValue(object value)
    {
      Instance = value;
    }
    protected override object ProduceValue()
    {
      return Instance;
    }

  }
}
