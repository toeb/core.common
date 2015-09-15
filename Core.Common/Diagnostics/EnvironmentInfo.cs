using System;
using System.Linq;
using System.Management;

namespace Core.Common.Diagnostics
{
  [Serializable]
  public class EnvironmentInfo
  {
    public EnvironmentInfo()
    {
      UserName = Environment.UserName;
      UserDomainName = Environment.UserDomainName;
      MachineName = Environment.MachineName;
    }

    public override string ToString()
    {
      return UserName + "@" + MachineName;
    }
    public string UserName { get; set; }
  
    public string UserDomainName { get; set; }
  
    public string MachineName { get; set; }
  }
}
