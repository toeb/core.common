using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.BitRot.Nodes
{
  /**
   * <summary> Node base. A Base class for nodes
   * 					 specifying the node storage</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   *
   * <typeparam name="TNodeType"> Type of the node type.</typeparam>
   */
  public class NodeBase<TNodeType > : Node<TNodeType>
    where TNodeType : NodeBase<TNodeType> 
  {
    /// <summary> The storage </summary>
    private INodeStorage<TNodeType> _storage;

    /**
     * <summary> Constructor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="storage"> The storage to be used.</param>
     */
    public NodeBase(INodeStorage<TNodeType> storage)
    {
      _storage = storage;
    }

    /**
     * <summary> Default constructor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     */
    public NodeBase() : this(null) { }

    /**
     * <summary> Executes the expand operation.</summary>
     *
     * <remarks> Tobi, 15.03.2012.</remarks>
     */
    protected override void DoExpand()
    {
      if (_storage == null) _storage = new NodeStorageMemoryLocal<TNodeType>(This);
    }

    /**
     * <summary> Gets the storage.</summary>
     *
     * <value> The storage.</value>
     */
    protected override INodeStorage<TNodeType> Storage
    {
      get { return _storage; }
    }
   
  }
}
