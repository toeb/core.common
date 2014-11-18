using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.FileSystem
{
  class DirectoryIsNotEmptyException : Exception
  {
    private string path;

    public DirectoryIsNotEmptyException(string path)
    {
      // TODO: Complete member initialization
      this.path = path;
    }
  }
}
