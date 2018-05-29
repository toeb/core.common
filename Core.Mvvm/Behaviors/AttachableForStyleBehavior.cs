using System.Windows;
using System.Linq;
using System.Windows.Interactivity;

namespace Core.Common.Wpf
{
    public class StyleBehavior<TComponent, TBehavior> : Behavior<TComponent>
        where TComponent : System.Windows.DependencyObject
        where TBehavior : StyleBehavior<TComponent, TBehavior>, new()
    {
        public static DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool),
            typeof(StyleBehavior<TComponent, TBehavior>), new FrameworkPropertyMetadata(false, OnIsEnabledChanged));

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = d as UIElement;

            if (uie != null)
            {
                var behColl = Interaction.GetBehaviors(uie);
                var existingBehavior = behColl.FirstOrDefault(b => b.GetType() ==
                      typeof(TBehavior)) as TBehavior;

                if ((bool)e.NewValue == false && existingBehavior != null)
                {
                    behColl.Remove(existingBehavior);
                }

                else if ((bool)e.NewValue == true && existingBehavior == null)
                {
                    behColl.Add(new TBehavior());
                }
            }
        }
    }
}