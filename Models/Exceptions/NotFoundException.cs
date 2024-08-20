using System.Net;

namespace Quiz_API.Exceptions
{
  public class NotFoundException : Exception
  {
    public NotFoundException() : base("The requested resource was not found.") { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
  }
}
