
namespace Core.Common.MVVM
{
  public class IconAttribute : System.Attribute
  {
    public IconAttribute(string iconPath) { Icon = iconPath; }
    public string Icon { get; set; }
  }
}
