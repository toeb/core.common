using Core.Modules.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test
{

  [TestClass]
  public class CoreMessagingTest
  {
    [TestMethod]
    public void ShouldCreateMessageRequest()
    {
      var request = new MyRequest();
      Assert.IsNull(request.Parent);
    }
    [TestMethod]
    public void ShouldCreateResponse()
    {
      var request = new MyRequest();
      var response = request.CreateResponse();
      Assert.IsNotNull(response);
      Assert.AreEqual(response.Request, request);
      Assert.IsTrue(response is MyResponse);
    }

    [TestMethod]
    public void ShouldForwardRequest()
    {
      var request = new MyRequest();
      request.Values["key"] = "value";
      var forwardedRequest = request.Forward();
      Assert.IsTrue(forwardedRequest is MyRequest);
      var f = forwardedRequest as MyRequest;
      Assert.AreEqual("value", f.Values["key"]);
      Assert.AreNotSame(request.Values, f.Values);
    }

    class MyRequest : MessageRequest
    {
      public MyRequest()
      {
        Values = new Dictionary<string, string>();
      }
      public override MessageResponse CreateResponse()
      {
        return new MyResponse(this);
      }
      public Dictionary<string, string> Values { get; set; }
    }
    class MyResponse : MessageResponse
    {
      public MyResponse(MyRequest myRequest):base(myRequest)
      {
      }



    }
  }

  public class MessageResponse
  {
    private MessageRequest request;
    public MessageResponse(MessageRequest request) { this.request = request; }
    public MessageRequest Request { get { return request; } }
  }

  public class MessageRequest
  {

    public MessageRequest Parent { get; private set; }
    public virtual MessageRequest Forward() { var request = this.DeepClone(); request.Parent = this; return request; }
    public virtual MessageResponse CreateResponse() { return new MessageResponse(this); }
  }
}
