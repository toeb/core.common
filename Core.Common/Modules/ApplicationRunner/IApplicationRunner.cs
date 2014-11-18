using System;

namespace Core.Modules.Applications.Runner
{
  public interface IApplicationRunner
  {
    bool IsStopping { get; }
    void Start();
    void Stop();
    event Action Starting;
    event Action Stopping;
  }
}
