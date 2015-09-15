using Core.Common.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Core.Common.Applications
{
  public class ViewModelMetadata : IViewModelMetadata
  {
    public Type ModelType
    {
      get;
      set;
    }
  
    public string Contract
    {
      get;
      set;
    }
  
    public Type ViewModelType
    {
      get;
      set;
    }
  }
}
