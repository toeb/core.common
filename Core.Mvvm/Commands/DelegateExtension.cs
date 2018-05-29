using System;
using Core.Common.Reflect;
using System.Windows.Data;

namespace Core.Common.Wpf.Export
{
    /// <summary>   an extension which converts the input optional target and delegate string
    ///             to a System.Delegate </summary>
    ///
    /// <remarks>   Toeb, 2018-05-10. </remarks>
    public class DelegateExtension : EvaluatingExtensionBase
    {
        private Type[] _types;

        public object Target { get => MultiArguments[0]; set => MultiArguments[0] = value; }
        public object Delegate { get => MultiArguments[1]; set => MultiArguments[1] = value; }

        public DelegateExtension(object target, object @delegate, Type[] typeList) : base(new object[2] { target, @delegate })
        {
            this._types = typeList;
        }

        public DelegateExtension(object target, object @delegate) : this(target, @delegate, (Type[])null)
        {

        }
        public DelegateExtension(object target, object @delegate, Type arg1Type) : this(target, @delegate, new[] { arg1Type })
        {

        }
        public DelegateExtension(object target, object @delegate, Type arg1Type, Type arg2Type) : this(target, @delegate, new[] { arg1Type, arg2Type })
        {

        }

        public DelegateExtension(object @delegate) : this(new Binding(), @delegate, (Type[])null)
        {

        }
        public DelegateExtension(object @delegate, Type arg1Type) : this(new Binding(), @delegate, new[] { arg1Type })
        {

        }
        public DelegateExtension(object @delegate, Type arg1Type, Type arg2Type) : this(new Binding(), @delegate, new[] { arg1Type, arg2Type })
        {

        }
        public DelegateExtension() : this(new Binding(), null) { }
        public static Delegate ResolveDelegate(object target, Type targetType, Type[] argTypes, object @delegate)
        {
            if(targetType ==null)
            {
                return null;
            }
            if (@delegate == null)
            {
                return null;
            }
            if (@delegate is string methodName)
            {

                var methodInfo = argTypes == null ? targetType.GetMethod(methodName) : targetType.GetMethod(methodName, argTypes);
                if (methodInfo == null)
                {
                    return null;
                }
                return methodInfo.CreateDelegate(target);
            }
            return null;

        }
        protected override object Evaluate(EvaluatingExtensionBase root, object[] values)
        {
            var target = values[0];
            var targetType = target as Type ?? target?.GetType();
            target = target is Type ? null : target;
            var @delegate = values[1];
            return ResolveDelegate(target, targetType, _types, @delegate);
        }


    }






}
