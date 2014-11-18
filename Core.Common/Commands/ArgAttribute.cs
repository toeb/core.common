
namespace Core.Commands
{
  public class ArgAttribute : System.Attribute
  {
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string HelpText { get; set; }
    public bool Required { get; set; }
    public object DefaultValue { get; set; }
  }
}
