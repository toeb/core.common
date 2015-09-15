using System;

namespace Core.Common
{
  public delegate bool PropertySetterDelegate(object @object, string key, Type type, object value);
}
