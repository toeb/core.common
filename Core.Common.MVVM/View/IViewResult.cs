using System.Threading.Tasks;

namespace Core.Common.MVVM
{
  public interface IViewHandle
  {
    object ViewModel { get; }
    void Close();
    void Activate();
  }
}
