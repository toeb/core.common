using System;
using System.Linq;
using System.Management;

namespace Core.Common.Diagnostics
{
  [Serializable]
  public class OsInfo
  {
    private static OsInfo instance;
    public static OsInfo Instance { get { return instance ?? (instance = new OsInfo()); } }
    public OsInfo()
    {

      if (instance != null)
      {
        Name = instance.Name;
        Version = instance.Version;
        MaxProcessCount= instance.MaxProcessCount;
        MaxProcessRAM= instance.MaxProcessRAM;
        Architecture = instance.Architecture;
        SerialNumber = instance.SerialNumber;
        Build = instance.Build;
        return;
      }
      
      var wmi =
       new ManagementObjectSearcher("select * from Win32_OperatingSystem")
       .Get()
       .Cast<ManagementObject>()
       .First();
  
      this.Name = ((string)wmi["Caption"]).Trim();
      this.Version = (string)wmi["Version"];
      this.MaxProcessCount = (uint)wmi["MaxNumberOfProcesses"];
      this.MaxProcessRAM = (ulong)wmi["MaxProcessMemorySize"];
      this.Architecture = (string)wmi["OSArchitecture"];
      this.SerialNumber = (string)wmi["SerialNumber"];
      this.Build = uint.Parse(((string)wmi["BuildNumber"]));
      instance = this;
    }

    public override string ToString()
    {
      return Name+ " - "+Architecture +" - "+Version;
    }

    public string Name { get; private set; }
  
    public string Version { get; private set; }
  
    public uint MaxProcessCount { get; private set; }
  
    public ulong MaxProcessRAM { get; private set; }
  
    public string Architecture { get; private set; }
  
    public string SerialNumber { get; private set; }
  
    public uint Build { get; private set; }
  }
}
