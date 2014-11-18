using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.FileSystem
{
  class FileDoesNotExistException : Exception
  {
    private string path;

    public FileDoesNotExistException(string path)
    {
      // TODO: Complete member initialization
      this.path = path;
    }
  }
}
