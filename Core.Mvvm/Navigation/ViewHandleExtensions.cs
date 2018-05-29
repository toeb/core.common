using Core.Common.MVVM;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Common;

namespace Core.Common.Wpf
{
    public static class ViewHandleExtensions
    {
       


        private const string ViewIdentifer = "UNIQ_VIEW_STR";
        public static void SetViewHandle(this CommandExecutionContext context, ViewHandle handle)
        {
            context.Data[ViewIdentifer] = handle;
        }
        public static ViewHandle ViewHandle(this CommandExecutionContext context)
        {
            if (context == null)
            {
                return null;
            }
            return context.Data.GetOrDefault(ViewIdentifer) as ViewHandle;
        }
        public static void OnViewClose(this CommandExecutionContext context, Action action)
        {
            var vh = context.ViewHandle();
            if (vh == null)
            {
                throw new InvalidOperationException();
            }
            TaskScheduler syncContext;
            if (SynchronizationContext.Current != null)
            {
                syncContext = TaskScheduler.FromCurrentSynchronizationContext();
            }
            else
            {
                syncContext = TaskScheduler.Current;
            }
            vh.ViewTask.ContinueWith(t =>
            {
                action();
            }, syncContext);
        }
    }

}
