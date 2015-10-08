using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Core.FileSystem;
using System.IO;

namespace Core.FileSystem
{
  public interface IRelativeReadonlyFilesystem : IReadonlyFileSystem
  {
    IFileSystem Parent { get; }
    IFileSystem Root { get; }
    IRelativeFileSystem VirtualRoot { get; }
    IRelativeFileSystem RelativeParent { get;  }

    /// <summary>
    /// returns the root path (the path of Root were the filesystem hierarchy starts)
    /// </summary>
    string RootPath
    {
      get;
    }
    /// <summary>
    /// returns the virtual root (the path from the Root to the current hierarchy node)
    /// </summary>
    string VirtualAbsolutePath
    {
      get;
    }
    /// <summary>
    /// returns the Absolutepath of this node (RootPath+VirtualAbsolutePath)
    /// </summary>
    string AbsolutePath
    {
      get;
    }
    /// <summary>
    /// returns the relative path from the Parent filesystem to current filesystem
    /// </summary>
    string RelativePath
    {
      get;
    }



    /// <summary>
    /// returns the virtual path relative to this filesystem instance
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    string ToVirtualPath(string path);
    /// <summary>
    /// returns the absolute virtual path relative to the base  relative file system
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    string ToVirtualAbsolutePath(string path);
    ///// <summary>
    ///// returns absolute path
    ///// </summary>
    ///// <param name="path"></param>
    ///// <returns></returns>
    //string ToAbsolutePath(string path);


    string AbsoluteToVirtualAbsolutePath(string absolute);
    string AbsoluteToVirtualPath(string absolute);
    string VirtualAbsoluteToVirtualPath(string virtualAbsoute);
  }
  public interface IRelativeFileSystem : IFileSystem, IRelativeReadonlyFilesystem
  {
  
  }




}
