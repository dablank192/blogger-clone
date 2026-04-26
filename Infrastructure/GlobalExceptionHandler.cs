using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using blogger_clone.Exception;
using blogger_clone.Exception.Auth;
using FluentValidation;
using blogger_clone.Model;
using blogger_clone.Exception.Blog;


namespace blogger_clone.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync (
        HttpContext context,
        System.Exception exception,
        CancellationToken ct)
    {
        _logger.LogError(exception, exception.Message);

        var problemDetail = new ProblemDetails
        {
            Instance= context.Request.Path
        };

        if (exception is EmailExistedException)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            problemDetail.Title = "An account is already linked to this email";
            problemDetail.Detail = exception.Message;
            problemDetail.Status = StatusCodes.Status409Conflict;
        }

        else if (exception is ValidationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetail.Title = "Invalid request data input";
            problemDetail.Detail = exception.Message;
            problemDetail.Status = StatusCodes.Status400BadRequest;
        }

        else if (exception is InvalidCredentialException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            problemDetail.Title = "Incorrect username or password";
            problemDetail.Detail = exception.Message;
            problemDetail.Status = StatusCodes.Status401Unauthorized;
        }

        else if (exception is BlogExistedException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetail.Title = "User already have a blog";
            problemDetail.Detail = exception.Message;
            problemDetail.Status = StatusCodes.Status400BadRequest;
        }

        else if (exception is BlogNotExistedException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            problemDetail.Title = "Blog not existed";
            problemDetail.Detail = exception.Message;
            problemDetail.Status = StatusCodes.Status404NotFound;
        }

        await context.Response.WriteAsJsonAsync(problemDetail, ct);

        return true;
    }
}