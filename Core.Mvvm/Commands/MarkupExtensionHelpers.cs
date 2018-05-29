using System;
using System.Windows.Markup;

namespace Core.Common.Wpf.Export
{
    public static class MarkupExtensionHelpers
    {
        public static bool TryResolveTypeString(this IServiceProvider provider, string typeName, out Type type)
        {
            type = null;
            if(typeName==null)
            {
                return false;
            }

            if (!typeName.Contains(":") || typeName.Contains("/"))
            {
                return false;
            }


            try
            {
                type = new TypeExtension(typeName).ProvideValue(provider) as Type;
                return true;
            }
            catch
            {
                return false;
            }



        }
    }






}
