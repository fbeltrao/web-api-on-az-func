namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class FunctionControllerBase
    {
        private const string DefaultNotFoundErrorMessage = "Not found";

        protected ILogger Logger { get; private set; }

        public FunctionControllerBase(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected BadRequestObjectResult BadRequest(string errorMessage)
        {
            var body = new ApiErrorResponse
            {
                Error = errorMessage,
            };

            var res = new BadRequestObjectResult(body);
            return res;
        }

        protected NotFoundObjectResult NotFound(string errorMessage = null)
        {
            var response = new ApiErrorResponse
            {
                ErrorCode = ErrorCodes.NotFound,
                Error = errorMessage ?? DefaultNotFoundErrorMessage,
            };

            return new NotFoundObjectResult(response);
        }

        /// <summary>
        /// Returns an <see cref="OkObjectResult"/>.
        /// </summary>
        protected OkObjectResult OK(object value) => new OkObjectResult(value);

        /// <summary>
        /// Returns a <see cref="CreatedResult"/>.
        /// </summary>
        protected CreatedResult Created(string location, object value) => new CreatedResult(location, value);

        /// <summary>
        /// Returns a <see cref="ApiErrorResponse"/> containing the validation error message.
        /// Status code is <see cref="HttpStatusCode.BadRequest"/>.
        /// </summary>
        protected IActionResult ValidationFailed(string errorMessage) => ValidationFailed(new[] { new ValidationResult(errorMessage) });

        /// <summary>
        /// Returns a <see cref="ApiErrorResponse"/> containing the validation error messages.
        /// Status code is <see cref="HttpStatusCode.BadRequest"/>.
        /// </summary>
        protected IActionResult ValidationFailed(IList<ValidationResult> validationResult)
        {
            var errorMessages = validationResult.Select(x => x.ErrorMessage).ToList();
            var body = new ApiErrorResponse
            {
                ErrorCode = ErrorCodes.InvalidRequest,
                Error = errorMessages.Count == 1 ? errorMessages[0] : null,
                Errors = errorMessages.Count > 1 ? errorMessages : null
            };

            Logger.LogInformation("Validation failed: {message}", errorMessages);

            return new BadRequestObjectResult(body);
        }

        /// <summary>
        /// Returns a <see cref="ApiErrorResponse"/> containing the validation error messages.
        /// Status code is <see cref="HttpStatusCode.BadRequest"/>.
        /// </summary>
        protected IActionResult ValidationFailed(ApiValidationResult validationResult)
        {
            _ = validationResult ?? throw new ArgumentNullException(nameof(validationResult));
            return ValidationFailed(validationResult.ValidationResults);
        }

        /// <summary>
        /// Validates an object using <see cref="Validator"/>.
        /// </summary>
        protected ApiValidationResult Validate(object target)
        {
            _ = target ?? throw new ArgumentNullException(nameof(target));

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(target, new ValidationContext(target), validationResults, validateAllProperties: true))
            {
                return new ApiValidationResult(validationResults);
            }

            return ApiValidationResult.Valid;
        }

        /// <summary>
        /// Indicates that the request has failed, returning status code 500.
        /// The response body will be of type <see cref="ApiErrorResponse"/>.
        /// </summary>
        protected IActionResult Failed(string message, Exception ex)
        {
            Logger.LogError(ex, message);

            var errorResponse = new ApiErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Error = message,
            };

            var result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
            return result;
        }

        /// <summary>
        /// Indicates that application failed with an specific <see cref="HttpStatusCode"/>.
        /// The response body will be of type <see cref="ApiErrorResponse"/>.
        /// </summary>
        protected IActionResult Failed(HttpStatusCode statusCode, string errorMessage, string errorCode)
        {
            Logger.LogWarning(errorMessage);

            var errorResponse = new ApiErrorResponse
            {
                ErrorCode = errorCode,
                Error = errorMessage,
            };

            var result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)statusCode,
            };
            return result;
        }

        /// <summary>
        /// Tries to handle the error.
        /// </summary>
        /// <returns>
        /// Returns true if a an response should be returned (where body is of type <see cref="ApiErrorResponse"/>.
        /// Returns false if the exception should be bubbled up.
        /// </returns>
        protected virtual bool TryHandleError(Exception ex, out IActionResult response)
        {
            // Let fatal exception go through
            if (ex != null && IsFatal(ex))
            {
                response = null;
                return false;
            }

            response = Failed("Failed processing request", ex);
            return true;
        }

        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1013:ClosingBracesMustBeSpacedCorrectly",
            Justification = "Rule is incompatible with pattern matching in switch cases.")]
        private bool IsFatal(Exception ex)
        {
            re: switch (ex)
            {
                case OutOfMemoryException _:
                    return true;
                case AggregateException { InnerException: { }, // avoids allocation if collection is empty
                                          InnerExceptions: var iexs }:
                    for (var i = 0; i < iexs.Count; i++)
                    {
                        if (IsFatal(iexs[i]))
                            return true;
                    }

                    return false;
                case { InnerException: { } iex }:
                    ex = iex;
                    goto re; // simpler and as effective as tail call recursion
                default:
                    return false;
            }
        }

        protected IActionResult Run(Func<IActionResult> fn)
        {
            _ = fn ?? throw new ArgumentNullException(nameof(fn));

            try
            {
                return fn();
            }
            catch (Exception ex)
            {
                if (TryHandleError(ex, out var errorResponse))
                {
                    return errorResponse;
                }

                throw;
            }
        }

        protected async Task<IActionResult> RunAsync(Func<Task<IActionResult>> fn)
        {
            _ = fn ?? throw new ArgumentNullException(nameof(fn));

            try
            {
                return await fn().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (TryHandleError(ex, out var errorResponse))
                {
                    return errorResponse;
                }

                throw;
            }
        }
    }
}
