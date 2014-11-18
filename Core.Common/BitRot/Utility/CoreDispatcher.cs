using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
  /**
   * <summary> Core dispatcher. The dispatch operation can be set to any action
   * 					 this is useful when using multithreading and specific operations
   * 					 need to be executed in a particular thread</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   */
  public class CoreDispatcher
  {

    /**
     * <summary> Gets or sets the dispatch oepration
     * 					 per default it just executes the code in place.</summary>
     *
     * <value> The dispatch.</value>
     */
    public static Action<Action> Dispatch
    {
      get
      {
        if (_dispatch == null) _dispatch = (a) => a();
        return _dispatch;
      }
      set
      {
        _dispatch = value;
      }
    }
    /// <summary> The dispatch operation field </summary>
    private static Action<Action> _dispatch = null;
  }
}
