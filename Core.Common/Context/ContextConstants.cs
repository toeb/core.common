using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Context
{
  /// <summary>
  /// contains constants for identifying context variables
  /// this should not be here since it "depends" on system.web etc 
  /// but currently it is out of convenience
  /// </summary>
  public static class Constants
  {
    public static readonly string RequestUserName = "request:userName";
    public static readonly string RequestUrl = "request:url";
    public static readonly string RequestHost= "request:host";
    public static readonly string ApplicationPhysicalPath = "application:physicalPath";
    public static readonly string ApplicationVirtualPath = "application:virtualPath";
    public static readonly string ApplicationId = "application:id";
    public static readonly string ApplicationContainer = "application:container";
    public static readonly string ApplicationDevelopementMode = "application:developement";
    public static readonly string ApplicationSiteName = "application:siteName";



  }
}
