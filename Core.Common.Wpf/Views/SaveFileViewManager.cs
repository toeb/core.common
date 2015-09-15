using Core.Common.MVVM;
using Microsoft.Win32;
using System;
using System.ComponentModel.Composition;

namespace Core.Common.Wpf
{
  [Export(typeof(ISubViewManager))]
  public class SaveFileViewManager : SubViewManagerBase<SaveFileViewModel>
  {

    protected override IViewHandle ShowTypedView(SaveFileViewModel view)
    {

      var sfd = new SaveFileDialog();
      var success = sfd.ShowDialog();


      view.Success = success;
      view.FileNames = sfd.FileNames;
      return CreateViewResult(view);
    }
  }
}
