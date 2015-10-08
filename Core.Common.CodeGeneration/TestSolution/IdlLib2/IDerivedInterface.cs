using Core.Annotation;
using IdlLib1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdlLib2
{

  public enum TrafficLightState
  {
    Red,
    Yellow,
    Green
  }
    public interface IDerivedInterface :IBaseInterface
    {
      
      string DerivedName { get; set; }
     
      TrafficLightState LightState { get; set; }
    
      [Version(2)]
      string Tags { get; set; } 
    }
}
