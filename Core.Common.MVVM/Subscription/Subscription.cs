using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;

namespace Core.Common.MVVM
{
    public class Subscription : IDisposable
    {
        private SubscriptionProvider provider;
        private string topic;
        private Action<object> action;
        public Subscription(SubscriptionProvider subscriptionProvider, string propertyName, Action<object> action)
        {
            this.provider = subscriptionProvider;
            this.topic = propertyName;
            this.action = action;
        }
        
        public string Topic { get { return topic; } }
        public Action<object> Action { get { return action; } }


        public void Dispose()
        {
            provider.Unsubscribe(this);
        }

    }
}
