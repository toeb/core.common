using Core.Common.Reflect;
using Core.Common.Reflect.Resources;
using System.Collections;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
namespace Core.Common.MVVM
{
  [Export(typeof(IResourceFinder))]
  public class IconResourceFinder : IResourceFinder
  {
    [Import]
    IResourceService ResourceService { get; set; }
    public object FindResource(object context, object identifier)
    {
      if (!(identifier is string)) return null;
      
      if (!(identifier as string).Equals("Icon")) return null;
      var type = context.GetType();
      var attr = type.GetCustomAttribute<IconAttribute>();
      if (attr == null) return null;
      var resource =ResourceService.GetResource(context, attr.Icon);

      return resource;

    }
  }
}
