using System.Windows;

namespace Core.Common.Wpf.Export
{
    public abstract class FrameworkMarkupExtensionBase : MarkupExtensionBase
    {
        public new FrameworkElement TargetObject
        {
            get
            {
                return base.TargetObject as FrameworkElement;
            }
        }



    }






}
