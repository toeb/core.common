using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public interface IIdentityAssignable<TId> : IIdentifiable<TId>
  {
    TId Id { get; set; }
  }
}
