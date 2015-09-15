using System;
using System.ComponentModel;

namespace Core.Common.MVVM
{
  public interface IViewMetadata
  {
    [DefaultValue("")]
    string Contract { get; }
    [DefaultValue(null)]
    Type ViewModelType { get; }
    [DefaultValue(null)]
    Type ViewType { get; }
    [DefaultValue(null)]
    Type ModelType { get; }
  }
}
