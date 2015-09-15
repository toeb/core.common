using System;
using System.Linq;
using System.Management;

namespace Core.Common.Diagnostics
{
  [Serializable]
  public class CpuInfo
  {
    private static CpuInfo instance;
    public static CpuInfo Instance { get { return instance ?? (instance = new CpuInfo()); } }


    public override string ToString()
    {
      return "{Name} @{SpeedMHz}MHz".FormatWith(this);
    }

    public CpuInfo()
    {
      if (CpuInfo.instance != null)
      {
        ID = instance.ID;
        Socket = instance.Socket;
        Name = instance.Name;
        Description = instance.Description;
        AddressWidth = instance.AddressWidth;
        DataWidth = instance.DataWidth;
        Architecture = instance.Architecture;
        SpeedMHz = instance.SpeedMHz;
        BusSpeedMHz = instance.BusSpeedMHz;
        L2Cache = instance.L2Cache;
        L3Cache = instance.L3Cache;
        Cores = instance.Cores;
        Threads = instance.Threads;
        return;
      }

      // http://stackoverflow.com/questions/6944779/determine-operating-system-and-processor-type-in-c-sharp
      var cpu =
  
        new ManagementObjectSearcher("select * from Win32_Processor")
        .Get()
        .Cast<ManagementObject>()
        .First();
  
      this.ID = (string)cpu["ProcessorId"];
      this.Socket = (string)cpu["SocketDesignation"];
      this.Name = (string)cpu["Name"];
      this.Description = (string)cpu["Caption"];
      this.AddressWidth = (ushort)cpu["AddressWidth"];
      this.DataWidth = (ushort)cpu["DataWidth"];
      this.Architecture = (ushort)cpu["Architecture"];
      this.SpeedMHz = (uint)cpu["MaxClockSpeed"];
      this.BusSpeedMHz = (uint)cpu["ExtClock"];
      this.L2Cache = (uint)cpu["L2CacheSize"] * (ulong)1024;
      this.L3Cache = (uint)cpu["L3CacheSize"] * (ulong)1024;
      this.Cores = (uint)cpu["NumberOfCores"];
      this.Threads = (uint)cpu["NumberOfLogicalProcessors"];
  
      Name =
         Name
         .Replace("(TM)", "™")
         .Replace("(tm)", "™")
         .Replace("(R)", "®")
         .Replace("(r)", "®")
         .Replace("(C)", "©")
         .Replace("(c)", "©")
         .Replace("    ", " ")
         .Replace("  ", " ");
      CpuInfo.instance = this;
    }
  
    public string ID { get; set; }
  
    public string Socket { get; set; }
  
    public string Name { get; set; }
  
    public string Description { get; set; }
  
    public ushort AddressWidth { get; set; }
  
    public ushort DataWidth { get; set; }
  
    public uint SpeedMHz { get; set; }
  
    public uint BusSpeedMHz { get; set; }
  
    public ulong L2Cache { get; set; }
  
    public ulong L3Cache { get; set; }
  
    public uint Cores { get; set; }
  
    public uint Threads { get; set; }
  
    public ushort Architecture { get; set; }
  }
}
