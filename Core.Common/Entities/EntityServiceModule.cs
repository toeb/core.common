using Core.Modules.Rules;
using System.ComponentModel.Composition;

namespace Core.Modules.Entities
{
  [Module(AutoDiscover=false)]
  public class EntityServiceModule
  {
    
    IRulesService RulesService { get; set; } 
    [ImportingConstructor]
    EntityServiceModule([Import] IRulesService service)
    {
      RulesService = service;
      EntityService = new EntityService(RulesService);
    }

    [Export]
    IEntityService EntityService { get; set; }
  }
}
