﻿using System.Net;

namespace Quiz_API.Exceptions
{
  public class ConflictException : Exception
  {
    public ConflictException() : base("Conflict") { }

    public ConflictException(string message) : base(message) { }

    public ConflictException(string message, Exception innerException) : base(message, innerException) { }

    public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
  }
}
