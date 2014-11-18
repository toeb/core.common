using Core.Merge;
using System;
using System.Threading.Tasks;

namespace Core.Values
{
  public interface ISource : IConnector
  {
    /// <summary>
    /// information about this source
    /// </summary>
    ISourceInfo SourceInfo{ get; }
    /// <summary>
    /// return true a value can be produced
    /// </summary>
    /// <returns></returns>
    bool CanProduce();
    /// <summary>
    /// call to receive a value from this source
    /// </summary>
    /// <returns></returns>
    object Produce();

    /// <summary>
    /// access to the current value of the source
    /// </summary>
    object Value { get; }

    /// <summary>
    /// call to pull a value from the source into a sink
    /// using the specified MergeStrategy
    /// </summary>
    /// <param name="sink"></param>
    /// <param name="strategy"></param>
    void Pull(ISink sink, IMergeStrategy strategy);
    

    event ValueProducedDelegate ValueProduced;
    event ValueChangeDelegate ValueChanged;
    //SourceState State { get; }

  }



}
