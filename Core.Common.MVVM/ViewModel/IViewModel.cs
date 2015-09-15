
namespace Core.Common.MVVM
{
  public interface IViewModel
  {
    object Model { get; set; }
    IViewModel RequireChild(object value, System.Type viewModelType, string viewModelContract);
    IViewModel Parent { get; set; }
  }
}
