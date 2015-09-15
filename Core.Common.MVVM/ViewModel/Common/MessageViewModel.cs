using System.ComponentModel.Composition;

namespace Core.Common.MVVM
{
  [ViewModel(typeof(MessageViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class MessageViewModel : ViewModelBase
  {
    public string Message { get; set; }
    public string Title { get; set; }
    public bool Result { get; set; }

    public override void OnAfterConstruction()
    {
    }

    public override void OnDispose()
    {
    }
  }
}
