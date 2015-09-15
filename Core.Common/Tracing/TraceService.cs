using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Core.Common.Tracing
{
  [Export(typeof(ITraceService))]
  public class TraceService : ITraceService
  {
    class TraceServiceTraceListener : TraceListener
    {
      private ICollection<TraceMessage> messages = new ObservableCollection<TraceMessage>();
  
  
      public ICollection<TraceMessage> Messages { get { return messages; } }
      public override void Write(string message)
      {
        messages.Add(new TraceMessage { Message = message, MessageDate = DateTime.Now });
      }
  
      public override void WriteLine(string message)
      {
        messages.Add(new TraceMessage { Message = message, MessageDate = DateTime.Now });
      }
    }
    static TraceServiceTraceListener listener;
    static TraceService()
    {
      listener = new TraceServiceTraceListener();
      Trace.Listeners.Add(listener);
    }
  
  
  
    public IEnumerable<TraceMessage> TraceMessages
    {
      get { return listener.Messages; }
    }
  }
}
