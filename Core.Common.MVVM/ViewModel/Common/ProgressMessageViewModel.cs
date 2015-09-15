using Core.Common.MVVM;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System;

namespace Core.Common.MVVM.ViewModels
{

  [ViewModel(typeof(ProgressMessageViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ProgressMessageViewModel : ViewModelBase
  {
    public ProgressMessageViewModel()
    {

    }

    public string Title
    {
      get { return Get<string>(); }
      set
      {
        Set(value);
        if (SetTitle != null) SetTitle(value);
      }
    }
    public string Message
    {
      get { return Get<string>(); }
      set
      {
        Set(value);
        if (SetMessage != null) SetMessage(value);
      }
    }
    public bool IsIndeterminate
    {
      get { return Get<bool>(); }
      set
      {
        Set(value);
        if (SetIndeterminate != null && value) SetIndeterminate();
        if (SetIndeterminate != null && !value) SetProgress(Progress);
      }
    }
    public double Progress
    {
      get { return Get<double>(); }
      set
      {
        Set(value);
        if (SetProgress != null) SetProgress(value);
      }
    }
    public Action<string> SetTitle { get; set; }
    public Action<string> SetMessage { get; set; }
    public Action<double> SetProgress { get; set; }
    public Action SetIndeterminate { get; set; }

    public override void OnAfterConstruction()
    {
    }

    public override void OnDispose()
    {
    }
  }
}
