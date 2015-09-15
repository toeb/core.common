using System.ComponentModel.Composition;

namespace Core.Common.MVVM
{

  [ViewModel(typeof(SaveFileViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class SaveFileViewModel : ViewModelBase
  {
    private SaveFileViewModel() { }

    public bool? Success { get { return Get<bool?>(); } set { Set(value); } }
    public string[] FileNames { get { return Get<string[]>(); } set { Set(value); } }

    public override void OnAfterConstruction()
    {
    }

    public override void OnDispose()
    {
    }
  }
}
