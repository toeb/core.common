using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace Core.Common.Wpf.Export
{
    public class NavigationTarget : MarkupExtension
    {


        public static DataTemplate GetTargetTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(TargetTemplateProperty);
        }

        public static void SetTargetTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(TargetTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for TargetTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetTemplateProperty =
            DependencyProperty.RegisterAttached("TargetTemplate", typeof(DataTemplate), typeof(NavigationTarget), new PropertyMetadata(null));




        public static object GetStartup(DependencyObject obj)
        {
            return (object)obj.GetValue(StartupProperty);
        }

        public static void SetStartup(DependencyObject obj, object value)
        {
            obj.SetValue(StartupProperty, value);
        }

        // Using a DependencyProperty as the backing store for Startup.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartupProperty =
            DependencyProperty.RegisterAttached("Startup", typeof(object), typeof(NavigationTarget), new PropertyMetadata(null));




        public static object GetDefault(DependencyObject obj)
        {
            return (object)obj.GetValue(DefaultProperty);
        }

        public static void SetDefault(DependencyObject obj, object value)
        {
            obj.SetValue(DefaultProperty, value);
        }

        // Using a DependencyProperty as the backing store for Default.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.RegisterAttached(
                "Default", 
                typeof(object), 
                typeof(NavigationTarget), 
                new FrameworkPropertyMetadata(typeof(Window), FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DefaultChanged)));

        private static void DefaultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d;


        }

        public static string GetUri(DependencyObject obj)
        {
            return (string)obj.GetValue(UriProperty);
        }

        public static void SetUri(DependencyObject obj, string value)
        {
            obj.SetValue(UriProperty, value);
        }

        // Using a DependencyProperty as the backing store for Uri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.RegisterAttached("Uri", typeof(string), typeof(NavigationTarget), new PropertyMetadata(new PropertyChangedCallback(UriChanged)));


        private static Dictionary<string, DependencyObject> _viewPorts = new Dictionary<string, DependencyObject>();
        public static DependencyObject GetViewPort(string uri)
        {
            if(uri==null)
            {
                return null;
            }
            return _viewPorts.GetOrDefault(uri);
        }

        private static void UriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _viewPorts[e.NewValue as string] = d;
        }

        public NavigationTarget(
            string uri = null)
        {
            this.Uri = uri;
        }

        public string Uri
        {
            get; private set;
        }
        public object ContentControl
        {
            get; set;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }






}
