using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Core.Common.Tracing
{
  public interface ITraceService
  {
    IEnumerable<TraceMessage> TraceMessages { get; }
  }
}
