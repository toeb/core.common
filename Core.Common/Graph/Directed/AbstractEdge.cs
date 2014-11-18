using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{
  public abstract class AbstractEdge<TNode, TEdge> :
    NotifyPropertyChangedBase,
    IEdge<TNode, TEdge>
    where TNode : AbstractNode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {
    protected TEdge Self
    {
      get
      {
        return (TEdge)(object)this;
      }
    }

    public TNode Head
    {
      get
      {
        return RetrieveHead();
      }
      set
      {
        if (!ChangeIfDifferent(RetrieveHead, StoreHead, value, HeadChanging, HeadChanged, HeadName))
          return;
        ConnectionChanged();

      }
    }
    protected abstract void ConnectionChanged();



    public void SetEdge(TNode tail,TNode head)
    {
      var headChanged = ChangeIfDifferent(RetrieveHead, StoreHead, head, HeadChanging, HeadChanged, HeadName);
      var tailChanged = ChangeIfDifferent(RetrieveTail, StoreTail, tail, TailChanging, TailChanged, TailName);
      if (!headChanged && !tailChanged) return;
      ConnectionChanged();
    }

    void NotifyHeadChanged()
    {
      RaisePropertyChanged(HeadName);
    }

    void NotifyTailChanged()
    {
      RaisePropertyChanged(TailName);
    }

    protected abstract void StoreHead(TNode head);
    protected abstract TNode RetrieveHead();
    protected abstract void StoreTail(TNode tail);
    protected abstract TNode RetrieveTail();


    private static readonly string HeadName = "Head";
    private static readonly string TailName = "Tail";

    protected virtual void HeadChanged(TNode oldValue, TNode newValue) { }
    protected virtual void HeadChanging(TNode oldValue, TNode newValue) { }
    protected virtual void TailChanged(TNode oldValue, TNode newValue) { }
    protected virtual void TailChanging(TNode oldValue, TNode newValue) { }

    public TNode Tail
    {
      get
      {
        return RetrieveTail();
      }
      set
      {
        if (!ChangeIfDifferent(RetrieveTail, StoreTail, value, TailChanging, TailChanged, TailName)) return;
        ConnectionChanged();
      }
    }

  }
}
