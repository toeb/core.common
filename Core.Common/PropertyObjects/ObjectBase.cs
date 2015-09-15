
namespace Core.Common
{
  public class ObjectBase : PropertyObject
  {
    protected T Get<T>([CallerMemberName] string key = null)
    {
      Property.CallerMemberName(ref key);
      return (T)Get(key, typeof(T));
    }
    protected void Set<T>(T value, [CallerMemberName] string key = null)
    {
      Property.CallerMemberName(ref key);
      Set(key, typeof(T), value);
    }
  
  }
}
