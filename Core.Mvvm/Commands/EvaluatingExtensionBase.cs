using System;
using System.Windows.Markup;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Injection;
using System.Xml;
using System.Diagnostics;
using System.Windows;

namespace Core.Common.Wpf.Export
{
    /// <summary>   A base class for Evaluating Extensions - Extensions which can contain recursive bound elements </summary>
    ///
    /// <remarks>   Toeb, 2018-05-09. </remarks>
    public abstract class EvaluatingExtensionBase : MarkupExtension, IMultiValueConverter
    {

        public EvaluatingExtensionBase(object[] arguments) { this.MultiArguments = arguments; }
        protected abstract object Evaluate(EvaluatingExtensionBase root, object[] values);

        public object[] MultiArguments { get; private set; }





        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var _ast = parameter as AstNode ?? throw new InvalidOperationException(); ;
            return _ast.Evaluate(this, new Queue<object>(values));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }



        private class AstNode
        {
            public override string ToString()
            {
                return $"Ast Input {InputObject} Value {Value}";
            }
            public object Value { get; internal set; }

            public MarkupExtension Provider
            {
                get; set;
            }
            public object InputObject
            {
                get; set;
            }
            public AstNode[] Children
            {
                get; set;
            }
            public IEnumerable<AstNode> Descendants
            {
                get
                {
                    return SelfAndDescendents.Skip(1);
                }
            }
            public IEnumerable<AstNode> SelfAndDescendents
            {
                get
                {
                    yield return this;
                    foreach (var child in Children.SelectMany(c => c.SelfAndDescendents))
                    {
                        yield return child;
                    }
                }
            }


            public static AstNode Parse(object current)
            {
                var result = new AstNode();
                result.InputObject = current;
                result.Children = Array.Empty<AstNode>();



                if (current is MultiBinding mb)
                {

                    if (mb.Converter is EvaluatingExtensionBase evaluatingExtensionBase)
                    {

                        result.Children = evaluatingExtensionBase.MultiArguments.Select(a => Parse(a)).ToArray();
                    }
                    else
                    {
                        throw new InvalidOperationException($"only multibindings of type {nameof(EvaluatingExtensionBase)} are supported");
                    }
                }


                if (current is MarkupExtension extension)
                {
                    result.Provider = extension;
                }
                return result;
            }
            public object Evaluate(EvaluatingExtensionBase root, Queue<object> values)
            {




                if (Provider is MultiBinding multi)
                {
                    if (multi.Converter is EvaluatingExtensionBase evb)
                    {
                        var items = Children.Select(child => child.Evaluate(root, values)).ToArray();
                        
                        return evb.Evaluate(root, items);
                    }
                }
                if (Provider != null)
                {
                    if (values.Count == 0)
                    {
                        return null;
                    }
                    return values.Dequeue();
                }


                return InputObject;

            }

            public object ProvideValue(IServiceProvider serviceProvider)
            {
                if (Provider == null)
                {
                    Value = InputObject;
                }
                else
                {
                    Value = Provider.ProvideValue(serviceProvider);
                }
                return Value;
            }
        }

        public DependencyObject TargetObject { get; protected set; }
        public object TargetProperty { get; protected set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetObject = serviceProvider.GetService<IProvideValueTarget>()?.TargetObject;
            TargetObject = targetObject as DependencyObject;
            if(targetObject !=null && TargetObject is null)
            {
                //shareddp - this extension is in template
                return this;
            }
            TargetProperty = serviceProvider.GetService<IProvideValueTarget>()?.TargetProperty;
            ServiceProvider = serviceProvider;
            OnProvideValue(serviceProvider);
            
            var _binding = new MultiBinding() { Converter = this };



            var  _ast = AstNode.Parse(_binding);
            _binding.ConverterParameter = _ast;

            var provided = _ast
                .Descendants
                .Select(n => n.ProvideValue(serviceProvider))
                .Where(v => v != null)
                .ToArray();

            var bindingExpressions = provided.OfType<BindingExpression>().ToArray();

            foreach (var binding in bindingExpressions)
            {
                _binding.Bindings.Add(binding.ParentBindingBase);
            }


            return _binding.ProvideValue(serviceProvider);
        }

        protected virtual void OnProvideValue(IServiceProvider provider)
        {

        }

    }






}
