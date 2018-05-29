using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Interactivity;
using System.Linq;
using Core.Common.MVVM;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Core.Common.Wpf
{


    public class MessageBoxViewModel
    {
        public string Message { get; set; }
        public MessageBoxResult Result { get; internal set; }
    }

    public static class ViewHandlers
    {

        public static IEnumerable<ViewHandleDelegate> List
        {
            get
            {
                yield return (ViewHandlers.OpenFileHandler);
                yield return (ViewHandlers.MessageBoxHandler);
                yield return (ViewHandlers.SaveFileHandler);
                yield return (ViewHandlers.BehaviorHandler);
                yield return (ViewHandlers.DialogBox);
                yield return (ViewHandlers.WindowViewControl);
                yield return (ViewHandlers.ContentControlViewHandle);

            }
        }
        public static bool TryHandle(ViewHandle handle)
        {
            bool found = false;
            foreach (var vhf in List)
            {
                if (found = vhf(handle)) break;
            }
            return found;
        }

     


        public static void SetWindow(this ViewHandle vh, Window window)
        {
            vh.HostControl = window;
            EventHandler closeHandler = null;
            closeHandler = (sender, args) =>
            {
                window.Closed -= closeHandler;
                vh.NotifyComplete();
            };
            window.Closed += closeHandler;
            vh.DoClose = window.Close;
            vh.SetCloseCommand(window);

            if (vh.Template != null)
            {
                var cc = new ContentPresenter();
                cc.Content = vh.Content;
                cc.ContentTemplate = vh.Template;
                window.Content = cc;
            }
            else
            {
                window.Content = vh.Content;

            }

        }

        internal static bool SaveFileHandler(ViewHandle viewHandle)
        {

            var vm = viewHandle.Content as OpenFileViewModel;
            if (vm == null)
            {
                return false;
            }
            if (vm.OpenDirectory)
            {

                var initialFolder = Environment.CurrentDirectory;
                if (File.Exists("lastFolder.txt"))
                {
                    initialFolder = File.ReadAllText("lastFolder.txt");
                }



                FolderBrowserDialog fbd = new FolderBrowserDialog();
                viewHandle.HostControl = fbd;
                fbd.SelectedPath = initialFolder;
                var success = fbd.ShowDialog();
                vm.FileNames = new[] { fbd.SelectedPath };
                vm.Success = success == DialogResult.OK || success == DialogResult.Yes;
                if (vm.Success.Value)
                {
                    initialFolder = fbd.SelectedPath;
                    File.WriteAllText("lastFolder.txt", initialFolder);
                }

                viewHandle.NotifyComplete();

            }
            else
            {
                var ofd = new Microsoft.Win32.OpenFileDialog();
                viewHandle.HostControl = ofd;
                ofd.Multiselect = vm.Multiselect;
                var filter = string.Join("|", vm.Filter.GroupBy(f => f.Name).Select(g => g.Key + " (" + string.Join(";", g.Select(i => "*." + i.Extension.ToLower())) + ")|" + string.Join(";", g.Select(i => "*." + i.Extension.ToLower()))));
                ofd.Filter = filter;
                var success = ofd.ShowDialog();

                vm.Success = success;
                vm.FileNames = ofd.FileNames;
                viewHandle.NotifyComplete();

            }

            return true;
        }

        internal static bool MessageBoxHandler(ViewHandle viewHandle)
        {
            var msgBx = viewHandle.Content as MessageBoxViewModel;
            if (msgBx == null)
            {
                return false;
            }
            msgBx.Result = System.Windows.MessageBox.Show(msgBx.Message);
            viewHandle.NotifyComplete();
            return true;
        }

        internal static bool OpenFileHandler(ViewHandle viewHandle)
        {
            var vm = viewHandle.Content as SaveFileViewModel;
            if (vm == null)
            {
                return false;
            }
            var sfd = new Microsoft.Win32.SaveFileDialog();
            viewHandle.HostControl = sfd;
            if (vm.FileNames != null)
            {
                sfd.FileName = vm.FileNames.FirstOrDefault();

            }
            var success = sfd.ShowDialog();
            vm.FileNames = sfd.FileNames;
            vm.Success = success;
            viewHandle.NotifyComplete();
            return true;
        }

        public static bool BehaviorHandler(ViewHandle ctx)
        {
            var item = ctx.Contract as DependencyObject;
            if (item == null)
            {
                return false;
            }

            var behaviors = Interaction.GetBehaviors(item);
            if (behaviors == null)
            {
                return false;
            }
            var viewManager = behaviors.OfType<IViewManager>().FirstOrDefault();
            if (viewManager == null)
            {
                return false;
            }


            if (!viewManager.ShowView(ctx))
            {
                return false;
            }


            return true;
        }


        public static bool DialogBox(ViewHandle ctx)
        {
            if (ctx.Contract as string == "dialog")
            {
                var window = new Window();
                window.Name = "DialogWindow";
                ctx.SetWindow(window);
                window.ShowDialog();
                return true;
            }
            return false;
        }
        public static bool WindowViewControl(ViewHandle viewHandle)
        {
            var frame = viewHandle.Contract;
            var type = frame as Type;
            if (type == null)
            {
                return false;
            }

            if (typeof(Window).IsAssignableFrom(type))
            {
                var window = System.Activator.CreateInstance(type) as Window;
                viewHandle.SetWindow(window);
                window.Show();
                return true;
            }

            return false;
        }




        public static bool ContentControlViewHandle(ViewHandle viewHandle)
        {
            var value = viewHandle.Content;
            var contentControl = viewHandle.Contract as ContentControl;
            if (contentControl == null)
            {
                return false;
            }
            viewHandle.HostControl = contentControl;
            viewHandle.DoClose = () =>
            {
                contentControl.Dispatcher.BeginInvoke(new Action(() => contentControl.Content = null));
            };
            viewHandle.SetCloseCommand(contentControl);

            contentControl.Dispatcher.BeginInvoke(new Action(() =>
            {

                EventHandler handler = null;

                handler = (sender, args) =>
                {
                    DependencyPropertyDescriptor
                    .FromProperty(ContentControl.ContentProperty, typeof(ContentControl))
                    .RemoveValueChanged(contentControl, handler);

                    viewHandle.NotifyComplete();

                };
                contentControl.Content = value;
                DependencyPropertyDescriptor
                    .FromProperty(ContentControl.ContentProperty, typeof(ContentControl))
                    .AddValueChanged(contentControl, handler);
            }));

            return true;

        }


    }
}