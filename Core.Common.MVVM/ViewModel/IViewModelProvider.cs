using System;
using System.Collections.Generic;

namespace Core.Common.MVVM
{
  public interface IViewModelProvider
  {
    IEnumerable<IFactory<object>> FindViewModels(Func<IViewModelMetadata,bool> predicate);

  }


  
}
