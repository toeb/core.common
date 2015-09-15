using Core.Common.MVVM;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;

namespace Core.Common.Wpf
{
  [Export(typeof(ISubViewManager))]
  public class ShowMessageViewManager : SubViewManagerBase<MessageViewModel>
  {
  
    protected override IViewHandle ShowTypedView(MessageViewModel view)
    {

      var result = MessageBox.Show(view.Message, view.Title);
      view.Result = result == MessageBoxResult.OK || result == MessageBoxResult.Yes;
      return CreateViewResult(view);
      
      

    }
  }
}
