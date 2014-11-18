using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.FileSystem
{
  class PathExistsException : Exception
  {
    private string path;

    public PathExistsException(string path)
    {
      // TODO: Complete member initialization
      this.path = path;
    }
  }
}
