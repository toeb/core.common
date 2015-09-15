using System.Threading.Tasks;

namespace Core.Common.MVVM
{
  public class ViewHandle : IViewHandle
  {
    public ViewHandle(object viewModel)
    {
      ViewModel = viewModel;

      var vmb = ViewModel as ViewModelBase;
      vmb.ViewHandle = this;
    }
    public object ViewModel
    {
      get;
      private set;
    }



    public virtual void Close()
    {
    }

    public virtual void Activate()
    {
    }
  }
}
