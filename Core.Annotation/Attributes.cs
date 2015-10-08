using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Annotation
{
    public enum VersionAction
  {
    Added,
    Removed,
    Changed
  }
    public class VersionAttribute : System.Attribute
    {
      public VersionAttribute(int version, VersionAction versionAction = VersionAction.Added) { }
    }
}
