using Core.Common.MVVM;
using Core.Common.Reflect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Core.Common.Wpf.Export
{
    public class ArgumentEvaluator : IMethodArguments
    {
        private object[] _arguments;

        public ArgumentEvaluator( object[] arguments)
        {
            _arguments = arguments ?? new object[] { };
        }
        public object[] Evaluate(Type[] expectedTypes)
        {
            var result = _arguments
                .Select((o, i) => TryConvert(expectedTypes[i], o))
                .ToArray();
            return result;


        }

        static (bool success, IEnumerable value) EnumerableToTypedEnumerable(object value, Type targetType)
        {
            if(value==null)
            {
                return (false, null);
            }
            var elementType = targetType.GetGenericEnumerableElementType();
            if(elementType ==null)
            {
                return (false,null);
            }
            if(value.GetType().IsGenericType )
            {                
                return (false, null);
            }
            if(!(value is IEnumerable<object>) && value.GetType().GetGenericEnumerableInterfaces().Any())
            {
                return (false, null);
            }

            var method = typeof(Enumerable).GetMethod(nameof(Enumerable.OfType));
            var methodInstance =method.MakeGenericMethod(elementType);
            var result =methodInstance.Invoke(null, new[] { value });
            return (true, result as IEnumerable);
        }

        static object TryConvert(Type targetType, object value)
        {
            if (value == null)
            {
                return null;
            }

            if (targetType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }


            var enumerable = EnumerableToTypedEnumerable(value, targetType);
            if(enumerable.success)
            {
                return enumerable.value;
            }
            return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);

        }

    }






}
