using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public class DateTimeInterval
  {
    public DateTime Lower { get; set; }
    public DateTime Upper { get; set; }
    public DateTimeInterval(DateTime lower, DateTime upper)
    {
      Lower = lower;
      Upper = upper;
    }
    public DateTimeInterval() { }
    public bool IsIn(DateTime date)
    {
      return Lower <= date && Upper >= date;
    }
  }
}
