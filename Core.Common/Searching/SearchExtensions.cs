using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common
{
  public static class SearchExtensions
  {
    public static string[] GetSearchKeywords(this object @object)
    {
      var terms = @object.GetSearchTerms();
      if (terms == null) return null;
      return @object.GetSearchTerms().Where(st=>st.keywords!=null).SelectMany(st => st.keywords).ToArray();
    }
  
    public static IEnumerable<SearchTerm> GetSearchTerms(this object @object)
    {
  
      var searchable = @object as ISearchable;
      if (searchable != null) return searchable.SearchTerms;
      return null;
    }
    public static bool SearchPredicate(object @object, string searchString)
    {
      return RatePredicate(@object, searchString) >0;
    }
    public static int RatePredicate(this object @object, string searchString)
    {
  
      if (string.IsNullOrEmpty(searchString)) return 1;
  
      var keywords = @object.GetSearchKeywords();
      if (keywords == null) return 0;
      var rating = keywords.Count(kw => kw.ContainsIgnoreCase(searchString));
  
      return rating;
  
  
    }
  }
}
