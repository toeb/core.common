
namespace Core.Graph
{
  
  public class DelegateExpandable : AbstractExpandable
  {
    public DelegateExpandable(ExpandDelegate expand,CollapseDelegate collapse)
    {
      this.ExpandDelegate = expand;
      this.CollapseDelegate = collapse;
    }

    protected override bool DoExpand()
    {
      return ExpandDelegate();
    }

    protected override bool DoCollapse()
    {
      return CollapseDelegate();
    }

    private ExpandDelegate ExpandDelegate { get; set; }

    private CollapseDelegate CollapseDelegate { get; set; }
  }
}
