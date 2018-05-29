using System;
using System.Windows.Input;
using System.Diagnostics;
using Core.Common.Reflect;
using System.Collections.ObjectModel;
using System.Windows;
using Core.Common.MVVM;
using System.Linq;

namespace Core.Common.Wpf.Export
{


  

    public delegate void CommandCompletedHandler(object sender, CommandCompletedArgs e);
    public delegate void CommandStartedHandler(object sender, CommandStartedArgs e);
    public class CommandEventArgs : RoutedEventArgs
    {
        public CommandEventArgs(CommandExecutionContext context, RoutedEvent ev) : base(ev) { CommandContext = context; }
        public CommandExecutionContext CommandContext { get; private set; }

    }
    public class CommandCompletedArgs : CommandEventArgs
    {
        public CommandCompletedArgs(CommandExecutionContext context) : base(context, CommandScope.CommandCompletedEvent) { }
    }
    public class CommandStartedArgs : CommandEventArgs
    {
        public CommandStartedArgs(CommandExecutionContext context) : base(context, CommandScope.CommandStartedEvent) { }
    }

    /// <summary>
    /// CommandScope can be introduced into the Xaml Tree using CommandScope.IsScope="True"  this causes all commands which bubble up to be collected
    /// </summary>
    public class CommandScope
    {

        
        public static int GetDelay(DependencyObject obj)
        {
            return (int)obj.GetValue(DelayProperty);
        }

        public static void SetDelay(DependencyObject obj, int value)
        {
            obj.SetValue(DelayProperty, value);
        }

        // Using a DependencyProperty as the backing store for Delay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.RegisterAttached("Delay", typeof(int), typeof(CommandScope), new FrameworkPropertyMetadata(2000, FrameworkPropertyMetadataOptions.Inherits));

        


        public static ObservableCollection<CommandContextItem> GetContexts(DependencyObject obj)
        {
            return (ObservableCollection<CommandContextItem>)obj.GetValue(ContextsProperty);
        }

        public static void SetContexts(DependencyObject obj, ObservableCollection<CommandContextItem> value)
        {
            obj.SetValue(ContextsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Contexts.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContextsProperty =
            DependencyProperty.RegisterAttached("Contexts", typeof(ObservableCollection<CommandContextItem>), typeof(CommandScope), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));


        public enum ScopeAction
        {

            /// <summary>  Causes the command contexts to pass through this element. </summary>
            Ignore,

            /// <summary>   Causes this element to listen to the command contexts </summary>
            Listen,

            /// <summary>   Causes this element to listen and keep command contexts not propagating them further</summary>

            Keep
        }


        public static ScopeAction GetAction(DependencyObject obj)
        {
            return (ScopeAction)obj.GetValue(ActionProperty);
        }

        public static void SetAction(DependencyObject obj, ScopeAction value)
        {
            obj.SetValue(ActionProperty, value);
        }

        // Using a DependencyProperty as the backing store for Action.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.RegisterAttached("Action", typeof(ScopeAction), typeof(CommandScope), new PropertyMetadata(new PropertyChangedCallback(IsCommandScopeChanged)));

        private static void IsCommandScopeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var isScope = GetAction(d);
            if (isScope != ScopeAction.Ignore)
            {
                SetContexts(d, new ObservableCollection<CommandContextItem>());
                var fe = d as UIElement;
                fe.AddHandler(CommandStartedEvent, new CommandStartedHandler((sender, args) =>
                {
                    var contexts = GetContexts(d);

                    contexts.Add(new CommandContextItem(args.CommandContext, d));
                    if(isScope==ScopeAction.Keep)
                    {
                        args.Handled = true;
                    }

                }));
                fe.AddHandler(CommandCompletedEvent, new CommandCompletedHandler((sender, args) =>
                {
                    var contexts = GetContexts(d);
                    var context = contexts.FirstOrDefault(it => Equals(it.CommandContext, args.CommandContext));
                    context?.AutoRemove(DateTime.Now + TimeSpan.FromMilliseconds(GetDelay(d)));
                }));
            }
            else
            {
                SetContexts(d, null);

            }

        }


        public static readonly RoutedEvent CommandCompletedEvent = EventManager.RegisterRoutedEvent("CommandCompleted", RoutingStrategy.Bubble, typeof(CommandCompletedHandler), typeof(CommandScope));
        public static void AddCommandCompletedHandler(DependencyObject d, RoutedEventHandler handler)
        {


            if (d is UIElement uie)
            {
                uie.AddHandler(CommandCompletedEvent, handler);
            }
        }
        public static void RemoveCommandCompletedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie)
            {
                uie.RemoveHandler(CommandCompletedEvent, handler);
            }
        }




        public static readonly RoutedEvent CommandStartedEvent = EventManager.RegisterRoutedEvent("CommandStarted", RoutingStrategy.Bubble, typeof(CommandStartedHandler), typeof(CommandScope));
        public static void AddCommandStartedHandler(DependencyObject d, RoutedEventHandler handler)
        {


            if (d is UIElement uie)
            {
                uie.AddHandler(CommandStartedEvent, handler);
            }
        }
        public static void RemoveCommandStartedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie)
            {
                uie.RemoveHandler(CommandStartedEvent, handler);
            }
        }

        internal static async void NotifyCommandStarted(FrameworkElement targetObject, CommandExecutionContext context)
        {

            targetObject.RaiseEvent(new CommandStartedArgs(context) { });
            await context.SignalTask.ConfigureAwait(true);
            targetObject.RaiseEvent(new CommandCompletedArgs(context) { });
        }
    }






}
