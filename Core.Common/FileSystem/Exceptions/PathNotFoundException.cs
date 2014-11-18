using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.FileSystem
{
  class PathNotFoundException : Exception
  {
    private string p;
    private string path;

    public PathNotFoundException(string p)
    {
      // TODO: Complete member initialization
      this.p = p;
    }

    public PathNotFoundException(string p, string path)
    {
      // TODO: Complete member initialization
      this.p = p;
      this.path = path;
    }
  }
}
