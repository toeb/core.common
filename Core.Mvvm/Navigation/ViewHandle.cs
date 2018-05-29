using Core.Common.MVVM;
using System;
using System.Windows;
using Core.Common.Reflect;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace Core.Common.Wpf
{

    [DebuggerDisplay("{DebugString}")]
    public class ViewHandle
    {


        public ViewHandle()
        {

        }

        public string DebugString
        {
            get
            {
                return ToString();
            }
        }


        public override string ToString()
        {

            return string.Format("", Contract, Content, HostControl, Source);
        }


        public static ICommand GetCloseCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CloseCommandProperty);
        }

        private static void SetCloseCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CloseCommandProperty, value);
        }
        public void SetCloseCommand(DependencyObject obj)
        {
            SetCloseCommand(obj, Command.CreateCommand(Close));
        }
        // Using a DependencyProperty as the backing store for CloseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached("CloseCommand", typeof(ICommand), typeof(ViewHandle), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));




        public object Contract { get; set; }
        public object Content { get; set; }
        public object HostControl { get; set; }


        public void Close()
        {
            if (DoClose != null)
            {
                DoClose();
            }
        }



        private Action _doClose;
        public Action DoClose
        {
            get
            {
                return _doClose;
            }
            set
            {
                _doClose = value;
            }
        }

        internal void NotifyComplete()
        {

            ViewTaskSource.SetResult(this);
        }
        internal void NotifyError(Exception exc)
        {
            ViewTaskSource.SetException(exc);
        }
        private TaskCompletionSource<ViewHandle> ViewTaskSource
        {
            get
            {
                if (_viewTaskSource == null)
                {
                    _viewTaskSource = new TaskCompletionSource<ViewHandle>();
                }
                return _viewTaskSource;
            }
        }
        private TaskCompletionSource<ViewHandle> _viewTaskSource;
        public Task<ViewHandle> ViewTask
        {
            get
            {

                return ViewTaskSource.Task;
            }
        }

        public DataTemplate Template { get; internal set; }
        public object Source { get; internal set; }
    }
}