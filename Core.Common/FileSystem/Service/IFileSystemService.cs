using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Core.FileSystem;
using System.IO;
using System.Text;

namespace Core.FileSystem
{
  public interface IFileSystemService
  {
    CompositeFileSystem SharedFileSystem { get; }
    IFileSystem FileSystem { get; }
    IRelativeFileSystem ApplicationFileSystem { get; }
  }
}
