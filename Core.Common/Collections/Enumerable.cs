using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
  public static class Enumerable
  {
    /// <summary>
    /// Creates a Enumerable from params
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IEnumerable<T> Create<T>(params T[] args)
    {
      return args;

    }

    //System.Linq.Enumerable range repeat emtpy... are potentially hidden

  }
}
