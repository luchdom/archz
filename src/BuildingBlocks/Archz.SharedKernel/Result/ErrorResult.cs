namespace Archz.SharedKernel.Result;
public class ErrorResult
{
    public ErrorResult()
    {
        
    }
    /// <summary>
    /// Create a new instance of ErrorResult
    /// </summary>
    /// <param name="error">Error instance</param>
    public ErrorResult(Error error)
    {
        Error = error;
    }

    /// <summary>
    /// Error of instance
    /// </summary>
    public Error Error { get; }
}
