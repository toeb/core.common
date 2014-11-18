using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class UriExtensions
  {
    public static bool IsEqualTo(this Uri current, Uri other, UriComponents components = UriComponents.AbsoluteUri)
    {
      return current.GetComponents(components, UriFormat.UriEscaped) == other.GetComponents(components, UriFormat.UriEscaped);
    }
    public static bool StartWith(this Uri current, Uri other)
    {
      return current.AbsoluteUri.StartsWith(other.AbsoluteUri);
    }
    public static string Without(this Uri current, Uri other)
    {
      if (!current.StartWith(other)) throw new ArgumentException("this uri needs to start with other uri","current");
      var result = current.AbsoluteUri.Substring(other.AbsoluteUri.Length);
      return result;
    }
    public static string GetRelativePath(this Uri current, Uri baseUri)
    {
      UriBuilder a = new UriBuilder(current);
      UriBuilder b=  new UriBuilder(baseUri);
      a.Query ="";
      b.Query = "";
      var result = a.Uri.Without(b.Uri);
      return result;
    }
    
  }
}
