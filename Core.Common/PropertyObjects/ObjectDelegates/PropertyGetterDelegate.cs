using System;

namespace Core.Common
{
  public delegate bool PropertyGetterDelegate(object @object, string key, Type type, out object value);
}
