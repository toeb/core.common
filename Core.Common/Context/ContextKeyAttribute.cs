using System;

namespace Core
{
  public class ContextKeyAttribute : System.Attribute { public ContextKeyAttribute(string key) { if (string.IsNullOrEmpty(key))throw new ArgumentException("key may not be null or empty", "key"); Key = key; } public string Key { get; set; } }
}