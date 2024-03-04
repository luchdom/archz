

using System.Text.Json.Serialization;

namespace Archz.SharedKernel.Result;

//TODO - Replace FluenResults with this class
public class Result
{
    private Result(ContentResult content, ErrorResult errorResult) =>
            (Content, ErrorResult) = (content, errorResult);
    /// <summary>
    /// Content for the request in case a valid response
    /// </summary>
    public ContentResult Content { get; }

    /// <summary>
    /// ErrorResult for the request in case a invalid response
    /// </summary>
    public ErrorResult ErrorResult { get; }

    /// <summary>
    /// Checks whether the response contains invalid data
    /// </summary>
    [JsonIgnore]
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Checks whether the response contains valid data
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => VerifyResponseIsSuccess();

    private bool VerifyResponseIsSuccess()
        => EqualityComparer<ErrorResult>.Default.Equals(ErrorResult, default) || EqualityComparer<Error>.Default.Equals(ErrorResult.Error, default);

    /// <summary>
    /// Create a response with no content
    /// </summary>
    /// <returns></returns>
    public static Result Ok() => new Result(new ContentResult(), new ErrorResult());

    /// <summary>
    /// Create a response with no content
    /// </summary>
    /// <returns></returns>
    public static Result Ok(object obj) => new Result(ContentResult.Create(obj), new ErrorResult());

    /// <summary>
    /// Creates a response that contains the Error
    /// </summary>
    /// <param name="error">Validation Error - ( Error - ErrorResult )</param>
    /// <returns>Returns a result containing the Validation Error</returns>
    public static Result Fail(Error error)
    {
        if (error.Equals(default(Error)))
            throw new ArgumentNullException(nameof(error));

        return new Result(new ContentResult(), new ErrorResult(error));
    }
}
