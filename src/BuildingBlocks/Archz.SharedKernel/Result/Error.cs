using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archz.SharedKernel.Result;
public  class Error : IError
{
    /// <summary>
    /// Create a instance of Error with a StatusCode 400
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Descritive message</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
        Details = new List<Error>();
    }

    public string Code { get; }

    public string Message { get; }

    /// <summary>
    /// Add a new error to a list of errors
    /// </summary>
    /// <param name="error"></param>
    public Error AddErrorDetail(Error error)
    {
        if (error.Code.Equals(Code, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentNullException(nameof(error.Code));

        if (Details.Any(x => x.Code.Equals(error.Code, StringComparison.InvariantCultureIgnoreCase)))
            return error;

        Details.Add(error);

        return this;
    }

    /// <summary>
    /// List of errors
    /// </summary>
    public List<Error> Details { get; }
}
