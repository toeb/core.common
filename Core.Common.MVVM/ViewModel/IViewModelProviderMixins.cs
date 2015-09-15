using System.Linq;

using System;

namespace Core.Common.MVVM
{
  public static class IViewModelProviderMixins
  {
    public static object RequireViewModel(this IViewModelProvider vmp, Func<IViewModelMetadata, bool> predicate)
    {
      var vms = vmp.FindViewModels(predicate);
      return vms.Single().Create();
    }

    public static object RequireViewModel(this IViewModelProvider vmp, Type viewModelType)
    {

      return vmp.RequireViewModel(md => md.ViewModelType == viewModelType);
    }
    public static TModel Require<TModel>(this IViewModelProvider vmp) where TModel : class
    {
      return vmp.RequireViewModel(typeof(TModel)) as TModel;
    }

    public static TModel Require<TModel>(this IViewModelProvider vmp, Action<TModel> initializer) where TModel : class
    {
      var vm = vmp.RequireViewModel(typeof(TModel)) as TModel;
      if (vm == null) return null;
      initializer(vm);
      return vm;
    }
  }
}
