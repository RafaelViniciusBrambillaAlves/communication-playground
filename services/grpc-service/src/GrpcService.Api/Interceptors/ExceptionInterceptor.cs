using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcService.Domain.Exceptions;

namespace GrpcService.Api.Interceptors;

public sealed class ExceptionInterceptor(ILogger<ExceptionInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(ex, "User not found: {Message}", ex.Message);
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error: {Message}", ex.Message);
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (FormatException ex)
        {
            logger.LogWarning(ex, "Format error: {Message}", ex.Message);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Id Format."));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            throw new RpcException(new Status(StatusCode.Internal, "An internal error occurred."));
        }
    }
}