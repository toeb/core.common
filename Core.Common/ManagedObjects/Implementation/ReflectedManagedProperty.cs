using System;

namespace Core.ManagedObjects
{
  public class ReflectedManagedProperty : AbstractReflectedManagedProperty
  {
    public ReflectedManagedProperty(object instance, ReflectedManagedPropertyInfo info)
      : base(info)
    {
      if (instance == null) throw new ArgumentNullException("instance");
      Instance = instance;

    }

    public object Instance { get; private set; }
    protected override object ProduceValue()
    {
      
      return ReflectionInfo.Info.GetValue(Instance);
    }

    protected override void ConsumeValue(object value)
    {
      ReflectionInfo.Info.SetValue(Instance, value);
    }
  }
}
