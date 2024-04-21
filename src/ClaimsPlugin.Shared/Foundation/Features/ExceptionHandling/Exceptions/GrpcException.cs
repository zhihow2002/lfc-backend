using Grpc.Core;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

public class GrpcException : RpcException
{
    public GrpcException() : base(new Status(StatusCode.Cancelled, "No error was stated."), "No error was stated.")
    {
    }

    public GrpcException(string message) : base(new Status(StatusCode.Cancelled, message), message)
    {
    }

    // Temporarily join all strings as grpc exception does not support list of message.
    public GrpcException(string[] message) : this(string.Join(", ", message))
    {
    }
}

public class GrpcException<T> : RpcException
{
    public GrpcException() : base(new Status(StatusCode.Cancelled, "No error was stated."), "No error was stated.")
    {
    }

    public GrpcException(string message) : base(new Status(StatusCode.Cancelled, message), message)
    {
    }

    // Temporarily join all strings as grpc exception does not support list of message.
    public GrpcException(string[] message) : this(string.Join(", ", message))
    {
    }

    public GrpcException(IEnumerable<T> items, Func<T, string> func) : this(items.Select(func).ToArray())
    {
    }
}
