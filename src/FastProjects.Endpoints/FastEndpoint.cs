using FastEndpoints;
using FastProjects.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FastProjects.Endpoints;

/// <summary>
/// Represents an endpoint with a request and response.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <typeparam name="TResponseType">The type of the response data.</typeparam>
/// <param name="mediator">The mediator instance.</param>
public abstract class FastEndpoint<TRequest, TResponse, TCommand, TResult, TResponseType>(IMediator mediator)
    : Endpoint<TRequest, TResponse>
    where TRequest : notnull
    where TCommand : class
    where TResult : Result<TResponseType>
    where TResponseType : notnull
{
    /// <summary>
    /// Handles the request asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous handle operation.</returns>
    public override async Task HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        TCommand command = CreateMediatorCommand(request);
        TResult result = await mediator.Send(command, cancellationToken) as TResult
            ?? throw new InvalidCastException("The result must be of type Result<TResponseType>");

        if (result.IsSuccess)
        {
            TResponse response = CreateResponse(result);
            Response = response;
            HttpContext.Response.StatusCode = SuccessStatusCode;
        }
        else
        {
            await SendResultAsync(result.ToWebResult());
        }
    }

    /// <summary>
    /// Creates the mediator command from the request.
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <returns>The created command.</returns>
    protected abstract TCommand CreateMediatorCommand(TRequest request);

    /// <summary>
    /// Creates the response from the result data.
    /// </summary>
    /// <param name="data">The result data.</param>
    /// <returns>The created response.</returns>
    protected abstract TResponse CreateResponse(TResponseType data);

    /// <summary>
    /// Gets the success status code.
    /// </summary>
    protected virtual int SuccessStatusCode => StatusCodes.Status200OK;
}

/// <summary>
/// Represents an endpoint without a request but with a response.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <typeparam name="TResponseType">The type of the response data.</typeparam>
/// <param name="mediator">The mediator instance.</param>
public abstract class FastEndpoint<TResponse, TCommand, TResult, TResponseType>(IMediator mediator)
    : FastEndpoint<EmptyRequest, TResponse, TCommand, TResult, TResponseType>(mediator)
    where TCommand : class
    where TResult : Result<TResponseType>
    where TResponseType : notnull;
