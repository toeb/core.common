using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Core.Common.Wpf.Export
{

    public class Host
    {




        public static IEnumerable<object> GetControls(DependencyObject obj)
        {
            return (IEnumerable<object>)obj.GetValue(ControlsProperty);
        }

        public static void SetControls(DependencyObject obj, IEnumerable<object> value)
        {
            obj.SetValue(ControlsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Controls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlsProperty =
            DependencyProperty.RegisterAttached("Controls", typeof(IEnumerable<object>), typeof(Host), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));






        public static object GetControl(DependencyObject obj)
        {
            return (object)obj.GetValue(ControlProperty);
        }

        public static void SetControl(DependencyObject obj, object value)
        {
            obj.SetValue(ControlProperty, value);
        }

        // Using a DependencyProperty as the backing store for Control.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlProperty =
            DependencyProperty.RegisterAttached("Control", typeof(object), typeof(Host), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ControlChanged)));

        private static void ControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.AfterLoad(() => {

                foreach(var host in d.EnumerateParentsAndThis())
                {
                    var list = GetControls(host) ?? new object[] { };

                    SetControls(host, list.Except(new[] { e.OldValue }).ConcatOne(new[] { e.NewValue }).ToArray());
                }
            });
            d.AfterUnLoad(() =>
            {

                foreach (var host in d.EnumerateParentsAndThis())
                {
                    var list = GetControls(host) ?? new object[] { };

                    SetControls(host, list.Except(new[] { GetControl(d) }).ToArray()) ;
                }
            });
        }
    }

    [MarkupExtensionReturnType(typeof(object))]
    public abstract class CommandExtensionBase : EvaluatingExtensionBase
    {

        protected CommandExtensionBase(object command, object[] args):base(new object[] { command}.Concat(args).ToArray())
        {

        }
        public object Command { get { return MultiArguments[0]; } set { MultiArguments[0] = value; } }

        public new FrameworkElement TargetObject { get { return base.TargetObject as FrameworkElement; } }
        protected override object Evaluate(EvaluatingExtensionBase root, object[] values)
        {
            base.TargetObject = root.TargetObject;
            TargetProperty = root.TargetProperty;
            return CoerceCommand(values[0], values.Skip(1).ToArray());
        }

        public abstract ICommand CoerceCommand(object commandish, object[] parameters);
   

    }






}
