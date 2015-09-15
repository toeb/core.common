using System.Linq;
using System;
using System.Collections.Generic;
namespace Core.Common.MVVM
{
  public interface IViewManager
  {
    /// <summary>
    /// show the view for the specified view model.  
    /// Task completes when the view is closed
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    IViewHandle ShowView(object viewModel);

    

  }

  public interface IViewProvider
  {
    IEnumerable<IFactory<IView>> FindViews(Func<IViewMetadata, bool> predicate);
  }

  public static class IViewManagerMixins 
  {


    public static IView CreateView(this IViewProvider viewManager, IViewModel viewModel)
    {
      var viewModelType = viewModel.GetType();
      var viewFactories =  viewManager.FindViews(vm =>
      {
        return vm.ViewModelType.IsAssignableFrom(viewModelType);
      });

      if (viewFactories.Count() == 0) return null;
      var viewFactory = viewFactories.First();
      var view = viewFactory.Create();
      return view;
    }
  }

}
