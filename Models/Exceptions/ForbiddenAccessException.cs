using System.Net;

namespace Quiz_API.Exceptions
{
  public class ForbiddenAccessException : Exception
  {
    public ForbiddenAccessException() : base("Access forbidden.") { }

    public ForbiddenAccessException(string message) : base(message) { }

    public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException) { }

    public HttpStatusCode StatusCode { get; } = HttpStatusCode.Forbidden;
  }

}
