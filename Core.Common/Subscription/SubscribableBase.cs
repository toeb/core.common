using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  /// <summary>
  /// Base class for topic subsription
  /// </summary>
  public class SubscribableBase : ISubscribable
  {
    /// <summary>
    /// 
    /// </summary>
    private IDictionary<string, ValueChangeDelegate> subscribers = null;

    /// <summary>
    /// subclasses call this to let all subscribers know that a topic has changed
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected void NotifyTopicChanged(string topic, object oldValue, object newValue)
    {
      var del = GetSubscriber(topic);
      if (del == null) return;
      OnNotifyTopicChanged(topic, oldValue, newValue);
      var args = new ValueChangeEventArgs(ChangeState.Changed, oldValue, newValue);
      del(this, args);
    }
    /// <summary>
    /// subclasses call this to let all subscribers know that a topic is changing
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected void NotifyTopicChanging(string topic, object oldValue, object newValue)
    {
      var del = GetSubscriber(topic);
      if (del == null) return;
      OnNotifyTopicChanging(topic, oldValue, newValue);
      var args = new ValueChangeEventArgs(ChangeState.Changing, oldValue, newValue);
      del(this, args);
    }
    /// <summary>
    /// extension point which is called whenever a topic changed
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnNotifyTopicChanged(string topic, object oldValue, object newValue) { }
    /// <summary>
    /// extension point which is called whenever a topic is changing
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnNotifyTopicChanging(string topic, object oldValue, object newValue) { }

    /// <summary>
    /// returns the subscriber to topic. null if non is found
    /// </summary>
    /// <param name="topic"></param>
    /// <returns></returns>
    protected ValueChangeDelegate GetSubscriber(string topic)
    {
      if (!Subscribers.ContainsKey(topic)) return null;
      return Subscribers[topic];
    }
    /// <summary>
    /// read/write access to all subscribers
    /// </summary>
    protected IDictionary<string, ValueChangeDelegate> Subscribers
    {
      get
      {
        if (subscribers == null) subscribers = new Dictionary<string, ValueChangeDelegate>();
        return subscribers;

      }
    }

    public void Subscribe(string topic, ValueChangeDelegate del)
    {
      if (!Subscribers.ContainsKey(topic))
      {
        Subscribers[topic] = null;
      }
      else
      {
        Subscribers[topic] += del;
      }
    }

    public void Unsubscribe(string topic, ValueChangeDelegate del)
    {
      if (!Subscribers.ContainsKey(topic)) return;
      Subscribers[topic] -= del;
    }


    public void UnsubscribeAll()
    {
      Subscribers.Clear();
    }
  }
}
