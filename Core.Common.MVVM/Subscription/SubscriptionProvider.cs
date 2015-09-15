using Core.Common.Reflect;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Core.Common.Collections;
using System.Linq;
using System.Diagnostics;
using System;
using System.ComponentModel;

namespace Core.Common.MVVM
{
    [Export]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public class SubscriptionProvider : IDisposable
    {
        private ICollection<Subscription> subscribers = new List<Subscription>();
        private BasePropertyObject context;

        public object Context
        {
            get { return context; }
            set
            {
                if (context != null && context != value)
                {
                    context.PropertyChanged -= OnPropertyChanged;
                }

                context = value as BasePropertyObject;
                if (context != null)
                {
                    context.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            var currentSubscribers = subscribers.Where(s => s.Topic == e.PropertyName);
            var value = context.Get(e.PropertyName, typeof(object));
            foreach (var subscriber in currentSubscribers)
            {
                subscriber.Action(value);
            }
        }

        public Subscription Subscribe(string propertyName, Action<object> action)
        {
            var subscription = new Subscription(this, propertyName, action);
            subscribers.Add(subscription);
            return subscription;
        }


        public void Unsubscribe(Subscription subscription)
        {
            subscribers.Remove(subscription);
        }

        public void Dispose()
        {
            Context = null;
        }
    }
}
