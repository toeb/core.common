using System;
using System.Collections.Generic;
using System.Linq;
namespace Core.FileSystem.Searching
{

  public struct PathSearchOption
  {
    public string Option { get; private set; }
    public static implicit operator PathSearchOption(string option)
    {

      return new PathSearchOption() { Option = option };
    }

  }


  /// <summary>
  /// class for configuring path searches
  /// </summary>
  public class PathSearch
  {
    public PathSearch()
    {
      SearchPaths = new List<string>();
      SearchDirectories = new List<string>();
      SearchPatterns = new List<string>();
    }
    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> SearchPaths { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> SearchDirectories { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> SearchPatterns { get; set; }

    /// <summary>
    /// generates concrete path from the directories an  patterns
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public IEnumerable<string> GeneratePathsForModel(object model)
    {
      var patterns = SearchPaths.Concat(PathSearchUtility.CombinePathPatterns(SearchDirectories, SearchPatterns));
      return PathSearchUtility.CreateSearchPaths(patterns, model);
    }

    public IEnumerable<string> GeneratePathsForModels(IEnumerable<object> models)
    {
      var patterns = SearchPaths.Concat(PathSearchUtility.CombinePathPatterns(SearchDirectories, SearchPatterns));
      return PathSearchUtility.CreateSearchPaths(patterns, models);
    }

    public IEnumerable<string> GetExistingPathsForModel(Func<string, bool> exists, object model)
    {
      return RelativePathUtility.GetExistingPaths(exists, GeneratePathsForModel(model));
    }

  }


  public struct Path
  {
    private string path;
    public Path(string path)
    {
      this.path = path;
    }
    public override bool Equals(object obj)
    {
      if (!(obj is Path)) return false;
      var path = (Path)obj;
      return path.path == this.path;
    }
    public override int GetHashCode()
    {
      return path.GetHashCode();
    }

    public Path GetParent()
    {
      return new Path(RelativePathUtility.GetParentPath(path));
    }
  }
}
