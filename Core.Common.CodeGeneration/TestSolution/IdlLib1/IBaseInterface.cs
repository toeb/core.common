using Core.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdlLib1
{

  public interface ICustomConfiguration : IConfiguration
  {

  }

  public interface IConfiguration
  {

     string Path { get; set; }


    [Version(1)]
     string Name { get; set; }
  }
  public interface IBaseInterface
  {
    ColorType ColorType { get; set; }

    string Name { get; set; }


    [Version(2)]
    IConfiguration Configuration { get; set; }


    [Version(2, VersionAction.Removed)]
    string OtherName { get; set; }


  }

  public enum ColorType
  {
    Red,
    Green,
    Blue
  }
}
