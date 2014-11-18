using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public interface ISaveRestoreEnumerator<T> : IEnumerator<T>
  {
    IEnumerator<T> Restore();
  }

}
