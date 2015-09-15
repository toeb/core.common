
using System;
namespace Core.Common.MVVM
{
  public interface IApplicationService
  {
    void Shutdown();


    event EventHandler BeforeShutdown;
  }
}
