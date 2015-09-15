
namespace Core.Common.MVVM
{
  public class CommandAttribute : System.Attribute
  {
    public CommandAttribute() { }
    public CommandAttribute(string name) { CommandName = name; }

    public string CommandName { get; set; }
    public string CanExecuteMember { get; set; }
  }
}
