using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{

  /// <summary>
  /// a path is a string 
  /// a path may contain / to denote segments
  /// a relative path starts with ~/
  /// an absolute path starts with /
  /// 
  /// a file is a path that does not end with / 
  /// a directory can be any path 
  /// a filename is the last segment of a path
  /// a filename is the segment after the last / in a normalized path
  /// "" is the null filename
  /// 
  /// 
  /// a filename may have an extension. it consists of the last . in the filename and whatever follows
  /// a path generally is case sensitive (users may ignore case sensitivity)
  /// 
  /// 
  /// a normalized directory ends with / 
  /// containing separated segments 
  /// 
  /// a path may not contain any invalid characters (Path.GetInvalidPathChars, Path.GetINvalidFileNameChars)
  /// a path may not contain //
  /// 
  /// a normalized path may not contain backslashes \ 
  /// 
  /// a path containing backslashes, //, /./ can be normalized
  /// a path containing .. will be normalized to the same path without the .. (.. does not work as expected)
  /// 
  /// 
  /// an absolute path starts with /
  /// (an absolute windows path starts with /$driveLetter$/ (/C/))
  /// a relative path starts with ~/
  /// 
  /// 
  /// </summary>
  public class RelativePathUtility
  {
    public static readonly char[] InvalidChars;
    static RelativePathUtility()
    {
      InvalidChars = Path.GetInvalidPathChars();
    }

    /// <summary>
    /// a algorithm for checking which paths exist (useful for many deep paths where only a small portion exists)
    /// first checks all directories 
    /// 
    /// algorithm does not change input order of paths
    /// </summary>
    /// <param name="exists">a function which returns true if the file or directory passed as a string exists</param>
    /// <param name="paths">the patsh to be searched</param>
    /// <returns></returns>
    public static IEnumerable<string> GetExistingPaths(Func<string, bool> exists, IEnumerable<string> paths)
    {
      var subPaths = paths
        .SelectMany(path => RelativePathUtility.GetSubSegments(path))
        .OrderBy(segments => segments.Count())
        .Select(segments => RelativePathUtility.CreatePath(segments))
        .Distinct()
        .ToArray();


      foreach (var path in subPaths)
      {
        if (!exists(path))
        {
          paths = paths.Where(p => !p.StartsWith(path));
        }
      }
      return paths.ToArray();
    }


    /// <summary>
    /// returns all subpaths for a path.
    /// e.g.  ~/a/b/c/d.txt
    /// would return
    /// ~/
    /// ~/a/
    /// ~/a/b/
    /// ~/a/b/c/
    /// ~/a/b/c/d.txt
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetSubPaths(string path)
    {
      var segments = GetSegments(path);
      for (int i = 0; i < segments.Count(); i++)
      {
        yield return CreatePath(segments.Take(i));
      }
    }
    public static bool IsPathValid(string path)
    {
      if (path.IndexOfAny(InvalidChars) > -1) return false;

      return true;
    }
    public static readonly char DirectorySeparator = '/';
    public static IEnumerable<string> GetSegments(string path)
    {
      path = Normalize(path);
      return path.Split(DirectorySeparator)
        // ignore root segment
        .Skip(1);
    }
    public static IEnumerable<IEnumerable<string>> GetSubSegments(string path)
    {
      return GetSubSegments(GetSegments(path));
    }
    public static IEnumerable<IEnumerable<string>> GetSubSegments(IEnumerable<string> segments)
    {
      for (int i = 0; i < segments.Count(); i++)
      {
        yield return segments.Take(i);
      }
    }

    public static string CreatePath(IEnumerable<string> segments)
    {
      var path = string.Join(DirectorySeparator.ToString(), segments);
      path = Normalize(path);
      return path;
    }
    /// <summary>
    /// returns the parent path for path, 
    /// if path is root, root is returned.
    /// if path is a file the containing directory is returned
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetParentPath(string path)
    {
      var segments = GetSegments(path);
      return CreatePath(segments.Take(segments.Count() - 1));
    }

    public static string ToAbsolute(string absoluteBase, string virtualbasePath, string path)
    {

      bool removeSlash = true;
      if (path.EndsWith("/") || path.EndsWith("\\")) removeSlash = false;
      path = ToAbsoluteVirtual(virtualbasePath, path);
      path = path.Replace("~", absoluteBase.Replace('\\', '/'));
      while (path.Contains("//")) path = path.Replace("//", "/");
      if (path.EndsWith("/") && removeSlash) path = path.Substring(0, path.Length - 1);
      return path;
    }

    public static string ToAbsoluteVirtual(string basePath, string path)
    {

      path = Normalize(path);
      if (!basePath.StartsWith("~")) return path;
      path = path.Replace("~", basePath);
      path = Normalize(path);
      return path;

    }
    public static string Normalize(string path)
    {
      StringBuilder builder = new StringBuilder(path.Replace('\\', '/'));
      while (true)
      {
        if (builder.Length == 0) break;
        if (builder[0] == '.' && builder.Length == 1) { builder.Remove(0, 1); break; }
        if (builder[0] == '.' && builder[1] == '/') { builder.Remove(0, 2); continue; }
        if (builder[0] == '/') { builder.Remove(0, 1); continue; }
        if (builder[0] == '~') { builder.Remove(0, 1); continue; }
        break;
      }
      builder.Insert(0, "~/");
      return builder.ToString();

    }
    public static string AbsoluteToVirtualAbsolute(string absoluteBase, string absolute)
    {
      var path = absolute.Substring(absoluteBase.Length);
      path = Normalize(path);
      return path;
    }
    public static string AbsoluteToVirtual(string virtualBase, string absoluteBase, string absolute)
    {
      var path = AbsoluteToVirtualAbsolute(absoluteBase, absolute);
      path = VirtualAbsoluteToVirtual(virtualBase, path);
      return path;
    }
    public static string VirtualAbsoluteToVirtual(string basePath, string virtualAbsoute)
    {
      virtualAbsoute = Normalize(virtualAbsoute);

      var path = virtualAbsoute.Substring(basePath.Length);
      path = Normalize(path);
      return path;

    }


    public static readonly string Root = "~/";

    /// <summary>
    /// combines two virtual paths 
    /// first normalizes both paths, then combines them
    /// </summary>
    /// <param name="requestSource"></param>
    /// <param name="path"></param>
    public static string Combine(string left, string right)
    {
      left = RemoveTrailingSlash(Normalize(left));
      right = Normalize(right);

      var result = right.Replace("~", left);
      return result;
    }
    /// <summary>
    /// unfinished. not tested. do not use!
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsNormalized(string path)
    {
      return Normalize(path) == path;
    }

    public static bool IsFile(string path)
    {
      if (path.EndsWith("/")) return false;
      return true;
    }
    public static bool IsDirectory(string path)
    {
      if (path.EndsWith("/")) return true;
      return true;
    }

    public static string GetFileName(string path)
    {
      path = Normalize(path);
      if (!IsFile(path)) return null;
      var filename = path.Substring(path.LastIndexOf('/') + 1);
      return filename;
    }

    public static string GetExtension(string path)
    {
      var filename = GetFileName(path);
      var index = filename.LastIndexOf('.');
      if (index < 0) return "";
      var extension = filename.Substring(index);
      return extension;
    }

    public static string GetDirectory(string path)
    {
      path = Normalize(path);
      var index = path.LastIndexOf('/');
      var result = path.Substring(0, index + 1);
      return result;
    }


    public static string ToAbsolute(string path)
    {
      path = Normalize(path);
      var absolutePath = path.Substring(1);
      return absolutePath;
    }

    public static string ToRelative(string path)
    {
      var relative = Normalize(path);
      return relative;
    }

    public static string AppendTrailingSlash(string path)
    {
      if (path.Last() == '/') return path;
      return path + "/";
    }

    public static string RemoveTrailingSlash(string path)
    {
      if (path.Last() != '/') return path;
      return path.Substring(0, path.Length - 1);
    }


    /// <summary>
    /// a path is absolute when it starts with a  / 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsAbsolute(string path)
    {
      return path.StartsWith("/");
    }


    /// <summary>
    /// a path is relative when it starts with a ~/
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsRelative(string path)
    {
      return path.StartsWith("~/");
    }



  }
}
