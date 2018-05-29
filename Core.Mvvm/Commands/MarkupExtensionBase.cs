using System;
using System.Windows.Markup;
using Core.Common.Injection;

namespace Core.Common.Wpf
{
    public abstract class MarkupExtensionBase : MarkupExtension
    {

        private object _targetObject;
        private object _targetProperty;
        private IServiceProvider _serviceProvider;


        protected object TargetObject { get { return _targetObject; } }
        protected object TargetProperty { get { return _targetProperty; } }
        protected IServiceProvider ServiceProvider { get { return _serviceProvider; } }

        public sealed override object ProvideValue(IServiceProvider serviceProvider)
        {
            
            var pvt = serviceProvider.GetService<IProvideValueTarget>();
            var targetObject = pvt.TargetObject;
            
            if(targetObject==null)
            {
                return this;
            }
            
            // in case we are inside a template we need to defer evaluation to instanciation time
            // this is achieved by just returning this markup extension which will be reevaluated
            if(targetObject.GetType().Name.ContainsIgnoreCase("shareddp"))
            {
                return this;
            }
            _targetObject = pvt.TargetObject;
            _targetProperty = pvt.TargetProperty;
            _serviceProvider = serviceProvider;
            var value = ProvideValue();
            _serviceProvider = null;
            return value;
        }

        protected abstract object ProvideValue();
    }






}
