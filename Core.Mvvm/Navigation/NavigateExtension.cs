using Core.Common.MVVM;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace Core.Common.Wpf.Export
{



    /// <summary>   A navigation extension.  Provides a command that can be used to navigate to the specified Argument.
    ///             Allows the client programmer to specify what to navigate to where to show it 
    ///             The Target property is usually a string specifying the viewport </summary>
    /// 
    /// <code>
    ///     <Button Command={Navigate SomeUri/ToAPage.xaml}/>
    ///     <Button Command={Navigate SomeViewModelOrViewTypeName}/>
    ///     <Button Command={Navigate {x:Type vm:somViewModelType}}/>
    ///     <Button Command={Navigate {x:Type vm:SomeViewType}}/>
    ///       </code>
    ///
    /// <remarks>   Toeb, 2018-04-23. </remarks>
    [MarkupExtensionReturnType(typeof(BindingExpression))]
    public class NavigateExtension : EvaluatingExtensionBase
    {
        public NavigateExtension(object content) : base(new object[2])
        {
            Content = content;
        }
      
        protected override void OnProvideValue(IServiceProvider provider)
        {

            if (provider.TryResolveTypeString(Content as string, out Type type))
            {
                Content = type;
            }



        }

        /// <summary>
        /// Target where to display the result of the navigation
        /// 
        /// </summary>
        public object Target { get { return MultiArguments[0]; } set { MultiArguments[0] = value; } }

        /// <summary>   The Argument What to display can be any kind of object or type
        ///             Types will be constructed / injected
        ///             (relative)Pack Uris will be loaded (page)
        ///             can be a binding to either
        ///             </summary>
        ///
        /// <value> The argument. </value>
        public object Content { get { return MultiArguments[1]; } set { MultiArguments[1] = value; } }


        protected override object Evaluate(EvaluatingExtensionBase root, object[] values)
        {
            var target = values[0];
            var content = values[1];


            return new NavigationCommand(
                content as ICommand ?? DelegateCommand.Create(() => content),
                this,
                target,
                content
                );



        }

        public object ResolveContent(object content)
        {
            if (content is InjectExtension injectExtension)
            {
                return injectExtension.Factory?.Invoke(TargetObject as DependencyObject);

            }
            return (TargetObject as DependencyObject).ResolveContent(content);
        }

        public static ViewHandle CreateViewHandle(object source, DependencyObject TargetObject, object target)
        {
            var viewHandle = new ViewHandle();
            viewHandle.Source = source;
            viewHandle.Template = NavigationTarget.GetTargetTemplate(TargetObject);
            viewHandle.Contract = ResolveViewPort(TargetObject, target);
            return viewHandle;
        }

        public static object ResolveViewPort(DependencyObject TargetObject, object Target)
        {

            object target = NavigationTarget.GetViewPort(Target as string);
            if (target != null)
            {
                return target;
            }
            if (Target != null)
            {
                return Target;
            }




            target = NavigationTarget.GetDefault(TargetObject);
            if (target is string uri)
            {
                target = NavigationTarget.GetViewPort(uri);
                return target;
            }
            if (target != null)
            {
                return target;
            }



            return null;

        }

        /// <summary>   A navigation command. used by command extension to correctly hook up
        ///             the view to be shown </summary>
        ///
        /// <remarks>   Toeb, 2018-04-26. </remarks>
        public class NavigationCommand : CommandBase
        {
            public ICommand Command { get; private set; }
            public NavigateExtension Extension { get; private set; }

            private object _target;
            private object _content;

            public NavigationCommand(ICommand inner, NavigateExtension extension, object target, object contetn)
            {
                Command = inner;
                Command.CanExecuteChanged += CommandCanExecuteChanged;
                this.Extension = extension;
                this._target = target;
                this._content = contetn;
                //  Name = extension.TargetObject.Name;
            }

            public string Name { get; set; }




            private void CommandCanExecuteChanged(object sender, EventArgs e)
            {
                RaiseCanExecuteChanged();
            }



            public override bool CanExecute(object parameter)
            {
                return Command.CanExecute(parameter);
            }

            public override void Execute(object parameter)
            {


                var vh = NavigateExtension.CreateViewHandle(Extension, Extension.TargetObject, _target);

                using (var ctx = CommandExecutionContext.Require(Command, parameter))
                {
                    ctx.SetViewHandle(vh);
                    ctx.SignalTask.ContinueWith(t =>
                    {
                        vh.Content = Extension.ResolveContent(ctx.CommandResult);
                        var handler = ViewHandlers.TryHandle(vh);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    Command.Execute(parameter);
                }

            }

        }


    }
}