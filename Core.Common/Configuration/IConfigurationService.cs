using Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{

  public interface IMonitoringContext : IDisposable
  {
    bool IsMonitoring { get; }
    Task StartMonitoringAsync();
    Task StopMonitoringAsync();
  }

  public interface IConfigurationContext : IMonitoringContext
  {

  }

  /// <summary>
  /// service for configuring object/properties
  /// </summary>
  public interface IConfigurationService
  {
    Task LoadConfigurationAsync(object value);
    Task StoreConfigurationAsync(object value);
  }
}
