using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.FileSystem
{
  class DirectoryDoesNotExistException : Exception
  {
    private string path;

    public DirectoryDoesNotExistException(string path)
    {
      // TODO: Complete member initialization
      this.path = path;
    }
  }
}
