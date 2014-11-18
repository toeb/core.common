using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class Network
  {



    public static bool ServerExists(this Uri uri)
    {
      using (var ping = new Ping())
      {
        var reply = ping.Send(uri.Host);
        return reply.Status == IPStatus.Success;        
      }
    }
    public static bool IsPortOpen(this Uri uri)
    {
      try
      {
        using (TcpClient client = new TcpClient(uri.Host, uri.Port))
        {
          return client.Connected;
        }
      }
      catch 
      {
        return false;
      }
    }


  }
}
