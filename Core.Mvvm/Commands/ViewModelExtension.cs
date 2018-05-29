using System;
using System.ComponentModel;

namespace Core.Common.Wpf.Export
{

    public static class ConversionExtensions
    {
        public static object ConvertTo(this object source, Type targetType)
        {
            if(targetType.IsAssignableFrom(source?.GetType()))
            {
                return source;
            }

            return TypeDescriptor.GetConverter(targetType).ConvertFrom(source);
        }

        public static T ConvertTo<T>(this object source)
        {
            return (T)source.ConvertTo(typeof(T));
        }

    }

    public class ViewModelExtension : MarkupExtensionBase
    {
        public ViewModelExtension(object anything)// possibly parameters
        {
            Type = anything.ConvertTo<Type>();
            Parameters = new object[] { };
        }

        public object[] Parameters { get; set; }
        public Type Type { get; private set; }


        

        protected override object ProvideValue()
        {
            // get injection service, 
            // construct using arguments.
            var injectionService = new Injection.InjectionService(); // get injection services
            return injectionService.Construct(Type, Parameters);
        }
    }






}
