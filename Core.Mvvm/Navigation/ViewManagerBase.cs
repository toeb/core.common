using Core.Common.MVVM;
using System;
using System.Windows;
using Core.Common.Reflect;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace Core.Common.Wpf
{

    public static class ViewManager
    {







    }

    public interface IViewManager
    {
        bool ShowView(ViewHandle viewHandle);
    }

    public abstract class ViewManagerBase<T, ViewManagerType> : StyleBehavior<T, ViewManagerType>, IViewManager where T : FrameworkElement where ViewManagerType : StyleBehavior<T, ViewManagerType>, new()
    {
        protected abstract void CloseView();

        protected sealed override void OnAttached()
        {
            base.OnAttached();
            SetupViewBehavior();
        }

        public bool ShowView(ViewHandle viewHandle)
        {
            if(viewHandle==null)
            {
                throw new ArgumentNullException(nameof(viewHandle));
            }
            viewHandle.HostControl = this;
            viewHandle.DoClose = CloseView;
            Trace.TraceInformation("Showing View " + viewHandle);
            if (!DoShowView(viewHandle))
            {
                return false;
            }
            viewHandle.SetCloseCommand(AssociatedObject);
            return true;
        }

        public abstract bool DoShowView(ViewHandle handle);
        protected virtual void SetupViewBehavior() { }
        protected virtual void CleanupViewBehavior() { }

        protected sealed override void OnDetaching()
        {
            CleanupViewBehavior();
            base.OnDetaching();
        }

    }
}