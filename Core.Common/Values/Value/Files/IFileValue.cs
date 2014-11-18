
namespace Core.Values
{
  public interface IFileValue
  {
    string Path { get; set; }
  }

  public interface IFileValue<T> : IFileValue, IValue<T>
  {

  }
}
