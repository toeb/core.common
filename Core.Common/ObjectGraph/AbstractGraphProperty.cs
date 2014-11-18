using Core.ManagedObjects;
using Core.Values;

namespace Core.ObjectGraph
{
  public abstract class AbstractGraphProperty : ManagedForwardingProperty, IGraphProperty
  {
    public AbstractGraphProperty(string name, IGraphObject graphObjectValue, AbstractGraphObject @object, AbstractGraphDomain domain)
      : base(name, graphObjectValue, graphObjectValue)
    {
      this.Domain = domain;
      this.GraphObject = @object;
      ConnectGraphObjectValue(graphObjectValue);
    }


    protected override void OnBeforeValueConsumed(object newValue)
    {
      var oldValue = Produce();
      var oldGraphObject = Domain.Get(oldValue);
      if (oldGraphObject == null) return ;
      DisconnectGraphObjectValue(oldGraphObject);
    }


    protected override void OnValueConsumed(object newValue)
    {
      var newGraphObject = Domain.Get(newValue);
      if (newGraphObject == null) return;
      ConnectGraphObjectValue(newGraphObject);
    }

    private AbstractConnection propertyValueConnection = null;
    protected AbstractConnection PropertyValueConnection
    {
      get
      {
        if (propertyValueConnection == null)
        {
          propertyValueConnection = (AbstractConnection)ConstructPropertyValueConnection();
        }
        return propertyValueConnection;
      }
    }

    protected virtual IPropertyValueConnection ConstructPropertyValueConnection() { return Domain.ConstructPropertyValueConnection(this); }


    private void ConnectGraphObjectValue(IGraphObject newValue)
    {
      PropertyValueConnection.SetEdge(this, newValue as AbstractGraphObject);
      
    }

    private void DisconnectGraphObjectValue(IGraphObject oldValue)
    {
      PropertyValueConnection.Head = null;
    }

    protected AbstractGraphObject GraphObject { get; private set; }
    IGraphObject IGraphProperty.GraphObject { get { return GraphObject; } }

    public AbstractGraphDomain Domain { get; private set; }
    IGraphDomain IGraphDomainNode.Domain
    {
      get { return Domain; }
    }


  }
}
