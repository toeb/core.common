using System.Collections.Generic;

namespace Core
{
  public interface IKeyValueStore
  {
    bool TryGet(string key, out object value);
    bool TrySet(string key, object value);
    bool HasKey(string key);
    IEnumerable<string> GetKeys();
  }
}
