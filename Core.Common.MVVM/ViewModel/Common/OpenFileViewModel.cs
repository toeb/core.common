using Core.Common.MVVM;
using System.ComponentModel.Composition;

namespace Core.Common.MVVM
{
  [ViewModel(typeof(OpenFileViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class OpenFileViewModel : ViewModelBase
  {

    public bool? Success { get; set; }

    public string[] FileNames { get; set; }

    public override void OnAfterConstruction()
    {
    }

    public override void OnDispose()
    {
    }
  }
}
