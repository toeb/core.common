using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Core.Common
{

  public interface ISearchable
  {
    [XmlIgnore]
    IEnumerable<SearchTerm> SearchTerms { get; }
  }
}
