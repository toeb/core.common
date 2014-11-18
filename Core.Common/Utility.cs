using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class Utility
  {
    public static void Swap<T>(ref T a, ref T b)
    {
      T tmp = a;
      a = b;
      b = tmp;
    }
  }
}
