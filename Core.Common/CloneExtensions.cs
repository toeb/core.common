using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Common
{
  public static class CloneExtensions
  {
    public static T DeepClone<T>(this T original)
    {
      using (var ms = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, original);
        ms.Position = 0;
        return (T)formatter.Deserialize(ms);
      }
    }
  }
}
