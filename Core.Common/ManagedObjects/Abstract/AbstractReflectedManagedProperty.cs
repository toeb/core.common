
namespace Core.ManagedObjects
{
  public abstract class AbstractReflectedManagedProperty : AbstractManagedProperty
  {
    protected AbstractReflectedManagedProperty(ReflectedManagedPropertyInfo info)
      : base(new ManagedPropertyInfo(info))
    {
      ReflectionInfo = info;
    }

    public ReflectedManagedPropertyInfo ReflectionInfo { get; private set; }
  }
}
