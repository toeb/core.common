using System;
using System.ComponentModel;

namespace Core.Common.MVVM
{
  public interface IViewModelMetadata
  {
    [DefaultValue(null)]
    Type ModelType { get; }
    [DefaultValue(null)]
    string Contract { get; }
    [DefaultValue(null)]
    Type ViewModelType { get; }

  }

}
