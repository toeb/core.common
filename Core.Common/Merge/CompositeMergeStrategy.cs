using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Core.Merge
{
  public class CompositeMergeStrategy : IMergeStrategy
  {

    public IEnumerable<IMergeStrategy> Strategies { get; set; }
    [ImportingConstructor]
    public CompositeMergeStrategy([Import(AllowDefault=true)]  IEnumerable<IMergeStrategy> strategies)
    {
      this.Strategies = strategies;
    }

    public CompositeMergeStrategy() { }

    public  bool CanMerge(object a, object b)
    {
      return Strategies.Any(strategy => strategy.CanMerge(a, b));
    }

    public  object Merge(object a, object b)
    {
      var strategy = Strategies.First(s => s.CanMerge(a, b));
      return strategy.Merge(a, b);
    }
  }
}
