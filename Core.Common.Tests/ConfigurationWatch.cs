using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Test
{
 public  class ConfigurationWatch : IDisposable
  {
    private string key;
    private Type type;
    private object value;
    private ConfigurationService configurationService;

    FileSystemWatcher watcher;
    public ConfigurationWatch(string key, Type type, object value, ConfigurationService configurationService)
    {
      this.key = key;
      this.type = type;
      this.value = value;
      this.configurationService = configurationService;
      configurationService.ConfigureAsync(key, type, value).Await();
      var path = configurationService.FileSystem.ToAbsolutePath(configurationService.GetPath(key, type));
      var dir = Path.GetDirectoryName(path);
      var file = Path.GetFileName(path);
      watcher = new FileSystemWatcher(dir,file);
      watcher.Changed += watcher_Changed;
      watcher.EnableRaisingEvents = true;
      
    }

    void watcher_Changed(object sender, FileSystemEventArgs e)
    {
      configurationService.ReadConfigAsync(key, type, value).ContinueWith(t => {
        if (ConfigurationChanged != null) ConfigurationChanged(this);
      });
    }

    public event Action<ConfigurationWatch> ConfigurationChanged;
    public void Dispose()
    {
      watcher.Dispose();
    }
  }
}
