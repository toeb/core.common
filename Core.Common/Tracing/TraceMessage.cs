using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Core.Common.Tracing
{
  public class TraceMessage
  {
    public string Message { get; set; }
    public DateTime MessageDate { get; set; }

    public override string ToString()
    {
      return MessageDate + " " + Message;
    }
    public IEnumerable<SearchTerm> SearchTerms
    {
      get
      {
        return ToString().Split(' ', '\t').Select(t => (SearchTerm)t).ToArray();
      }
    }
  }
}
