using Core.Merge;
using System;
using System.Threading.Tasks;

namespace Core.Values
{
  public interface ISink : IConnector
  {
    /// <summary>
    /// return true if the specified value can be cnsumed
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool CanConsume(object value);
    /// <summary>
    /// returns the info object for this sinkl
    /// </summary>
    ISinkInfo SinkInfo { get; }
    /// <summary>
    /// causes the sink to consume the specified value
    /// </summary>
    /// <param name="value"></param>
    void Consume(object value);
    /// <summary>
    /// convenience accessor to consume a value
    /// </summary>
    object Value { set; }

    /// <summary>
    /// method for merge a new value into the sink
    /// </summary>
    /// <param name="source"></param>
    /// <param name="strategy"></param>
    void Push(ISource source, IMergeStrategy strategy);

    /// <summary>
    /// fired when sink consumes  a value
    /// </summary>
    event ValueConsumedDelegate ValueConsumed;
  }

}
