using Core.FileSystem;
using Core.Strings;
using System.Collections.Generic;
using System.Linq;

namespace Core.FileSystem.Searching
{
  public static class PathSearchUtility
  {
    /// <summary>
    /// combines search directories and search pattersn to path patterns
    /// </summary>
    /// <param name="searchDirectories"></param>
    /// <param name="searchPatterns"></param>
    /// <returns></returns>
    public static IEnumerable<string> CombinePathPatterns(IEnumerable<string> searchDirectories, IEnumerable<string> searchPatterns)
    {
      return searchDirectories.SelectMany(directory => searchPatterns.Select(pattern => RelativePathUtility.Combine(directory, pattern))).Distinct();
    }

    /// <summary>
    /// fills path patterns with model data
    /// </summary>
    /// <param name="pathPatterns"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static IEnumerable<string> CreateSearchPaths(IEnumerable<string> pathPatterns, object model)
    {
      return pathPatterns.Select(pattern => RelativePathUtility.Normalize(pattern.FormatWith(model))).Distinct();
    }

    /// <summary>
    /// fills path patterns with multiple model data
    /// </summary>
    /// <param name="pathPatterns"></param>
    /// <param name="models"></param>
    /// <returns></returns>
    public static IEnumerable<string> CreateSearchPaths(
      IEnumerable<string> pathPatterns,
      IEnumerable<object> models)
    {
      return models.SelectMany(model => CreateSearchPaths(pathPatterns, model)).Distinct();
    }

  }
}
