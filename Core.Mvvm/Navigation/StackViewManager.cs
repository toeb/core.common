using Core.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using Core.Common.Reflect;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Windows.Controls;

namespace Core.Common.Wpf
{

    public class StackViewManager: ViewManagerBase<ContentControl, StackViewManager>
    {



        public static ICommand GetGoBackTo(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(GoBackToProperty);
        }

        public static void SetGoBackTo(DependencyObject obj, ICommand value)
        {
            obj.SetValue(GoBackToProperty, value);
        }

        // Using a DependencyProperty as the backing store for GoBackTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoBackToProperty =
            DependencyProperty.RegisterAttached("GoBackTo", typeof(ICommand), typeof(StackViewManager), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));






        public static ICommand GetGoBack(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(GoBackProperty);
        }

        public static void SetGoBack(DependencyObject obj, ICommand value)
        {
            obj.SetValue(GoBackProperty, value);
        }

        // Using a DependencyProperty as the backing store for GoBack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoBackProperty =
            DependencyProperty.RegisterAttached("GoBack", typeof(ICommand), typeof(StackViewManager), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));



        public static IEnumerable<StackViewItem> GetViewStack(DependencyObject obj)
        {
            return (IEnumerable<StackViewItem>)obj.GetValue(ViewStackProperty);
        }

        public static void SetViewStack(DependencyObject obj, IEnumerable<StackViewItem> value)
        {
            obj.SetValue(ViewStackProperty, value);
        }
        // Using a DependencyProperty as the backing store for ViewStack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewStackProperty =
            DependencyProperty.RegisterAttached("ViewStack", typeof(IEnumerable<StackViewItem>), typeof(StackViewManager), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));




        public void Back()
        {
            if (ViewStack.Count() > 1)
            {

                Remove(ViewStack.Last());
                var item = ViewStack.Last();
                AssociatedObject.Content = item.Content;

            }
        }

        public void Remove(StackViewItem item)
        {
            if (item == null)
            {
                return;
            }

            ModifiableViewStack.Remove(item);
            item.ViewHandle.NotifyComplete();
        }


        public void BackTo(object item)
        {
            if (item == null)
            {
                return;
            }
            if (!ViewStack.Any(i => i.Content == item))
            {
                return;
            }

            while (ViewStack.Last().Content != item)
            {
                Remove(ViewStack.Last());
            }
            AssociatedObject.Content = item;
        }

        protected override void SetupViewBehavior()
        {
            SetGoBack(AssociatedObject, Command.CreateCommand(Back));
            SetGoBackTo(AssociatedObject, Command.CreateCommand<StackViewItem>(BackTo));
        }

        protected override void CloseView()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                BackTo(ViewStack.Last());
                Back();
            }));

        }

        public void CloseSpecificView(object o)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {

                BackTo(o);
                Back();

            }));

        }
        public override bool DoShowView(ViewHandle handle)
        {

            ModifiableViewStack.Add(new StackViewItem() { ViewHandle = handle, Content = handle.Content, Header = DateTime.Now.ToString() });


            handle.DoClose = () => CloseSpecificView(handle.Content);
            AssociatedObject.Content = handle.Content;
            return true;
        }



        private ObservableCollection<StackViewItem> ModifiableViewStack
        {
            get
            {
                var col = GetViewStack(AssociatedObject) as ObservableCollection<StackViewItem>;
                if (col == null)
                {
                    col = new ObservableCollection<StackViewItem>();
                }
                SetViewStack(AssociatedObject, col);
                return col;
            }
        }
        public IEnumerable<StackViewItem> ViewStack
        {
            get
            {
                return ModifiableViewStack;
            }
        }






    }
}