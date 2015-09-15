using Core.Common.MVVM;
using Microsoft.Win32;
using System.ComponentModel.Composition;

namespace Core.Common.Wpf
{
  [Export(typeof(ISubViewManager))]
  public class OpenFileViewManager : SubViewManagerBase<OpenFileViewModel>
  {
  
    protected override IViewHandle ShowTypedView(OpenFileViewModel view)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      var success = ofd.ShowDialog();

      view.Success = success;
      view.FileNames = ofd.FileNames;
      return CreateViewResult(view);
      
    }
  }
}
