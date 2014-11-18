using System.Collections.Generic;

namespace Core.BitRot.Nodes
{
  /**
 * <summary> Interface for node storage.</summary>
 *
 * <remarks> Tobi, 3/15/2012.</remarks>
 *
 * <typeparam name="TNodeType"> Type of the node type.</typeparam>
 */
  public interface INodeStorage<TNodeType> where TNodeType : Node<TNodeType>
  {
    /**
     * <summary> Deletes the successor from currentNode.</summary>
     *
     * <param name="currentNode"> The current node.</param>
     * <param name="successor">   The successor.</param>
     */
    void DeleteSuccessor(TNodeType currentNode, TNodeType successor);

    /**
     * <summary> Deletes the predecessor from currentNode.</summary>
     *
     * <param name="currentNode"> The current node.</param>
     * <param name="predecessor"> The predecessor.</param>
     */
    void DeletePredecessor(TNodeType currentNode, TNodeType predecessor);

    /**
     * <summary> Stores a successor of currentNode.</summary>
     *
     * <param name="currentNode"> The current node.</param>
     * <param name="successor">   The successor.</param>
     */
    void StoreSuccessor(TNodeType currentNode, TNodeType successor);

    /**
     * <summary> Stores a predecessor of currentNode.</summary>
     *
     * <param name="currentNode"> The current node.</param>
     * <param name="predecessor"> The predecessor.</param>
     */
    void StorePredecessor(TNodeType currentNode, TNodeType predecessor);

    /**
     * <summary> Enumerates get the predecessors  of currentNode.</summary>
     *
     * <param name="currentNode"> The current node.</param>
     *
     * <returns> An enumerator that allows foreach to be used to process get predecessors in this
     *           collection.</returns>
     */
    IEnumerable<TNodeType> GetPredecessors(TNodeType currentNode);

    /**
     * <summary> Enumerates get the successors of  the currentNode.</summary>
     *
     * <param name="currentNode"> The current node.</param>
     *
     * <returns> An enumerator that allows foreach to be used to process get successors in this
     *           collection.</returns>
     */
    IEnumerable<TNodeType> GetSuccessors(TNodeType currentNode);
  }
}
