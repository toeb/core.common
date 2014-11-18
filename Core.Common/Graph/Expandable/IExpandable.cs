

namespace Core.Graph
{


  public interface IExpandable
  {
    /// <summary>
    /// 
    /// </summary>
    ExpandableState State { get; }
    /// <summary>
    /// returns true when node is changing state from collapsed to expanded or vice versa
    /// </summary>
    bool IsTransitioning { get; }
    /// <summary>
    /// indicates wether the node is read or dormant
    /// </summary>
    bool IsExpanded { get; }
    /// <summary>
    /// 
    /// </summary>
    bool IsCollapsed { get; }
    /// <summary>
    /// causes the node to load all necessary connection
    /// returns true if expansion was sucessfully started
    /// returns fals if node is either expanding or fails to expand
    /// </summary>
    ExpandableState Expand();
    /// <summary>
    ///  causes node to unload
    /// </summary>
    ExpandableState Collapse();
  }
}
