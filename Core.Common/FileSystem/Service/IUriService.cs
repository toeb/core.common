using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{
  /// <summary>
  /// allows an uri to be re-written (similar to routes in asp)
  /// this allows for custom handling of relativ uri
  /// for example when working with modules
  /// </summary>
  public interface IUriService
  {
    Uri MapUri(Uri uri);
    Uri MapPath(string path);
  }

  [Export]
  [Export(typeof(IUriService))]
  public class DefaultUriService : IUriService
  {
    public Uri MapUri(Uri uri)
    {
      return uri;
    }

    public Uri MapPath(string path)
    {
      var uri = new Uri(Path.GetFullPath(path), UriKind.Absolute);
      return uri;
    } 

  }
}
