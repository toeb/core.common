using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Core.Common.Injection;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Core.Common.Reflect;
using Core.Common.Wpf.Export;

namespace Core.Common.Wpf
{

    public class OwningObjectContext
    {

        public OwningObjectContext(DependencyObject owningObject)
        {
            _weak = new WeakReference<DependencyObject>(owningObject);
        }
        private WeakReference<DependencyObject> _weak;


        public DependencyObject OwningObject
        {
            get
            {
                if(_weak.TryGetTarget(out var result))
                {
                    return result;
                }
                return null;
            }
            
        }


    }


    public static class WpfHelpers
    {

        public static DependencyObject FirstChildOfType<T>(this DependencyObject element) where T : DependencyObject
        {
            if (element == null)
            {
                return null;
            }
            return element.EnumerateParents().FirstOrDefault(inner => inner.EnumerateParents().FirstOrDefault() is T);
        }


        public static IEnumerable<T> GetDescendentsOfType<T>(this DependencyObject obj) where T : DependencyObject
        {

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                {
                    yield return child as T;
                }
                foreach (var childChild in child.GetDescendentsOfType<T>())
                {
                    yield return childChild;
                }
            }

        }

        public static T FindVisualChild<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null)
                return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null)
                        break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static T GetVisualChild<T>(this DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static void AfterLoad(this DependencyObject obj, Action action)
        {
            var fe = obj as FrameworkElement;
            if (fe != null)
            {
                if (!fe.IsLoaded)
                {

                    RoutedEventHandler handler = null;

                    handler = (sender, arg2s) =>
                    {
                        fe.Loaded -= handler;
                        action();
                    };
                    fe.Loaded += handler;
                    return;
                }
            }
            action();
        }
        public static void AfterUnLoad(this DependencyObject obj, Action action)
        {
            var fe = obj as FrameworkElement;
            if (fe != null)
            {
                if (fe.IsLoaded)
                {

                    void handler(object sender, RoutedEventArgs arg2s)
                    {
                        fe.Unloaded -= handler;
                        action();
                    }

                    fe.Unloaded += handler;
                    return;
                }
            }
            
        }
        public static TreeViewItem GetTreeViewItem(this TreeView treeView, object item)
        {
            return treeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
        }
        public static void SelecItem(this TreeView treeView, object item)
        {
            var tvItem = treeView.GetTreeViewItem(item);
            tvItem.IsSelected = true;
        }

        /// <summary>
        /// Get the UIElement that is in the container at the point specified
        /// </summary>
        /// <param name="container"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static UIElement GetUIElement(ItemsControl container, Point position)
        {
            UIElement elementAtPosition = container.InputHitTest(position) as UIElement;
            //move up the UI tree until you find the actual UIElement that is the Item of the container
            if (elementAtPosition != null)
            {
                while (elementAtPosition != null)
                {
                    object testUIElement = container.ItemContainerGenerator.ItemFromContainer(elementAtPosition);
                    if (testUIElement != DependencyProperty.UnsetValue) //if found the UIElement
                    {
                        return elementAtPosition;
                    }
                    else
                    {
                        elementAtPosition = VisualTreeHelper.GetParent(elementAtPosition) as UIElement;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Determines if the relative position is above the UIElement in the coordinate
        /// </summary>
        /// <param name="i"></param>
        /// <param name="relativePosition"></param>
        /// <returns></returns>
        public static bool IsPositionAboveElement(UIElement i, Point relativePosition)
        {
            if (relativePosition != null)
                if (relativePosition.Y < ((FrameworkElement)i).ActualHeight / 2) //if above
                    return true;
            return false;
        }
        public static T FirstParentOfType<T>(this DependencyObject @object) where T : DependencyObject
        {
            return @object.EnumerateParents().OfType<T>().FirstOrDefault();
        }
        public static IEnumerable<DependencyObject> EnumerateParents(this DependencyObject @object)
        {
            if (@object == null)
            {
                yield break;
            }
            var current = VisualTreeHelper.GetParent(@object);
            while (current != null)
            {
                yield return current;
                current = VisualTreeHelper.GetParent(current);
            }
        }
        public static IEnumerable<DependencyObject> EnumerateParentsAndThis(this DependencyObject @object)
        {
            if (@object == null)
            {
                yield break;
            }
            yield return @object;

            var current = VisualTreeHelper.GetParent(@object);
            while (current != null)
            {
                yield return current;
                current = VisualTreeHelper.GetParent(current);
            }
        }

        /// <summary>   A DependencyObject extension method that gets injection service by enumerating the visual tree. </summary>
        ///
        /// <remarks>   Toeb, 2018-04-23. </remarks>
        ///
        /// <param name="TargetObject"> . </param>
        ///
        /// <returns>   The injection service. </returns>
        public static IInjectionService GetInjectionService(this DependencyObject TargetObject)
        {
            // needs to be extended to look at the views themselves
            foreach (var obj in TargetObject.EnumerateParentViewModels())
            {
                if (obj is IInjector ij)
                {
                    return ij.Injector;
                }
            }
            return null;
        }

        /// <summary>   Resolves the content for a specified dependency object. </summary>
        ///
        /// <remarks>   Toeb, 2018-04-23. </remarks>
        ///
        /// <param name="TargetObject"> . </param>
        /// <param name="content">      . </param>
        /// <param name="parameters">   (Optional) </param>
        ///
        /// <returns>   An object. </returns>
        public static object ResolveContent(this DependencyObject TargetObject, object content, object[] parameters = null)
        {
            var injectionService = TargetObject.GetInjectionService() ?? Application.Current.RootInjector();
            injectionService = injectionService.NewScope();
            injectionService.RegisterService(new OwningObjectContext(TargetObject));
            var reflectionService = injectionService?.GetService<IReflectionService>();
            if (content is string str)
            {

                if (Regex.IsMatch(str, "^[a-zA-Z_][a-zA-Z_0-9]*$"))
                {
                    // identifier
                    if (injectionService != null)
                    {
                        var types = reflectionService.FindTypeByPartialName(str).ToArray();
                        var t = types.SingleOrDefault();
                        if(t!=null)
                        {
                            return injectionService.Construct(t, parameters);
                        }

                    }
                }
                
                
                try
                {
                    var componentUri = new Uri(BaseUriHelper.GetBaseUri(TargetObject), "..");
                    if (Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out var newUri))
                    {
                        componentUri = newUri;
                    }
                    else
                    {
                        componentUri = new Uri(componentUri, str);
                    }


                    var item = Application.LoadComponent(componentUri);



                    return item;
                }
                catch (Exception e)
                {
                    Trace.TraceInformation($"failed to load uri content {str}");
                    return null;
                }
            }
            if (content is Type type)
            {
                if (injectionService == null)
                {
                    return null;
                }
                return injectionService.Construct(type, parameters);

            }
            return null;
        }

        public static IEnumerable<object> EnumerateParentViewModels(this DependencyObject @object)
        {
            return @object.EnumerateParents().OfType<FrameworkElement>().Select(fe => fe.DataContext).Where(e => e != null).Distinct();
        }


        public static int GetItemIndexFromPoint(this ItemsControl source, Point point)
        {

            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                    if (element == source)
                    {
                        return -1;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return source.ItemContainerGenerator.IndexFromContainer(element);
                }
            }
            return -1;

        }
        public static object GetItemFromPoint(this ItemsControl source, Point point)
        {

            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                    if (element == source)
                    {
                        return null;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }
    }

}
