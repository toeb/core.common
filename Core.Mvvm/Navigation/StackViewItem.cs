using System.Windows;

namespace Core.Common.Wpf
{
    public class StackViewItem : DependencyObject
    {

        public ViewHandle ViewHandle { get; set; }


        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public object Content { get; internal set; }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(StackViewItem), new PropertyMetadata(""));


    }
}