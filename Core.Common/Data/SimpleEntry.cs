using Core.Common.Reflect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Crypto;

namespace Core.Common.Data
{
  [DebuggerDisplay("Entry {Id}-{State} {Value}")]
  public class SimpleEntry : IEntry
  {
    public SimpleEntry(SimpleDataContext context)
    {
      this.state = EntityState.Detached;
      this.Context = context;
    }
    
    public object Id { get; set; }
    public object Value { get; set; }
    public object Hash { get; set; }
    public DateTime ChangeDate { get; set; }
    private EntityState state;
    private SimpleDataContext Context;
    public void SetContext(SimpleDataContext Context) { this.Context = Context; }
    public EntityState State
    {
      get { return state; }
      set
      {
        if (Context == null)
        {
          return;
        }
        var oldState = state;
  
        if (Context.ChangeState(this, state, value, newState => state = newState)) Trace.WriteLine("entry " + Id + " state change " + oldState + " => " + value);
      }
    }
  }
}
