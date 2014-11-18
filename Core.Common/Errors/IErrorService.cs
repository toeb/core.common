using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core
{


  public struct Error
  {
    public string Message { get; set; }
    public string Caller { get; set; }
    public int Line { get; set; }
    public string FilePath { get; set; }
    public Type ExceptionType { get; set; }
    public Exception InnerException { get; set; }
  }

  public interface IErrorService
  {
    void InvokeError(Error error);

    void RegisterError(string errorCode);
    void Error(string errorCode, params object[] args);
  }

  public static class IErrorServiceExtensions
  {
    public static void Error(this IErrorService service, string message, Type exceptionType = null, Exception innerException = null, [CallerMemberName] string caller = null, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null)
    {
      if (exceptionType == null) exceptionType = typeof(InvalidOperationException);
      var error = new Error() { Message = message, Caller = caller, Line = line, FilePath = path, ExceptionType = exceptionType, InnerException = innerException };
      service.InvokeError(error);
    }
    public static void Argument(this IErrorService service, string argument, string message, Exception innerException = null, [CallerMemberName] string caller = null, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null)
    {
      service.Error(message, typeof(ArgumentException), innerException, caller, line, path);
    }
    public static void InvalidOperation(this IErrorService service, string message, Type exceptionType = null, Exception innerException = null, [CallerMemberName] string caller = null, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null)
    {
      Error(service, message, exceptionType, innerException, caller, line, path);
    }
  }


  public class ErrorService : IErrorService
  {
    private static ErrorService instance;
    public static ErrorService Instance { get { return instance ?? (instance = new ErrorService()); } }
    public static readonly Type[] InnerExceptionConstructorTypeList = new[] { typeof(string), typeof(Exception) };
    public static readonly Type[] MessageExceptionConstructorTypeList = new[] { typeof(string) };
    public static readonly Type[] MessageDefaultConstructorTypeList = new Type[0];


    private static readonly string UnspecifiedError = "unspecified error";
    public void InvokeError(Error error)
    {
      if (error.ExceptionType == null)
      {
        error.ExceptionType = typeof(InvalidOperationException);
      }
      if (string.IsNullOrEmpty(error.Message))
      {
        error.Message = UnspecifiedError;
      }
      Exception exception = null;
      ConstructorInfo constructor = null;
      if (error.InnerException != null)
      {
        constructor = error.ExceptionType.GetConstructor(InnerExceptionConstructorTypeList);
        if (constructor != null)
        {
          exception = constructor.Invoke(null, new object[] { error.Message, error.InnerException }) as Exception;
          throw exception;
        }
      }

      constructor = error.ExceptionType.GetConstructor(MessageExceptionConstructorTypeList);
      if (constructor != null)
      {
        exception = constructor.Invoke(null, new object[] { error.Message }) as Exception;
        throw exception;
      }

      constructor = error.ExceptionType.GetConstructor(MessageDefaultConstructorTypeList);
      if (constructor != null)
      {
        exception = constructor.Invoke(null, new object[] { }) as Exception;
        throw exception;
      }

      throw new InvalidOperationException("could not create exception because it is missiung the appropriate constructors, message was " + error.Message);

    }


    public void RegisterError(string errorCode)
    {
    }




    public void Error(string errorCode, params object[] args)
    {
      this.InvalidOperation(errorCode);
    }
  }
}
