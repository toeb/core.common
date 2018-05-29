using Core.Common.Wpf;
using System;
using System.Windows.Controls;

namespace Core.Common.Wpf
{
    public class ModalViewManager: ViewManagerBase<ContentControl, ModalViewManager>
    {

        private ViewHandle _current;
        public override bool DoShowView(ViewHandle handle)
        {
            _current = handle;
            AssociatedObject.Content = handle.Content;
            return true;
        }

        protected override void CloseView()
        {
            AssociatedObject.Content = null;
            var current = _current;
            _current = null;
            if (current!=null)
            {
                current.NotifyComplete();
            }
        }

        
    }
}