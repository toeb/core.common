using System.Diagnostics;

namespace Core.Common
{
  public static class Property
  {
  
  
    public static void CallerMemberName(ref string name)
    {
      if (name != null) return;
      StackFrame frame = new StackFrame(2);
      var method = frame.GetMethod();
      var type = method.DeclaringType;
      name = method.Name.Substring(4);
  
    }
  
  }
}
