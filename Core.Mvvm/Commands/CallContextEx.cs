using System;
using System.Runtime.Remoting.Messaging;

namespace Core.Common.MVVM
{
    public static class CallContextEx
    {
        private static object _lock = new object();
        public static T Require<T>(string key, Func<T> provider)
        {
            lock (_lock)
            {
                T result;
                if (TryGet(key, out result))
                {
                    return result;
                }
                result = provider();
                CallContext.LogicalSetData(key, result);
                return result;
            }
        }
        public static T GetOrDefault<T>(string key, T defaultValue)
        {
            return GetOrDefault(key, () => defaultValue);
        }

        public static T GetOrDefault<T>(string key, Func<T> defaultProvider)
        {
            T result;
            if (!TryGet(key, out result))
            {
                return defaultProvider();
            }
            return result;
        }
        public static T GetOrDefault<T>(string key)
        {
            return GetOrDefault(key, () => default(T));
        }

        public static bool TryGet<T>(string key, out T value)
        {
            var data = CallContext.LogicalGetData(key);
            if (data == null)
            {
                value = default(T);
                return false;
            }

            if (!(data is T))
            {
                value = default(T);
                return false;
            }
            value = (T)data;
            return true;

        }
    }
}

