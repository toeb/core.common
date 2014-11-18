using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;

namespace Core.BitRot.Nodes
{

  /**
   * <summary> Tree node class.</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   */
  public class TreeNode : NotifyPropertyChangedBase
  {
    private int _depth = 0;
    private TreeNode _parent;

    private ICollection<TreeNode> _children = CollectionFactory.CreateCollection<TreeNode>();
    public IEnumerable<TreeNode> Children { get { return _children; } }

    protected void PrintIndented(string what)
    {
      for (int i = 0; i < Depth; i++)
      {
        Console.Write(" ");
      }
      Console.WriteLine(what);
    }

    public virtual void AddChild(TreeNode child)
    {
  
      child.Parent = this;
    }
    public virtual TreeNode Parent
    {
      get { return _parent; }
      set
      {
        if (value == _parent) return;
        
        if(_parent == null && value != null)
        {
          _parent = value;
          _parent._children.Add(this);
          _parent.OnChildAdded(this);
          Depth = _parent.Depth+1;
          RaisePropertyChanged("Parent");
          _parent.RaisePropertyChanged("Children");
          RaisePropertyChanged("Depth");
          return;
        }
        if(_parent != null && value == null)
        {
          _parent._children.Remove(this);
          _parent.OnChildRemoved(this);
          _parent.RaisePropertyChanged("Children");
          _parent = null;
          RaisePropertyChanged("Parent");
          return;
        }
        if(_parent != null && value != null)
        {
          _parent._children.Remove(this);
          _parent.OnChildRemoved(this);
          _parent.RaisePropertyChanged("Children");
          _parent = value;
          _parent._children.Add(this);
          _parent.OnChildAdded(this);
          _parent.RaisePropertyChanged("Children");
          RaisePropertyChanged("Parent");
          Depth = _parent.Depth + 1;
          return;
        }
      }
    }

    public int Depth
    {
      get { return _depth; }
      private set
      {
        if (_depth == value) return;
        _depth = value;
        RaisePropertyChanged("Depth");
      }
    }

    protected virtual void OnChildAdded(TreeNode child)
    {
   
    }
    protected virtual void OnChildRemoved(TreeNode treeNode)
    {
   
    }

    public virtual void RemoveChild(TreeNode treeNode)
    {
      treeNode.Parent = null;
    }
    
    public TreeNode()
    {

    }
  }
}