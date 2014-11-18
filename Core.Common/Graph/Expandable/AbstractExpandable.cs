
namespace Core.Graph
{
  public abstract class AbstractExpandable : NotifyPropertyChangedBase, IExpandable
  {
    private object stateLock = new object();
    protected abstract bool DoExpand();
    protected abstract bool DoCollapse();
    private const string IsExpandedName = "IsExpanded";
    private const string IsCollapsedName = "IsCollapsed";
    private const string IsTransitioningName = "IsTransitioning";

    private const string StateName = "State";

    private ExpandableState state = ExpandableState.Collapsed;

    public ExpandableState State
    {
      get { return state; }
      set
      {
        ChangeIfDifferent(ref state, value, StateName);
        RaisePropertyChanged(IsTransitioningName);
        RaisePropertyChanged(IsExpandedName);
        RaisePropertyChanged(IsCollapsedName);
      }
    }


    public ExpandableState Expand()
    {
      lock (stateLock)
      {
        if (State != ExpandableState.Collapsed) return State;
        State = ExpandableState.Expanding;
      }
      var couldExpand = DoExpand();
      lock (stateLock)
      {
        State = couldExpand ? ExpandableState.Expanded : ExpandableState.Collapsed;
        return State;
      }
    }

    public ExpandableState Collapse()
    {
      lock (stateLock)
      {
        if (State != ExpandableState.Expanded) return State;
        State = ExpandableState.Collapsing;
      }
      var couldCollapse = DoCollapse();

      lock (stateLock)
      {
        State = couldCollapse ? ExpandableState.Collapsed : ExpandableState.Expanded;
        return State;
      }
    }


    public bool IsTransitioning
    {
      get { return State == ExpandableState.Collapsing || State == ExpandableState.Expanding; }
    }

    public bool IsExpanded
    {
      get { return State == ExpandableState.Expanded; }
    }


    public bool IsCollapsed
    {
      get { return State == ExpandableState.Expanded; }
    }
  }
}
