using System;
using System.ComponentModel.Composition;

namespace Core.Identifiers
{
  [Export]
  [Export(typeof(IIdentifierProvider))]
  [Export(typeof(IIdentifierProvider<Guid>))]
  public class GuidIdentifierProvider : AbstractIdentifierProvider<Guid>
  {
    public override Guid CreateIdentifier()
    {
      return Guid.NewGuid();
    }
    public override Guid DefaultId
    {
      get
      {
        return Guid.Empty;
      }
    }
  }

  public class IntIdentifierProvider : AbstractIdentifierProvider<int>
  {
    static int counter = 1;
    public override int CreateIdentifier()
    {
      return counter++;
    }
    public override int DefaultId
    {
      get
      {
        return 0;
      }
    }
  }
}
