using FastEndpoints;
using FastProjects.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace FastProjects.Endpoints;

/// <summary>
/// Represents an endpoint with a request but without a response.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <param name="mediator">The mediator instance.</param>
public abstract class FastEndpointWithoutResponse<TRequest, TCommand, TResult>(IMediator mediator)
    : Endpoint<TRequest>
    where TRequest : notnull
    where TCommand : class
    where TResult : Result
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
            HttpContext.Response.StatusCode = SuccessStatusCode;
        }
        else
        {
            IResult webResult = (result as Result).ToWebResult();
            await SendResultAsync(webResult);
        }
    }

    /// <summary>
    /// Creates the mediator command from the request.
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <returns>The created command.</returns>
    protected abstract TCommand CreateMediatorCommand(TRequest request);

    /// <summary>
    /// Gets the success status code.
    /// </summary>
    protected virtual int SuccessStatusCode => StatusCodes.Status200OK;
}

/// <summary>
/// Represents an endpoint without a request and without a response.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <param name="mediator">The mediator instance.</param>
public abstract class FastEndpointWithoutResponse<TCommand, TResult>(IMediator mediator)
    : FastEndpointWithoutResponse<EmptyRequest, TCommand, TResult>(mediator)
    where TCommand : class
    where TResult : Result;
