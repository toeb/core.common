using System.Threading.Tasks;

namespace Core.Common.MVVM
{
  public abstract class SubViewManagerBase<T> : ISubViewManager
  {

    
    protected IViewHandle CreateViewResult(object viewModel)
    {
      return new ViewHandle(viewModel);
    }
    public IViewHandle ShowView(object viewmodel)
    {

      if (!(viewmodel is T)) return null;
      var viewResult = ShowTypedView((T)viewmodel);
      return viewResult;
    }
    protected abstract IViewHandle ShowTypedView(T view);
  }
}
