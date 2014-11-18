using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Core.FileSystem;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Core.FileSystem
{



  [Export(typeof(IRelativeFileSystem))]
  [Export]
  [DebuggerDisplay("relfs://{RootPath} {VirtualAbsolutePath}")]
  public class RelativeFileSystem : DelegateFileSystem, IRelativeFileSystem
  {
    public string RootPath
    {
      get { return VirtualRoot.RelativePath; }
    }

    public string VirtualAbsolutePath
    {
      get;
      private set;
    }

    public string AbsolutePath
    {
      get { return ToAbsolutePath("."); }
    }

    public string RelativePath
    {
      get;
      private set;
    }
    
    public IFileSystem Parent
    {
      get;
      private set;
    }
    public IFileSystem Root
    {
      get
      {
        return VirtualRoot.Parent;
      }
    }

    public IRelativeFileSystem VirtualRoot
    {
      get
      {

        if (Parent is IRelativeFileSystem) return (Parent as IRelativeFileSystem).VirtualRoot;
        return this;
      }
    }

    public RelativeFileSystem(string absolutePath)
      : this(new PhysicalFileSystem(), absolutePath)
    {

    }
    public RelativeFileSystem(IFileSystem parent, string relativePath)
    {
      AutoNormalize = true;
      Parent = parent;
      SetImplementation(Root);
      DelegateNormalizePath = RelativeNormalizePath;
      RelativePath = parent.NormalizePath(relativePath);

      if (RelativeParent!=null)
      {
        VirtualAbsolutePath = RelativeParent.ToVirtualAbsolutePath(RelativePath);
      }
      else
      {
        VirtualAbsolutePath = RelativePathUtility.Root;
      }

    }

    

    private string RelativeNormalizePath(string path)
    {
      return RelativePathUtility.Normalize(path);
    }

    protected override string TransformInputPath(string path)
    {
      return ToAbsolutePath(path);
    }
    protected override string TransformOutputPath(string absolutePath)
    {
      var path = AbsoluteToVirtualPath(absolutePath);
      return path;
    }


    public IRelativeFileSystem ScopeTo(string path)
    {
      path = ToVirtualAbsolutePath(path);
      return new RelativeFileSystem(this, path);
    }

    public string ToVirtualPath(string path)
    {
      return NormalizePath(path);
    }


    public override string NormalizePath(string path)
    {
      return RelativePathUtility.Normalize(path);
    }


    public string ToVirtualAbsolutePath(string path)
    {
      return RelativePathUtility.ToAbsoluteVirtual(VirtualAbsolutePath, path);
    }


    public new string ToAbsolutePath(string path)
    {
      var absolute = RelativePathUtility.ToAbsolute(RootPath, VirtualAbsolutePath, path);
      return Root.NormalizePath(absolute);
    }

    public string AbsoluteToVirtualAbsolutePath(string absolute)
    {
      var path = absolute.Substring(RootPath.Length);
      path = NormalizePath(path);
      return path;
    }
    public string AbsoluteToVirtualPath(string absolute)
    {
      var path = AbsoluteToVirtualAbsolutePath(absolute);
      path = VirtualAbsoluteToVirtualPath(path);
      return path;
    }
    public string VirtualAbsoluteToVirtualPath(string virtualAbsolute)
    {
      virtualAbsolute = NormalizePath(virtualAbsolute);
      if (Parent is RelativeFileSystem)
      {
        var path = virtualAbsolute.Substring(VirtualAbsolutePath.Length);
        path = NormalizePath(path);
        return path;
      }
      return virtualAbsolute;
    }




    public IRelativeFileSystem RelativeParent
    {
      get { return Parent as IRelativeFileSystem; }
    }
  }



}
