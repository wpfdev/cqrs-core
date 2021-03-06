﻿using System;
using System.Linq;
using System.Net;

namespace CQRS.Models
{
    public static class FailureExtensions
    {
        public static HttpStatusCode GetStatusCode(this Failure failure)
        {
            switch (failure)
            {
                case ExceptionFailure _:
                    return HttpStatusCode.InternalServerError;
                default:
                    return HttpStatusCode.BadRequest;
                case UnauthorizedFailure _:
                    return HttpStatusCode.Unauthorized;
                case ForbiddenFailure _:
                    return HttpStatusCode.Forbidden;
                case NotFoundFailure _:
                    return HttpStatusCode.NotFound;
            }
        }
    }

    public class Failure
    {
        public string Message { get; set; }

        public Failure(string message)
        {
            Message = message;
        }
    }

    public class NotFoundFailure : Failure
    {
        public NotFoundFailure(string message) : base(message)
        {
        }
    }

    public class UnauthorizedFailure : Failure
    {
        public UnauthorizedFailure(string message) : base(message)
        {
        }
    }

    public class ForbiddenFailure : Failure
    {
        public ForbiddenFailure(string message) : base(message)
        {
        }
    }

    public class ExceptionFailure : Failure
    {
        public string StackTrace { get; set; }

        public ExceptionFailure(Exception exception) : base(exception.Message)
        {
            StackTrace = exception.StackTrace;
        }

        public ExceptionFailure(string message) : base(message)
        {
        }
    }

    public class ValidationFailure : Failure
    {
        public ValidationError[] ValidationErrors { get; set; }

        public ValidationFailure(ValidationError[] validationErrors) : base("Validation Error")
        {
            if (validationErrors == null || !validationErrors.Any())
            {
                throw new ArgumentException(nameof(validationErrors));
            }

            ValidationErrors = validationErrors;
        }

        public ValidationFailure(ValidationError validationError) : this(new[] { validationError })
        {

        }
    }
}
