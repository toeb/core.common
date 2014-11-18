using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{
  public interface IFileSystem : IReadonlyFileSystem
  {
    /// <summary>
    /// rerturns a stream to the specified file
    /// </summary>
    /// <param name="path"></param>
    /// <param name="access"></param>
    /// <returns></returns>
    Stream OpenFile(string path, FileAccess access);



    /// <summary>
    /// creates a file and returns a writable stream to it
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    Stream CreateFile(string path);

    /// <summary>
    /// "touches" a file causing its last change date to be updated to now and if it does not exist it will be created
    /// </summary>
    /// <param name="path"></param>
    void TouchFile(string path);


    /// <summary>
    /// creates a directory 
    /// </summary>
    /// <param name="path"></param>
    void CreateDirectory(string path);

    /// <summary>
    /// deletes the specified path
    /// </summary>
    /// <param name="path"></param>
    void Delete(string path);

    void DeleteDirectory(string path, bool recurse);
  }
}
