using Core.Common.Injection;
using Core.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace Core.Common.Wpf.Export
{
   

    /// <summary>  Allows the injection of an object of  type (name) at the specified location {Inject DomainModelViewModel} 
    ///            To be explicit you should specify the complete type using {x:Type ...} syntax
    ///            
    ///             An Extra service is done for binding the DataContext property
    ///             then you can try to add an empty inject DataContext="{Inject}" 
    ///             which will try to bind a viewmodel by convention
    ///             e.g. NameOfPage => NameOfViewModel, NameOfVM, NameOfPageViewModel,... </summary>
    ///
    /// <remarks>   Toeb, 2018-04-23. </remarks>
    public class InjectExtension : EvaluatingExtensionBase
    {


        protected override void OnProvideValue(IServiceProvider provider)
        {
            if (provider.TryResolveTypeString(TargetType as string, out var type))
            {
                TargetType = type;
            }

        }

        private InjectExtension(object targetType, object[] objects) : base(targetType.Yield().Concat(objects.AsEnumerable()).ToArray())
        {
            TargetType = targetType;

        }


        public InjectExtension() : this(null, new object[] { })
        {

        }
        public InjectExtension(object targetType) : this(targetType, new object[] { })
        {
        }
        public InjectExtension(object targetType, object arg0) : this(targetType, new[] { arg0 })
        {
        }

        public InjectExtension(object targetType, object arg0, object arg1) : this(targetType, new[] { arg0, arg1 })
        {
        }



        public InjectExtension(object targetType, object arg0, object arg1, object arg2) : this(targetType, new[] { arg0, arg1, arg2 })
        {
        }



        public object TargetType { get; set; }

        protected override object Evaluate(EvaluatingExtensionBase root, object[] values)
        {
            var targetType = values[0];
            var parameters = values.Skip(1).ToArray();

            if (TargetObject == null)
            {
                Factory = (targetObject) => targetObject.ResolveContent(targetType, parameters);
                return this;
            }
            (TargetObject as FrameworkElement).AfterLoad(() => AfterLoad(targetType, parameters));
            return this;

        }
        public Func<DependencyObject, object> Factory { get; set; }


        public static IEnumerable<string> ResolveViewModelNames(object view)
        {
            var possibleNames = new List<string>();
            var typename = view.GetType().Name;

            var suffixes = new[] { "ViewModel", "VM" };

            possibleNames.Add(typename);

            var controlSuffixes = new[] { "Page", "Control", "View", "Window", "Dialog" };
            var pattern = $"^(.+)({string.Join("|", controlSuffixes.Select(s => $"({s})"))})$";
            var match = Regex.Match(typename, pattern);
            if (match.Success)
            {
                possibleNames.Add(match.Groups[1].Value);
            }


            var q = from name in possibleNames
                    from suffix in suffixes
                    select name + suffix;

            return q;
        }

        public object Resolve(FrameworkElement targetObject, DependencyProperty property, object targetType, object[] parameters)
        {

            var resolved = targetObject.ResolveContent(targetType, parameters);
            if (resolved != null)
            {
                return resolved;
            }

            if (property == FrameworkElement.DataContextProperty)
            {
                foreach (var item in ResolveViewModelNames(targetObject))
                {
                    resolved = targetObject.ResolveContent(item,  parameters);
                    if (resolved != null)
                    {
                        return resolved;
                    }
                }
            }

            return null;
        }

        private object AfterLoad(object targetType, object[] parameters)
        {
            var frameworkElement = TargetObject as FrameworkElement;
            var property = TargetProperty as DependencyProperty;
            var resolved = Resolve(frameworkElement, property, targetType, parameters);
            frameworkElement?.SetValue(property, resolved);
            return resolved;

        }
    }
}