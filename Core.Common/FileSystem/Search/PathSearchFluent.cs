using System.Collections.Generic;
using System.Linq;

namespace Core.FileSystem.Searching
{
  public static class PathSearchFluent
  {
    /// <summary>
    /// adds a directory which is to be included in the path search
    /// </summary>
    /// <param name="self"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    public static PathSearch IncludeDirectory(this PathSearch self, string directory)
    {
      self.SearchDirectories.Add(directory);
      return self;
    }
    /// <summary>
    /// adds multiple directories which are to be included in search
    /// </summary>
    /// <param name="self"></param>
    /// <param name="directories"></param>
    /// <returns></returns>
    public static PathSearch IncludeDirectories(this PathSearch self, IEnumerable<string> directories)
    {
      foreach (var directory in directories)
      {
        self.SearchDirectories.Add(directory);
      }
      return self;
    }
    /// <summary>
    /// adds multiple directories to be included in search
    /// </summary>
    /// <param name="self"></param>
    /// <param name="directories"></param>
    /// <returns></returns>
    public static PathSearch IncludeDirectories(this PathSearch self, params string[] directories)
    {
      return self.IncludeDirectories(directories.AsEnumerable());
    }
    /// <summary>
    /// adds a pattern to the path search
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static PathSearch WithPattern(this PathSearch self, string pattern)
    {
      self.SearchPatterns.Add(pattern);
      return self;
    }
    /// <summary>
    /// adds multiple patterns to the path search
    /// </summary>
    /// <param name="self"></param>
    /// <param name="patterns"></param>
    /// <returns></returns>
    public static PathSearch WithPatterns(this PathSearch self, IEnumerable<string> patterns)
    {
      foreach (var pattern in patterns)
      {
        self.SearchPatterns.Add(pattern);
      }
      return self;
    }


    public static PathSearch WithPaths(this PathSearch self,IEnumerable<string> paths)
    {
      foreach (var path in paths)
      {
        self.SearchPaths.Add(path);
      }
      return self;
    }

    /// <summary>
    /// adds multiple patterns to the paths search
    /// </summary>
    /// <param name="self"></param>
    /// <param name="patterns"></param>
    /// <returns></returns>
    public static PathSearch WithPatterns(this PathSearch self, params string[] patterns)
    {
      return self.WithPatterns(patterns.AsEnumerable());
    }


  }
}
