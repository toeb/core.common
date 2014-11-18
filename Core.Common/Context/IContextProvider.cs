namespace Core
{
  public interface IContextProvider
  {
    ContextDescriptor ContextDescriptor { get; }
    IContext GetContext();
  }
}