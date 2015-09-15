using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Data
{
  public interface IShareService
  {
    ICollection<T> Collection<T>(string key) where T : class;
  
  }
}
