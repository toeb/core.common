using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public interface ISubscribable
  {
    /// <summary>
    /// subscribes to the topic - whenever the topic changes del is called
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="del"></param>
    void Subscribe(string topic, ValueChangeDelegate del);
    /// <summary>
    /// unsubscirbes the specific topic - whenever the topic changes del will no longer be called
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="del"></param>
    void Unsubscribe(string topic, ValueChangeDelegate del);
    /// <summary>
    /// unsubscribes all
    /// </summary>
    void UnsubscribeAll();
  }
}
